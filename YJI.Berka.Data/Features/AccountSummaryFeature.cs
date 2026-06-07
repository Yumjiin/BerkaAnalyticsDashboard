using System;
using System.Collections.Generic;
using System.Text;
using YJI.Berka.Data.DTOs;
using YJI.Berka.Data.Repositories;

namespace YJI.Berka.Data.Features;

// <summary>
// 사이드바 계좌 목록 데이터 가공
// AccountProfile → AccountSummaryDto[]
// </summary>
public class AccountSummaryFeature
{
    private readonly IAccountRepository _accountRepo;

    public AccountSummaryFeature(IAccountRepository accountRepo)
    {
        _accountRepo = accountRepo;
    }

    public async Task<IEnumerable<AccountSummaryDto>> LoadAsync()
    {
        var profiles = await _accountRepo.GetAllProfilesAsync();

        return profiles.Select(p => new AccountSummaryDto
        {
            AccountId = p.AccountId,
            TotalSpent = p.TotalSpent,
            AnomalyRate = p.AnomalyRate,
            TopCategory = p.TopCategory,
            ZscoreAnomalyCount = p.ZscoreAnomalyCount,
            IforestAnomalyCount = p.IforestAnomalyCount,
            AeAnomalyCount = p.AeAnomalyCount,
        });
    }
}
