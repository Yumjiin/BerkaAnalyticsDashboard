using System.Windows;
using Dapper;
using Microsoft.Extensions.Configuration;
using YJI.Berka.Data.Connection;
using YJI.Berka.Data.DTOs;
using YJI.Berka.Data.Entities;

namespace YJI.Berka.Dashboard.Views;

public partial class AnomalyResultWindow : Window
{
    private readonly IConnectionFactory _factory;
    private readonly int _accountId;
    private readonly DateTime _start;
    private readonly DateTime _end;

    public AnomalyResultWindow(
        string connectionString, int accountId, DateTime start, DateTime end)
    {
        InitializeComponent();

        _accountId = accountId;
        _start = start;
        _end = end;

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:BerkaDb"] = connectionString
            })
            .Build();

        _factory = new ConnectionFactory(config);

        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        TxtSubtitle.Text = $"Account {_accountId} · {_start:yyyy-MM-dd} ~ {_end:yyyy-MM-dd}";
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        try
        {
            using var conn = _factory.Create();

            // 모델별 이상 탐지 건수
            var zscoreCount = await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM zscore_detail WHERE account_id = @Id AND is_anomaly = 1",
                new { Id = _accountId });

            var iforestCount = await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM iforest_detail WHERE account_id = @Id AND is_anomaly = 1",
                new { Id = _accountId });

            var aeCount = await conn.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM autoencoder_detail WHERE account_id = @Id AND is_anomaly = 1",
                new { Id = _accountId });

            TxtZscoreCount.Text = $"{zscoreCount:N0}건";
            TxtIforestCount.Text = $"{iforestCount:N0}건";
            TxtAeCount.Text = $"{aeCount:N0}건";

            // 고신뢰 이상 거래 조회
            var highConfidence = await conn.QueryAsync<HighConfidenceRow>(@"
                SELECT h.date         AS Date,
                       h.detected_by  AS DetectedBy,
                       h.model_count  AS ModelCount,
                       z.amount       AS Amount,
                       z.z_value      AS ZscoreValue,
                       f.score        AS IforestScore,
                       a.reconstruction_error AS AeError
                FROM   high_confidence_anomalies h
                LEFT JOIN zscore_detail z
                       ON z.account_id = h.account_id AND z.date = h.date
                LEFT JOIN iforest_detail f
                       ON f.account_id = h.account_id AND f.date = h.date
                LEFT JOIN autoencoder_detail a
                       ON a.account_id = h.account_id AND a.date = h.date
                WHERE  h.account_id = @Id
                ORDER  BY h.date DESC",
                new { Id = _accountId });

            var dtos = highConfidence.Select(r => new AnomalyDetailDto
            {
                Date = r.Date,
                Amount = r.Amount,
                ZscoreValue = r.ZscoreValue,
                IforestScore = r.IforestScore,
                AeError = r.AeError,
                DetectedBy = r.DetectedBy,
                ModelCount = r.ModelCount,
            }).ToList();

            DgAnomalies.ItemsSource = dtos;
            TxtTableTitle.Text =
                $"고신뢰 이상 거래 (2개 이상 모델 공통 탐지) — {dtos.Count:N0}건";
            TxtLastRefresh.Text = $"마지막 갱신: {DateTime.Now:HH:mm:ss}";
        }
        catch (Exception ex)
        {
            TxtStatus.Text = $"오류 — {ex.Message}";
        }
    }

    private void OnClose(object sender, RoutedEventArgs e) => Close();

    // Dapper 매핑용 내부 클래스
    private class HighConfidenceRow
    {
        public DateTime? Date { get; set; }
        public string DetectedBy { get; set; } = string.Empty;
        public int ModelCount { get; set; }
        public decimal Amount { get; set; }
        public decimal? ZscoreValue { get; set; }
        public decimal? IforestScore { get; set; }
        public decimal? AeError { get; set; }
    }
}