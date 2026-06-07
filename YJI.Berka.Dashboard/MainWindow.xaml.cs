using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Extensions.Configuration;
using YJI.Berka.Data.Connection;
using YJI.Berka.Data.DTOs;
using YJI.Berka.Data.Features;
using YJI.Berka.Data.Repositories;

namespace YJI.Berka.Dashboard;

public partial class MainWindow : Window
{
    public string ConnectionString { get; }

    private IConnectionFactory? _connectionFactory;
    private AccountSummaryFeature? _accountSummaryFeature;
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
        // 상태바 초기화
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
        _accountSummaryFeature = new AccountSummaryFeature(accountRepo, _connectionFactory);

        // 계좌 목록 로드
        await LoadAccountsAsync();
    }

    private async Task LoadAccountsAsync()
    {
        try
        {
            var accounts = await _accountSummaryFeature!.LoadAsync();
            _accounts = accounts.ToList();

            // ListView 바인딩
            LstAccounts.ItemsSource = _accounts;
            TxtAccountCount.Text = $"{_accounts.Count}개";
            TxtLastRefresh.Text = $"마지막 갱신: {DateTime.Now:HH:mm:ss}";
        }
        catch (Exception ex)
        {
            TxtDbStatus.Text = $"DB: 오류 — {ex.Message}";
        }
    }

    private void LstAccounts_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (LstAccounts.SelectedItem is not AccountSummaryDto selected) return;

        TxtCurrentAccount.Text = $"계좌: Account {selected.AccountId}";
        // TODO: 4개 패널 갱신
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
            TxtSearch.Foreground = new SolidColorBrush(Color.FromRgb(0xAA, 0xAA, 0xAA));
        }
    }
}