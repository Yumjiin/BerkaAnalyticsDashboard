using System;
using System.Collections.Generic;
using System.Text;
using YJI.Berka.Data.Connection;
using YJI.Berka.Data.DTOs;
using Dapper;

namespace YJI.Berka.Data.Features;

// <summary>
// Panel 2 — 시간×요일 히트맵 데이터 가공
// raw_transactions → 7×24 행렬 → BehaviorHeatmapDto[]
// </summary>
public class BehaviorHeatmapFeature
{
    private readonly IConnectionFactory _factory;

    public BehaviorHeatmapFeature(IConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<BehaviorHeatmapDto>> LoadAsync(
        int accountId, DateTime start, DateTime end)
    {
        using var conn = _factory.Create();

        // MySQL에서 요일/시간별 집계
        // raw_transactions 에는 시간 정보가 없어서 date 기준 요일만 집계
        var rows = await conn.QueryAsync<BehaviorHeatmapDto>(@"
                SELECT WEEKDAY(date)    AS DayOfWeek,
                       0               AS Hour,
                       COUNT(*)        AS TxCount,
                       SUM(amount)     AS TotalAmount
                FROM   raw_transactions
                WHERE  account_id = @AccountId
                AND    date BETWEEN @Start AND @End
                AND    type = 'debit'
                GROUP  BY WEEKDAY(date)
                ORDER  BY DayOfWeek",
            new { AccountId = accountId, Start = start, End = end });

        return rows;
    }
}
