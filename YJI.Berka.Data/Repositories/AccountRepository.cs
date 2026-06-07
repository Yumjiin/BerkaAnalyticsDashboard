using YJI.Berka.Data.Connection;
using YJI.Berka.Data.Entities;
using Dapper;

namespace YJI.Berka.Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly IConnectionFactory _factory;

    public AccountRepository(IConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IEnumerable<AccountProfile>> GetAllProfilesAsync()
    {
        using var conn = _factory.Create();
        return await conn.QueryAsync<AccountProfile>(@"
                SELECT account_id         AS AccountId,
                       total_days         AS TotalDays,
                       total_spent        AS TotalSpent,
                       avg_daily_spent    AS AvgDailySpent,
                       max_daily_spent    AS MaxDailySpent,
                       min_daily_spent    AS MinDailySpent,
                       std_daily_spent    AS StdDailySpent,
                       avg_daily_tx_count AS AvgDailyTxCount,
                       max_daily_tx_count AS MaxDailyTxCount,
                       first_tx_date      AS FirstTxDate,
                       last_tx_date       AS LastTxDate,
                       active_days        AS ActiveDays,
                       top_category       AS TopCategory,
                       total_anomaly_flags   AS TotalAnomalyFlags,
                       zscore_anomaly_count  AS ZscoreAnomalyCount,
                       iforest_anomaly_count AS IforestAnomalyCount,
                       ae_anomaly_count      AS AeAnomalyCount,
                       anomaly_rate          AS AnomalyRate,
                       updated_at            AS UpdatedAt
                FROM   account_profiles
                ORDER  BY account_id");
    }

    public async Task<AccountProfile?> GetProfileAsync(int accountId)
    {
        using var conn = _factory.Create();
        return await conn.QueryFirstOrDefaultAsync<AccountProfile>(@"
                SELECT account_id         AS AccountId,
                       total_days         AS TotalDays,
                       total_spent        AS TotalSpent,
                       avg_daily_spent    AS AvgDailySpent,
                       max_daily_spent    AS MaxDailySpent,
                       min_daily_spent    AS MinDailySpent,
                       std_daily_spent    AS StdDailySpent,
                       avg_daily_tx_count AS AvgDailyTxCount,
                       max_daily_tx_count AS MaxDailyTxCount,
                       first_tx_date      AS FirstTxDate,
                       last_tx_date       AS LastTxDate,
                       active_days        AS ActiveDays,
                       top_category       AS TopCategory,
                       total_anomaly_flags   AS TotalAnomalyFlags,
                       zscore_anomaly_count  AS ZscoreAnomalyCount,
                       iforest_anomaly_count AS IforestAnomalyCount,
                       ae_anomaly_count      AS AeAnomalyCount,
                       anomaly_rate          AS AnomalyRate,
                       updated_at            AS UpdatedAt
                FROM   account_profiles
                WHERE  account_id = @AccountId",
            new { AccountId = accountId });
    }

    public async Task<IEnumerable<DailySummary>> GetDailySummaryAsync(
        int accountId, DateTime start, DateTime end)
    {
        using var conn = _factory.Create();
        return await conn.QueryAsync<DailySummary>(@"
                SELECT id           AS Id,
                       account_id   AS AccountId,
                       date         AS Date,
                       total_amount AS TotalAmount,
                       tx_count     AS TxCount,
                       avg_amount   AS AvgAmount,
                       max_amount   AS MaxAmount
                FROM   daily_summary
                WHERE  account_id = @AccountId
                AND    date BETWEEN @Start AND @End
                ORDER  BY date",
            new { AccountId = accountId, Start = start, End = end });
    }

    public async Task<IEnumerable<CategorySummary>> GetCategorySummaryAsync(int accountId)
    {
        using var conn = _factory.Create();
        return await conn.QueryAsync<CategorySummary>(@"
                SELECT id           AS Id,
                       account_id   AS AccountId,
                       k_symbol     AS KSymbol,
                       total_amount AS TotalAmount,
                       tx_count     AS TxCount
                FROM   category_summary
                WHERE  account_id = @AccountId
                ORDER  BY total_amount DESC",
            new { AccountId = accountId });
    }
}