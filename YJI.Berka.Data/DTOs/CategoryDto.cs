using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.DTOs;

// <summary>
// Panel 3 — 업종별 지출 분포 차트용 DTO
// CategoryFeature → CategoryRender 로 전달
// </summary>
public class CategoryDto
{
    public string KSymbol { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int TxCount { get; set; }
    public bool IsAnomalous { get; set; }
}
