using System.Windows;
using System.Windows.Media;
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
            var main = new MainWindow(ConnectionString!);
            main.Show();
            Close();
        }

        private string BuildConnectionString()
        {
            return $"Server={TxtHost.Text};" +
                   $"Port={TxtPort.Text};" +
                   $"Database={TxtDatabase.Text};" +
                   $"Uid={TxtUsername.Text};" +
                   $"Pwd={PwdPassword.Password};";
        }

        private void TxtPort_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}