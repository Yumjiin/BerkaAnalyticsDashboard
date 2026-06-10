# BerkaAnalyticsDashboard

![C#](https://img.shields.io/badge/C%23-.NET_10-512BD4?logo=dotnet&logoColor=white)
![WPF](https://img.shields.io/badge/UI-WPF%2FXAML-512BD4)

실제 체코 은행 공개 데이터(Berka Dataset)의 ETL + 이상탐지 결과를 시각화하는 C# WPF 기반 분석 대시보드입니다.

> **연관 레포**: [BerkaETLPipeline](https://github.com/Yumjiin/BerkaETLPipeline)
> — ETL 파이프라인 + 3가지 독립적인 이상 탐지 모델 (Z-Score · Isolation Forest · Autoencoder)

---

## 스크린샷

### DB 연결 화면 (S-01)
<!-- 스크린샷 추가 예정 -->

### 메인 대시보드 (S-02)
<!-- 스크린샷 추가 예정 -->

### 이상 탐지 결과 (S-03)
<!-- 스크린샷 추가 예정 -->

### 설정 화면 (S-04)
<!-- 스크린샷 추가 예정 -->

---

## 화면 구성

| 화면 | 설명 |
|------|------|
| S-01 DB 연결 | MySQL 연결 정보 입력 및 연결 확인 |
| S-02 메인 대시보드 | 계좌 선택 + 기간 필터 + 4분할 차트 패널 |
| S-03 이상 탐지 결과 | 3개 모델 탐지 건수 카드 + 고신뢰 이상 거래 상세 테이블 |
| S-04 설정 | DB 연결 정보 확인 + 이상 탐지 임계값 참고 + 연결 끊기 |

### 4분할 패널

| 패널 | 차트 유형 | 설명 |
|------|-----------|------|
| Panel 1 - 일별 지출 추이 | ScottPlot 라인 | 일별 지출액 + 이상 탐지 날짜 수직선 |
| Panel 2 - 모델별 이상 탐지 비교 | ScottPlot 가로 막대 | Z-Score / IForest / AE / 고신뢰 건수 비교 |
| Panel 3 - 업종별 지출 분포 | ScottPlot 가로 막대 | 업종(k_symbol)별 총 지출액 |
| Panel 4 - 월별 지출 비교 | ScottPlot 세로 막대 | 월별 총지출 + 전월 대비 증감 색상 구분 |

---

## 기술 스택

| 분류 | 기술 |
|------|------|
| 언어 | C# / .NET 10 |
| UI | WPF / XAML |
| 아키텍처 | MVVM (DashboardViewModel) |
| 차트 | ScottPlot 5 |
| DB 연동 | MySql.Data, Dapper |
| 설정 | Microsoft.Extensions.Configuration |

---

## 아키텍처

5계층 구조로 관심사를 분리했습니다.

```
Docker MySQL (BerkaETLPipeline에서 적재)
    ↓
Repository Layer   Dapper로 MySQL 쿼리 실행
    ↓
Feature Layer      Entity → DTO 변환 및 가공
    ↓
Renderer Layer     ScottPlot 차트 렌더링
    ↓
View Layer         4분할 대시보드 + S-03 + S-04
    ↕
ViewModel Layer    계좌 목록 / 선택 상태 / 기간 필터 관리
```

### 프로젝트 구조

```
BerkaAnalyticsDashboard/
├── YJI.Berka.Data/                  데이터 레이어
│   ├── Connection/                  DB 연결 팩토리
│   ├── Entities/                    MySQL 테이블 매핑
│   ├── Repositories/                Dapper 쿼리
│   ├── DTOs/                        화면 전달용 가공 데이터
│   └── Features/                    Entity → DTO 변환
└── YJI.Berka.Dashboard/             UI 레이어
    ├── Renderers/                   ScottPlot 차트 렌더러
    │   ├── SpendingTrendRender.cs
    │   ├── AnomalyComparisonRender.cs
    │   ├── CategoryRender.cs
    │   └── MonthlySpendingRender.cs
    ├── ViewModels/
    │   └── DashboardViewModel.cs    계좌/기간 상태 관리
    └── Views/
        ├── StartupWindow.xaml       S-01 DB 연결
        ├── MainWindow.xaml          S-02 메인 대시보드
        ├── AnomalyResultWindow.xaml S-03 이상 탐지 결과
        └── SettingsWindow.xaml      S-04 설정
```

---

## 실행 방법

### 사전 요건

- .NET 10 SDK
- Visual Studio 2022 이상
- Docker Desktop
- [BerkaETLPipeline](https://github.com/Yumjiin/BerkaETLPipeline) 먼저 실행하여 DB 구축 필요

### 1. ETL 파이프라인 실행 (BerkaETLPipeline)

```bash
docker compose run --rm etl        # ETL 실행
docker compose run --rm detection  # 이상 탐지 실행
```

### 2. 대시보드 실행

```bash
git clone https://github.com/Yumjiin/BerkaAnalyticsDashboard.git
```

Visual Studio에서 `BerkaAnalyticsDashboard.slnx` 열기 후 `F5` 실행

### 3. DB 연결

실행 후 S-01 화면에서 DB 연결 정보를 입력합니다.

| 항목 | 기본값 |
|------|--------|
| Host | 127.0.0.1 |
| Port | 3306 |
| Database | berka |
| Username | berka_user |
| Password | berka1234 |

> Docker Desktop이 실행 중이어야 MySQL 컨테이너에 접속 가능합니다.

---

## 설계 문서

- [프로젝트 설계서](./docs/BerkaAnalytics_설계서.md)
- [기획서 + IA](./docs/BerkaAnalytics_기획서_IA.md)
- [Figma 와이어프레임](https://www.figma.com/design/apK5GQdChjRr2nldRsveic/BerkaAnalytics---Wireframe?node-id=0-1&t=RIcuQDwEJz1s34k1-1)
