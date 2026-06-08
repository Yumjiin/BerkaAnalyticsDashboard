using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using YJI.Berka.Data.DTOs;
using YJI.Berka.Data.Features;

namespace YJI.Berka.Dashboard.ViewModels;

public class DashboardViewModel : INotifyPropertyChanged
{
    private readonly AccountSummaryFeature _accountSummaryFeature;

    // ── 계좌 목록 ──────────────────────────────────────
    private ObservableCollection<AccountSummaryDto> _accounts = new();
    public ObservableCollection<AccountSummaryDto> Accounts
    {
        get => _accounts;
        set { _accounts = value; OnPropertyChanged(); }
    }

    // ── 선택된 계좌 ────────────────────────────────────
    private AccountSummaryDto? _selectedAccount;
    public AccountSummaryDto? SelectedAccount
    {
        get => _selectedAccount;
        set { _selectedAccount = value; OnPropertyChanged(); }
    }

    // ── 기간 필터 ──────────────────────────────────────
    private DateTime _startDate = new DateTime(1993, 1, 1);
    public DateTime StartDate
    {
        get => _startDate;
        set { _startDate = value; OnPropertyChanged(); }
    }

    private DateTime _endDate = new DateTime(1998, 12, 31);
    public DateTime EndDate
    {
        get => _endDate;
        set { _endDate = value; OnPropertyChanged(); }
    }

    // ── 상태 ───────────────────────────────────────────
    private string _statusText = "DB: 연결됨";
    public string StatusText
    {
        get => _statusText;
        set { _statusText = value; OnPropertyChanged(); }
    }

    private string _accountCountText = "로딩 중...";
    public string AccountCountText
    {
        get => _accountCountText;
        set { _accountCountText = value; OnPropertyChanged(); }
    }

    // ── 생성자 ─────────────────────────────────────────
    public DashboardViewModel(AccountSummaryFeature accountSummaryFeature)
    {
        _accountSummaryFeature = accountSummaryFeature;
    }

    // ── 계좌 목록 로드 ─────────────────────────────────
    public async Task LoadAccountsAsync()
    {
        try
        {
            var accounts = await _accountSummaryFeature.LoadAsync();
            Accounts = new ObservableCollection<AccountSummaryDto>(accounts);
            AccountCountText = $"{Accounts.Count}개";
        }
        catch (Exception ex)
        {
            StatusText = $"DB: 오류 — {ex.Message}";
        }
    }

    // ── 검색 필터링 ────────────────────────────────────
    public IEnumerable<AccountSummaryDto> Filter(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword) || keyword == "계좌 검색...")
            return Accounts;

        return Accounts.Where(a => a.AccountId.ToString().Contains(keyword));
    }

    // ── INotifyPropertyChanged ─────────────────────────
    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}