using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.DTOs;

// <summary>
// S-03 이상 탐지 결과 화면용 DTO
// AnomalyRepository → AnomalyResultViewModel 로 전달
// </summary>
public class AnomalyResultDto
{
    public DateTime? Date { get; set; }
    public string Method { get; set; } = string.Empty;
    public float Score { get; set; }
    public int ModelCount { get; set; }
    public string DetectedBy { get; set; } = string.Empty;

    /// <summary>3개 모델 모두 탐지 여부</summary>
    public bool IsHighConfidence => ModelCount >= 3;
}
