using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.Entities;

public class CategorySummary
{
    // <summary>
    // category_summary 테이블 - 계좌별 업종(k_symbol)별 지출 집계
    // k_symbol: "household" | "services" | "insurance" | "pension" | "unknown" 등
    // </summary>

    public int Id { get; set; }
    public int AccountId { get; set; }
    public string KSymbol { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int TxCount { get; set; }
}
