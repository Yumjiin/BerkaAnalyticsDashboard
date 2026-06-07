using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.DTOs;

// <summary>
// Panel 4 — 월별 지출 비교 차트용 DTO
// MonthlySpendingFeature → MonthlySpendingRender 로 전달
// </summary>
public class MonthlySpendingDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalAmount { get; set; }

    /// <summary>전월 대비 증감률 (%) — null 이면 전월 데이터 없음</summary>
    public decimal? MomChangeRate { get; set; }

    public string Label => $"{Year}-{Month:D2}";
}
