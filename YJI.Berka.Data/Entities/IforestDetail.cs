using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.Entities;

public class IforestDetail
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public DateTime? Date { get; set; }
    public decimal Amount { get; set; }
    public int TxCount { get; set; }
    public decimal AvgAmount { get; set; }
    public decimal MaxAmount { get; set; }
    public decimal Score { get; set; }
    public bool IsAnomaly { get; set; }
}
