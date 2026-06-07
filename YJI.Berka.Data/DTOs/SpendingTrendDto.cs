using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.DTOs;

// <summary>
// Panel 1 — 일별 지출 추이 차트용 DTO
// SpendingTrendFeature → SpendingTrendRender 로 전달
// </summary>
public class SpendingTrendDto
{
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public int TxCount { get; set; }
    public bool IsAnomaly { get; set; }
    public float AnomalyScore { get; set; }
}
