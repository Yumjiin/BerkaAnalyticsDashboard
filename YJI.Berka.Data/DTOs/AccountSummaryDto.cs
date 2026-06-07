using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.DTOs;

// <summary>
// 사이드바 계좌 목록용 DTO
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
    public int HighConfidenceCount { get; set; }
    public int TotalDays { get; set; }
    public DateTime? FirstTxDate { get; set; }
    public DateTime? LastTxDate { get; set; }

    public string DisplayName => $"Account {AccountId}";

    public string PeriodLabel { get; set; } = string.Empty;
    public string AnomalyLabel { get; set; } = string.Empty;
    public string DaysLabel { get; set; } = string.Empty;
    public string AnomalyBadgeVisibility { get; set; } = "Collapsed";
}
