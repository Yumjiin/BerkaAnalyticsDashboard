using YJI.Berka.Data.Entities;

namespace YJI.Berka.Data.Repositories;

public interface IAccountRepository
{
    // <summary>계좌 프로파일 전체 목록 조회 (사이드바 계좌 목록용)</summary>
    Task<IEnumerable<AccountProfile>> GetAllProfilesAsync();

    // <summary>특정 계좌 프로파일 조회</summary>
    Task<AccountProfile?> GetProfileAsync(int accountId);

    // <summary>특정 계좌의 일별 지출 조회</summary>
    Task<IEnumerable<DailySummary>> GetDailySummaryAsync(int accountId, DateTime start, DateTime end);

    // <summary>특정 계좌의 업종별 지출 조회</summary>
    Task<IEnumerable<CategorySummary>> GetCategorySummaryAsync(int accountId);
}