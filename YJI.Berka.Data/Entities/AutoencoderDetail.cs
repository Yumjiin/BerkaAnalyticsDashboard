using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.Entities;

public class AutoencoderDetail
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public DateTime? Date { get; set; }
    public decimal Amount { get; set; }
    public decimal ReconstructionError { get; set; }
    public decimal Threshold { get; set; }
    public bool IsAnomaly { get; set; }
}
