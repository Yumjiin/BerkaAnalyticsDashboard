using System;
using System.Collections.Generic;
using System.Text;
using YJI.Berka.Data.DTOs;
using YJI.Berka.Data.Repositories;

namespace YJI.Berka.Data.Features;

// <summary>
// Panel 1 — 일별 지출 추이 데이터 가공
// DailySummary + AnomalyFlag → SpendingTrendDto[]
// </summary>
public class SpendingTrendFeature
{
    private readonly IAccountRepository _accountRepo;
    private readonly IAnomalyRepository _anomalyRepo;

    public SpendingTrendFeature(
        IAccountRepository accountRepo,
        IAnomalyRepository anomalyRepo)
    {
        _accountRepo = accountRepo;
        _anomalyRepo = anomalyRepo;
    }

    public async Task<IEnumerable<SpendingTrendDto>> LoadAsync(
        int accountId, DateTime start, DateTime end)
    {
        // 일별 지출 데이터
        var daily = await _accountRepo.GetDailySummaryAsync(accountId, start, end);

        // 이상 탐지 결과 (고신뢰 이상만)
        var anomalies = await _anomalyRepo.GetHighConfidenceAsync(accountId);
        var anomalyDates = anomalies
            .Where(a => a.Date.HasValue)
            .ToDictionary(a => a.Date!.Value.Date, a => a);

        return daily.Select(d => new SpendingTrendDto
        {
            Date = d.Date,
            TotalAmount = d.TotalAmount,
            TxCount = d.TxCount,
            IsAnomaly = anomalyDates.ContainsKey(d.Date.Date),
            AnomalyScore = anomalyDates.TryGetValue(d.Date.Date, out var flag)
                           ? flag.ModelCount : 0f,
        });
    }
}
