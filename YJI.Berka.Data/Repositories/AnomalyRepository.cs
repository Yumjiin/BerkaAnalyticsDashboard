using YJI.Berka.Data.Connection;
using YJI.Berka.Data.Entities;
using Dapper;

namespace YJI.Berka.Data.Repositories;

public class AnomalyRepository : IAnomalyRepository
{
    private readonly IConnectionFactory _factory;

    public AnomalyRepository(IConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<AnomalyFlag>> GetByAccountAsync(
        int accountId, string? method = null)
    {
        using var conn = _factory.Create();

        var sql = @"
                SELECT id          AS Id,
                       account_id  AS AccountId,
                       date        AS Date,
                       method      AS Method,
                       score       AS Score,
                       is_anomaly  AS IsAnomaly,
                       created_at  AS CreatedAt
                FROM   anomaly_flags
                WHERE  account_id = @AccountId
                AND    is_anomaly = 1";

        if (method is not null)
            sql += " AND method = @Method";

        sql += " ORDER BY date";

        return await conn.QueryAsync<AnomalyFlag>(
            sql, new { AccountId = accountId, Method = method });
    }

    public async Task<IEnumerable<HighConfidenceAnomaly>> GetHighConfidenceAsync(int accountId)
    {
        using var conn = _factory.Create();
        return await conn.QueryAsync<HighConfidenceAnomaly>(@"
                SELECT id          AS Id,
                       account_id  AS AccountId,
                       date        AS Date,
                       detected_by AS DetectedBy,
                       model_count AS ModelCount,
                       created_at  AS CreatedAt
                FROM   high_confidence_anomalies
                WHERE  account_id = @AccountId
                ORDER  BY date",
            new { AccountId = accountId });
    }
}