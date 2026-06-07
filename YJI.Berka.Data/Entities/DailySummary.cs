using System;
using System.Collections.Generic;
using System.Text;

namespace YJI.Berka.Data.Entities;

public class DailySummary
{
    // <summary>
    // daily_summary 테이블 - 계좌별 일별 지출 집계
    // Panel 1 (지출 추이 라인 차트) 의 주요 데이터 소스
    // </summary>

    public int Id { get; set; }
    public int AccountId { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public int TxCount { get; set; }
    public decimal AvgAmount { get; set; }
    public decimal MaxAmount { get; set; }
}