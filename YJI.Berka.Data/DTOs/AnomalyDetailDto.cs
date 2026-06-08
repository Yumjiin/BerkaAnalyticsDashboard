using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.DTOs;

public class AnomalyDetailDto
{
    public DateTime? Date { get; set; }
    public decimal Amount { get; set; }
    public decimal? ZscoreValue { get; set; }
    public decimal? IforestScore { get; set; }
    public decimal? AeError { get; set; }
    public string DetectedBy { get; set; } = string.Empty;
    public int ModelCount { get; set; }

    // DataGrid 표시용
    public string DateLabel => Date.HasValue ? Date.Value.ToString("yyyy-MM-dd") : "-";
    public string AmountLabel => Amount.ToString("N0");
    public string ZscoreLabel => ZscoreValue.HasValue ? ZscoreValue.Value.ToString("F2") : "-";
    public string IforestLabel => IforestScore.HasValue ? IforestScore.Value.ToString("F4") : "-";
    public string AeLabel => AeError.HasValue ? AeError.Value.ToString("F6") : "-";
    public string ConfidenceLabel => $"{ModelCount} / 3";
}
