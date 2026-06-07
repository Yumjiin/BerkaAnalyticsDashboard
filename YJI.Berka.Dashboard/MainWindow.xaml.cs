using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace YJI.Berka.Dashboard;

public partial class MainWindow : Window
{
    public string ConnectionString { get; }

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
        // 하단 상태바 업데이트
        TxtDbStatus.Text = "DB: 연결됨";
        TxtLastRefresh.Text = $"마지막 갱신: {DateTime.Now:HH:mm:ss}";

        // TODO: 계좌 목록 로드 (Repository 연동 후 구현)
        TxtAccountCount.Text = "로딩 중...";
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