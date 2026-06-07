using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.Entities;

public class AccountProfile
{
    // <summary>
    // account_profiles 테이블 - 계좌별 거래 프로파일 (Python feature_builder 생성)
    // </summary>

    public int AccountId { get; set; }
    public int TotalDays { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal AvgDailySpent { get; set; }
    public decimal MaxDailySpent { get; set; }
    public decimal MinDailySpent { get; set; }
    public decimal StdDailySpent { get; set; }
    public decimal AvgDailyTxCount { get; set; }
    public int MaxDailyTxCount { get; set; }
    public DateTime? FirstTxDate { get; set; }
    public DateTime? LastTxDate { get; set; }
    public int ActiveDays { get; set; }
    public string TopCategory { get; set; } = string.Empty;
    public int TotalAnomalyFlags { get; set; }
    public int ZscoreAnomalyCount { get; set; }
    public int IforestAnomalyCount { get; set; }
    public int AeAnomalyCount { get; set; }
    public decimal AnomalyRate { get; set; }
    public DateTime UpdatedAt { get; set; }
}
