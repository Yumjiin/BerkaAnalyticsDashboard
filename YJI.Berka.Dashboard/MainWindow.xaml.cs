using Microsoft.Extensions.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using YJI.Berka.Dashboard.Renderers;
using YJI.Berka.Dashboard.Views;
using YJI.Berka.Data.Connection;
using YJI.Berka.Data.DTOs;
using YJI.Berka.Data.Features;
using YJI.Berka.Data.Repositories;

namespace YJI.Berka.Dashboard;

public partial class MainWindow : Window
{
    public string ConnectionString { get; }

    // DB 연결
    private IConnectionFactory? _connectionFactory;

    // Features
    private AccountSummaryFeature? _accountSummaryFeature;
    private SpendingTrendFeature? _spendingTrendFeature;
    private CategoryFeature? _categoryFeature;
    private MonthlySpendingFeature? _monthlySpendingFeature;

    // Renderers
    private readonly SpendingTrendRender _spendingRender = new();
    private readonly AnomalyComparisonRender _anomalyRender = new();
    private readonly CategoryRender _categoryRender = new();
    private readonly MonthlySpendingRender _monthlyRender = new();

    // 계좌 목록
    private List<AccountSummaryDto> _accounts = new();

    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(string connectionString) : this()
    {
        ConnectionString = connectionString;
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        TxtDbStatus.Text = "DB: 연결됨";
        TxtLastRefresh.Text = $"마지막 갱신: {DateTime.Now:HH:mm:ss}";
        TxtAccountCount.Text = "로딩 중...";

        // ConnectionFactory 생성
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:BerkaDb"] = ConnectionString
            })
            .Build();

        _connectionFactory = new ConnectionFactory(config);

        // Feature 생성
        var accountRepo = new AccountRepository(_connectionFactory);
        var anomalyRepo = new AnomalyRepository(_connectionFactory);

        _accountSummaryFeature = new AccountSummaryFeature(accountRepo, _connectionFactory);
        _spendingTrendFeature = new SpendingTrendFeature(accountRepo, anomalyRepo);
        _categoryFeature = new CategoryFeature(accountRepo, anomalyRepo);
        _monthlySpendingFeature = new MonthlySpendingFeature(_connectionFactory);

        await LoadAccountsAsync();
    }

    private async Task LoadAccountsAsync()
    {
        try
        {
            var accounts = await _accountSummaryFeature!.LoadAsync();
            _accounts = accounts.ToList();

            LstAccounts.ItemsSource = _accounts;
            TxtAccountCount.Text = $"{_accounts.Count}개";
            TxtLastRefresh.Text = $"마지막 갱신: {DateTime.Now:HH:mm:ss}";
        }
        catch (Exception ex)
        {
            TxtDbStatus.Text = $"DB: 오류 — {ex.Message}";
        }
    }

    private async void LstAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LstAccounts.SelectedItem is not AccountSummaryDto selected) return;

        TxtCurrentAccount.Text = $"계좌: Account {selected.AccountId}";
        BtnDetect.IsEnabled = true;

        var start = DpStartDate.SelectedDate ?? new DateTime(1993, 1, 1);
        var end = DpEndDate.SelectedDate ?? new DateTime(1998, 12, 31);

        await RenderAllPanelsAsync(selected, start, end);
    }

    private async Task RenderAllPanelsAsync(
        AccountSummaryDto selected, DateTime start, DateTime end)
    {
        try
        {
            // Panel 1 — 일별 지출 추이
            var spendingData = await _spendingTrendFeature!
                .LoadAsync(selected.AccountId, start, end);
            _spendingRender.Render(Plot1SpendingTrend, spendingData);

            // Panel 2 — 모델별 이상 탐지 비교 (DB 쿼리 없이 DTO 직접 생성)
            var anomalyDto = new AnomalyComparisonDto
            {
                ZscoreCount = selected.ZscoreAnomalyCount,
                IforestCount = selected.IforestAnomalyCount,
                AeCount = selected.AeAnomalyCount,
                HighConfidenceCount = selected.HighConfidenceCount
            };
            _anomalyRender.Render(Plot2Heatmap, anomalyDto);

            // Panel 3 — 업종별 지출 분포
            var categoryData = await _categoryFeature!
                .LoadAsync(selected.AccountId);
            _categoryRender.Render(Plot3Category, categoryData);

            // Panel 4 — 월별 지출 비교
            var monthlyData = await _monthlySpendingFeature!
                .LoadAsync(selected.AccountId, start, end);
            _monthlyRender.Render(Plot4Monthly, monthlyData);

            // 상태바 갱신
            TxtDataCount.Text = $"데이터: {spendingData.Count()}일";
            TxtLastRefresh.Text = $"마지막 갱신: {DateTime.Now:HH:mm:ss}";
        }
        catch (Exception ex)
        {
            TxtDbStatus.Text = $"오류 — {ex.Message}";
        }
    }

    // 기간 필터 적용 버튼
    private async void BtnApply_Click(object sender, RoutedEventArgs e)
    {
        if (LstAccounts.SelectedItem is not AccountSummaryDto selected) return;

        var start = DpStartDate.SelectedDate ?? new DateTime(1993, 1, 1);
        var end = DpEndDate.SelectedDate ?? new DateTime(1998, 12, 31);

        await RenderAllPanelsAsync(selected, start, end);
    }

    // 검색창 Placeholder 처리
    private void TxtSearch_GotFocus(object sender, RoutedEventArgs e)
    {
        if (TxtSearch.Text == "계좌 검색...")
        {
            TxtSearch.Text = "";
            TxtSearch.Foreground = new SolidColorBrush(Colors.Black);
        }
    }

    private void TxtSearch_LostFocus(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtSearch.Text))
        {
            TxtSearch.Text = "계좌 검색...";
            TxtSearch.Foreground = new SolidColorBrush(
                Color.FromRgb(0xAA, 0xAA, 0xAA));
        }
    }

    private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (LstAccounts == null || _accounts == null || _accounts.Count == 0)
            return;

        // placeholder 텍스트이면 전체 목록 표시
        if (TxtSearch.Text == "계좌 검색...")
        {
            LstAccounts.ItemsSource = _accounts;
            return;
        }

        // 빈 문자열이면 전체 목록 표시
        if (string.IsNullOrWhiteSpace(TxtSearch.Text))
        {
            LstAccounts.ItemsSource = _accounts;
            return;
        }

        // AccountId로 필터링
        var filtered = _accounts
            .Where(a => a.AccountId.ToString().Contains(TxtSearch.Text.Trim()))
            .ToList();

        LstAccounts.ItemsSource = filtered;
    }
    private void BtnDetect_Click(object sender, RoutedEventArgs e)
    {
        if (LstAccounts.SelectedItem is not AccountSummaryDto selected) return;

        var start = DpStartDate.SelectedDate ?? new DateTime(1993, 1, 1);
        var end = DpEndDate.SelectedDate ?? new DateTime(1998, 12, 31);

        var window = new AnomalyResultWindow(
            ConnectionString, selected.AccountId, start, end);
        window.Show();
    }
    private void BtnSettings_Click(object sender, RoutedEventArgs e)
    {
        var settings = new SettingsWindow(ConnectionString);
        settings.Show();
    }
}