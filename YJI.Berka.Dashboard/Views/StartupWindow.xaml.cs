using System.Windows;
using MySql.Data.MySqlClient;

namespace YJI.Berka.Dashboard.Views
{
    public partial class StartupWindow : Window
    {
        public string? ConnectionString { get; private set; }

        public StartupWindow()
        {
            InitializeComponent();
            PwdPassword.Password = "berka1234";
        }

        private async void OnTestConnection(object sender, RoutedEventArgs e)
        {
            TxtStatus.Text = "연결 중...";
            TxtStatusBar.Text = "DB: 연결 중...";
            BtnStart.IsEnabled = false;

            var connStr = BuildConnectionString();

            try
            {
                using var conn = new MySqlConnection(connStr);
                await conn.OpenAsync();

                TxtStatus.Text = "✅ 연결 성공!";
                TxtStatusBar.Text = "DB: 연결됨";
                BtnStart.IsEnabled = true;
                ConnectionString = connStr;
            }
            catch (Exception ex)
            {
                TxtStatus.Text = $"❌ 연결 실패: {ex.Message}";
                TxtStatusBar.Text = "DB: 연결 실패";
                BtnStart.IsEnabled = false;
            }
        }

        private void OnStart(object sender, RoutedEventArgs e)
        {
            // 현재 창 크기와 위치를 MainWindow에 그대로 전달
            var main = new MainWindow(ConnectionString!)
            {
                Width = this.Width,
                Height = this.Height,
                Left = this.Left,
                Top = this.Top,
                WindowState = this.WindowState
            };

            main.Show();
            this.Close();
        }

        private string BuildConnectionString()
        {
            return $"Server={TxtHost.Text};" +
                   $"Port={TxtPort.Text};" +
                   $"Database={TxtDatabase.Text};" +
                   $"Uid={TxtUsername.Text};" +
                   $"Pwd={PwdPassword.Password};";
        }
    }
}