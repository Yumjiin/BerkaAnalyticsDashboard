using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.Entities;

public class HighConfidenceAnomaly
{
    // <summary>
    // high_confidence_anomalies 테이블 - 2개 이상 모델이 공통 탐지한 고신뢰 이상 거래
    // DetectedBy: "isolation_forest,zscore" | "autoencoder,isolation_forest,zscore" 등
    // ModelCount: 2 또는 3
    // </summary>

    public int Id { get; set; }
    public int AccountId { get; set; }
    public DateTime? Date { get; set; }
    public string DetectedBy { get; set; } = string.Empty;
    public int ModelCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
