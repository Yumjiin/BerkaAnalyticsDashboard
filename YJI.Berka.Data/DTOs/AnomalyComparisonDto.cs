using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.DTOs;

// <summary>
// Panel 2 — 모델별 이상 탐지 비교 차트용 DTO
// AccountSummaryDto → AnomalyComparisonRender 로 전달
// </summary>
public class AnomalyComparisonDto
{
    public int ZscoreCount { get; set; }
    public int IforestCount { get; set; }
    public int AeCount { get; set; }
    public int HighConfidenceCount { get; set; }
}
