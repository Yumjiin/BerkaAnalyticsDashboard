using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace YJI.Berka.Dashboard.Views;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
public partial class SettingsWindow : Window
{
    private readonly string _connectionString;
    private bool _disconnected = false;

    public SettingsWindow(string connectionString)
    {
        InitializeComponent();
        _connectionString = connectionString;
        Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // ConnectionString 파싱해서 각 필드 표시
        var parts = _connectionString
            .Split(';', StringSplitOptions.RemoveEmptyEntries)
            .Select(p => p.Split('=', 2))
            .Where(p => p.Length == 2)
            .ToDictionary(
                p => p[0].Trim().ToLower(),
                p => p[1].Trim()
            );

        TxtHost.Text = parts.GetValueOrDefault("server", "-");
        TxtPort.Text = parts.GetValueOrDefault("port", "-");
        TxtDatabase.Text = parts.GetValueOrDefault("database", "-");
        TxtUsername.Text = parts.GetValueOrDefault("uid", "-");
    }

    private void OnDisconnect(object sender, RoutedEventArgs e)
    {
        _disconnected = true;

        // StartupWindow 열기
        var startup = new StartupWindow();
        startup.Show();

        // 현재 MainWindow 닫기
        foreach (Window window in Application.Current.Windows)
        {
            if (window is MainWindow mainWindow)
            {
                mainWindow.Close();
                break;
            }
        }

        Close();
    }

    private void OnClose(object sender, RoutedEventArgs e) => Close();
}
