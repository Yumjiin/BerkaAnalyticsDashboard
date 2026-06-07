# BerkaAnalyticsDashboard

![C#](https://img.shields.io/badge/C%23-.NET_10-512BD4?logo=dotnet&logoColor=white)
![WPF](https://img.shields.io/badge/UI-WPF%2FXAML-512BD4)
![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?logo=mysql&logoColor=white)
![ScottPlot](https://img.shields.io/badge/Chart-ScottPlot-blue)
![SkiaSharp](https://img.shields.io/badge/Render-SkiaSharp-pink)

실제 체코 은행 공개 데이터(Berka Dataset)를 시각화하는 WPF 기반 4분할 분석 대시보드입니다.

> **연관 레포**: [BerkaETLPipeline](https://github.com/Yumjiin/BerkaETLPipeline)
> — ETL 파이프라인 + 이상 탐지 (Z-Score → Isolation Forest → Autoencoder)

---

## 화면 구성

| 패널 1 — 일별 지출 추이 | 패널 2 — 시간×요일 히트맵 |
|------------------------|--------------------------|
| ScottPlot 라인 차트     | SkiaSharp 히트맵          |
| **패널 3** — 업종별 지출 분포 | **패널 4** — 월별 지출 비교 |
| ScottPlot 막대 차트     | ScottPlot 막대 차트       |

> 스크린샷은 개발 완료 후 추가 예정입니다.  
> [Figma 와이어프레임](https://www.figma.com/design/apK5GQdChjRr2nldRsveic/BerkaAnalytics---Wireframe?node-id=0-1&t=RIcuQDwEJz1s34k1-1)에서 UI 구성을 미리 확인하실 수 있습니다.

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

5계층 구조로 관심사를 분리하여 각 레이어의 역할을 명확히 구분했습니다.

```
MySQL (BerkaETLPipeline에서 적재)
    ↓
Repository Layer   MySQL 쿼리 실행, 데이터 반환
    ↓
Service Layer      쿼리 조합, 비즈니스 로직
    ↓
Feature Layer      패널별 ViewModel, 데이터 바인딩
    ↓
Render Layer       ScottPlot / SkiaSharp 렌더링
    ↓
Entry Layer        진입점, DI 컨테이너, 앱 초기화
                   ↕
                WPF View (4분할 대시보드 출력)
```

`BerkaETLPipeline`에서 정제·적재된 MySQL 데이터를 Repository가 읽어,  
Service → Feature → Render 순으로 전달해 각 패널에 시각화합니다.

---

## 실행 방법

### 사전 요건

- .NET 10 SDK
- Visual Studio 2022
- MySQL 8.0
- [BerkaETLPipeline](https://github.com/Yumjiin/BerkaETLPipeline) 먼저 실행하여 DB 구축 필요

### 실행

```bash
# 1. 레포 클론
git clone https://github.com/Yumjiin/BerkaAnalyticsDashboard.git

# 2. Visual Studio에서 .sln 파일 열기

# 3. appsettings.json에 DB 연결 정보 입력 (아래 DB 연결 설정 참고)

# 4. F5 실행
```

---

## DB 연결 설정

`appsettings.json`에 MySQL 연결 정보를 입력하세요.

```json
{
  "ConnectionStrings": {
    "BerkaDb": "Server=localhost;Port=3306;Database=berka;Uid=berka_user;Pwd=your_password;"
  }
}
```

---

## 설계 문서

- [프로젝트 설계서](./docs/BerkaAnalytics_설계서.md)
- [기획서 + IA](./docs/BerkaAnalytics_기획서_IA.md)
- [Figma 와이어프레임](https://www.figma.com/design/apK5GQdChjRr2nldRsveic/BerkaAnalytics---Wireframe?node-id=0-1&t=RIcuQDwEJz1s34k1-1)
