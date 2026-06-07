using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.Entities;

public class AnomalyFlag
{
    // <summary>
    // anomaly_flags 테이블 - 3가지 이상 탐지 결과 (Python detection_pipeline 생성)
    // method: "zscore" | "isolation_forest" | "autoencoder"
    // </summary>

    public int Id { get; set; }
    public int AccountId { get; set; }
    public DateTime? Date { get; set; }
    public string Method { get; set; } = string.Empty;
    public float Score { get; set; }
    public bool IsAnomaly { get; set; }
    public DateTime CreatedAt { get; set; }
}