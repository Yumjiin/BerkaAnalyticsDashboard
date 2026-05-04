# BerkaAnalyticsDashboard

실제 체코 은행 공개 데이터(Berka Dataset)를 시각화하는
WPF 기반 4분할 분석 대시보드입니다.

> **연관 레포**: [BerkaETLPipeline](https://github.com/{Yumjiin}/BerkaETLPipeline) — ETL 파이프라인 + 이상 탐지

---

## 화면 구성

```
┌──────────────────────┬──────────────────────┐
│  패널 1               │  패널 2               │
│  일별 지출 추이        │  시간×요일 히트맵      │
│  (ScottPlot 라인)     │  (SkiaSharp)          │
├──────────────────────┼──────────────────────┤
│  패널 3               │  패널 4               │
│  업종별 지출 분포      │  월별 지출 비교        │
│  (ScottPlot 막대)     │  (ScottPlot 막대)     │
└──────────────────────┴──────────────────────┘
```

---

## 기술 스택

| 분류 | 기술 |
|------|------|
| 언어 | C# / .NET 10 |
| UI | WPF / XAML / MVVM |
| 그래프 | ScottPlot, SkiaSharp |
| DB 연동 | MySql.Data, Dapper |
| 아키텍처 | 5계층 (Entry → Feature → Render → Service → Repository) |

---

## 아키텍처

인턴 프로젝트(EventViewer)와 동일한 5계층 구조를 유지하면서
데이터 소스를 파일(JSON/WAV) → MySQL DB로 심화했습니다.

```
EventViewer (인턴)     BerkaAnalytics (이 프로젝트)
────────────────────────────────────────────────
JSON/WAV 파일 읽기  →  MySQL 쿼리
NoiseLevelFeature   →  SpendingTrendFeature
MelSpectrogramFeature → BehaviorHeatmapFeature
ClipLabelRepository →  AnomalyRepository
```

---

## 실행 방법

### 사전 요건
- .NET 10 SDK
- Visual Studio 2022
- MySQL 8.0 (BerkaETLPipeline 먼저 실행 필요)

### 실행

```bash
# 1. 레포 클론
git clone https://github.com/{Yumjiin}/BerkaAnalyticsDashboard.git

# 2. Visual Studio에서 .sln 파일 열기

# 3. appsettings.json에 DB 연결 정보 입력

# 4. F5 실행
```

---

## 설계 문서

- [프로젝트 설계서](./docs/BerkaAnalytics_설계서.md)
- [기획서 + IA](./docs/BerkaAnalytics_기획서_IA.md)
- [Figma 와이어프레임](https://www.figma.com/design/apK5GQdChjRr2nldRsveic/BerkaAnalytics---Wireframe?node-id=0-1&t=RIcuQDwEJz1s34k1-1)
