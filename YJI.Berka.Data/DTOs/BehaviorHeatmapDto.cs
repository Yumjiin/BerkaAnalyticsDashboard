using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.DTOs;

// <summary>
// Panel 2 — 시간×요일 히트맵용 DTO
// BehaviorHeatmapFeature → BehaviorHeatmapRender 로 전달
// 7(요일) × 24(시간) 2D 행렬
// </summary>
public class BehaviorHeatmapDto
{
    // <summary>0=월 ~ 6=일</summary>
    public int DayOfWeek { get; set; }

    // <summary>0~23시</summary>
    public int Hour { get; set; }

    public int TxCount { get; set; }
    public decimal TotalAmount { get; set; }
}
