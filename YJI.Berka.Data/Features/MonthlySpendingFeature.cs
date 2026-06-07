using System;
using System.Collections.Generic;
using System.Text;
using YJI.Berka.Data.Connection;
using YJI.Berka.Data.DTOs;
using Dapper;

namespace YJI.Berka.Data.Features;

// <summary>
// Panel 4 — 월별 지출 비교 데이터 가공
// daily_summary → 월별 집계 → 전월 대비 증감률 계산 → MonthlySpendingDto[]
// </summary>
public class MonthlySpendingFeature
{
    private readonly IConnectionFactory _factory;

    public MonthlySpendingFeature(IConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<MonthlySpendingDto>> LoadAsync(
        int accountId, DateTime start, DateTime end)
    {
        using var conn = _factory.Create();

        var rows = await conn.QueryAsync<(int Year, int Month, decimal TotalAmount)>(@"
                SELECT YEAR(date)       AS Year,
                       MONTH(date)      AS Month,
                       SUM(total_amount) AS TotalAmount
                FROM   daily_summary
                WHERE  account_id = @AccountId
                AND    date BETWEEN @Start AND @End
                GROUP  BY YEAR(date), MONTH(date)
                ORDER  BY Year, Month",
            new { AccountId = accountId, Start = start, End = end });

        // 전월 대비 증감률 계산
        var list = rows.ToList();
        var result = new List<MonthlySpendingDto>();

        for (int i = 0; i < list.Count; i++)
        {
            var current = list[i];
            decimal? momRate = null;

            if (i > 0)
            {
                var prev = list[i - 1];
                if (prev.TotalAmount != 0)
                    momRate = (current.TotalAmount - prev.TotalAmount)
                              / prev.TotalAmount * 100m;
            }

            result.Add(new MonthlySpendingDto
            {
                Year = current.Year,
                Month = current.Month,
                TotalAmount = current.TotalAmount,
                MomChangeRate = momRate,
            });
        }

        return result;
    }
}
