using System;
using System.Collections.Generic;
using System.Text;
using YJI.Berka.Data.DTOs;
using YJI.Berka.Data.Repositories;

namespace YJI.Berka.Data.Features;

// <summary>
// Panel 3 — 업종별 지출 분포 데이터 가공
// CategorySummary → CategoryDto[]
// </summary>
public class CategoryFeature
{
    private readonly IAccountRepository _accountRepo;
    private readonly IAnomalyRepository _anomalyRepo;

    public CategoryFeature(
        IAccountRepository accountRepo,
        IAnomalyRepository anomalyRepo)
    {
        _accountRepo = accountRepo;
        _anomalyRepo = anomalyRepo;
    }

    public async Task<IEnumerable<CategoryDto>> LoadAsync(int accountId)
    {
        var categories = await _accountRepo.GetCategorySummaryAsync(accountId);

        // 이상 탐지된 업종 확인
        var anomalies = await _anomalyRepo.GetByAccountAsync(accountId);
        var anomalousDates = anomalies
            .Where(a => a.IsAnomaly && a.Date.HasValue)
            .Select(a => a.Date!.Value.Date)
            .ToHashSet();

        return categories.Select(c => new CategoryDto
        {
            KSymbol = c.KSymbol,
            TotalAmount = c.TotalAmount,
            TxCount = c.TxCount,
            IsAnomalous = false, // 업종 단위 이상 여부는 추후 확장
        });
    }
}
