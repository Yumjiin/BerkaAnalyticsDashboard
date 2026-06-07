using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.Entities;

public class RawTransaction
{
    // <summary>
    // raw_transactions 테이블 - 원본 거래 데이터 (1,056,320행)
    // type: "credit" | "debit"
    // operation: "deposit" | "withdrawal" | "card_withdrawal" | "transfer_in" | "transfer_out" | "unknown"
    // k_symbol: "household" | "services" | "insurance" | "interest" | "pension" | "unknown" 등
    // </summary>

    public int TransId { get; set; }
    public int AccountId { get; set; }
    public DateTime? Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }
    public string KSymbol { get; set; } = string.Empty;
    public string? Bank { get; set; }
    public string? Account { get; set; }
}
