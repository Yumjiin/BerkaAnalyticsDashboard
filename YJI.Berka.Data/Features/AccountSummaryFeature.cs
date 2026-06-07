using System;
using System.Collections.Generic;
using System.Text;
using YJI.Berka.Data.Connection;
using YJI.Berka.Data.DTOs;
using YJI.Berka.Data.Repositories;
using Dapper;

namespace YJI.Berka.Data.Features;

// <summary>
// 사이드바 계좌 목록 데이터 가공
// AccountProfile + HighConfidenceAnomaly → AccountSummaryDto[]
// </summary>
public class AccountSummaryFeature
{
    private readonly IAccountRepository _accountRepo;
    private readonly IConnectionFactory _factory;

    public AccountSummaryFeature(
        IAccountRepository accountRepo,
        IConnectionFactory factory)
    {
        _accountRepo = accountRepo;
        _factory = factory;
    }

    public async Task<IEnumerable<AccountSummaryDto>> LoadAsync()
    {
        var profiles = await _accountRepo.GetAllProfilesAsync();

        // 계좌별 고신뢰 이상 건수 조회
        using var conn = _factory.Create();
        var highConfidenceCounts = await conn.QueryAsync<(int AccountId, int Count)>(@"
                SELECT account_id AS AccountId,
                       COUNT(*)   AS Count
                FROM   high_confidence_anomalies
                GROUP  BY account_id");

        var countDict = highConfidenceCounts
            .ToDictionary(x => x.AccountId, x => x.Count);

        return profiles.Select(p => new AccountSummaryDto
        {
            AccountId = p.AccountId,
            TotalSpent = p.TotalSpent,
            AnomalyRate = p.AnomalyRate,
            TopCategory = p.TopCategory,
            ZscoreAnomalyCount = p.ZscoreAnomalyCount,
            IforestAnomalyCount = p.IforestAnomalyCount,
            AeAnomalyCount = p.AeAnomalyCount,
            HighConfidenceCount = countDict.TryGetValue(p.AccountId, out var cnt) ? cnt : 0,
            PeriodLabel = p.FirstTxDate.HasValue && p.LastTxDate.HasValue
            ? $"{p.FirstTxDate.Value:yyyy-MM} ~ {p.LastTxDate.Value:yyyy-MM}"
            : "-",
            AnomalyLabel = $"Z:{p.ZscoreAnomalyCount} IF:{p.IforestAnomalyCount} AE:{p.AeAnomalyCount}",
            DaysLabel = $"{p.TotalDays:N0}일",
            AnomalyBadgeVisibility = (p.ZscoreAnomalyCount > 0 || p.IforestAnomalyCount > 0 || p.AeAnomalyCount > 0)
            ? "Visible" : "Collapsed",
        });
    }
}
