using YJI.Berka.Data.Entities;


namespace YJI.Berka.Data.Repositories;

public interface IAnomalyRepository
{
    // <summary>특정 계좌의 이상 탐지 결과 조회 (method별)</summary>
    Task<IEnumerable<AnomalyFlag>> GetByAccountAsync(int accountId, string? method = null);

    // <summary>고신뢰 이상 거래 조회 (2개 이상 모델 공통 탐지)</summary>
    Task<IEnumerable<HighConfidenceAnomaly>> GetHighConfidenceAsync(int accountId);
}
