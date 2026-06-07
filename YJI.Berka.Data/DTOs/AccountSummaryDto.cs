using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.DTOs;

// <summary>
// 사이드바 계좌 목록용 DTO
// AccountRepository → DashboardViewModel 로 전달
// </summary>
public class AccountSummaryDto
{
    public int AccountId { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal AnomalyRate { get; set; }
    public string TopCategory { get; set; } = string.Empty;
    public int ZscoreAnomalyCount { get; set; }
    public int IforestAnomalyCount { get; set; }
    public int AeAnomalyCount { get; set; }

    public string DisplayName => $"Account {AccountId}";
}
