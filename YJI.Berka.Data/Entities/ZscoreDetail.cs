using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.Entities;

public class ZscoreDetail
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public DateTime? Date { get; set; }
    public decimal Amount { get; set; }
    public decimal Mean { get; set; }
    public decimal Std { get; set; }
    public decimal ZValue { get; set; }
    public bool IsAnomaly { get; set; }
}
