using TravelEaseBackend.Models;

namespace TravelEaseBackend.Repository.Interface
{
    public interface IKpiReportRepository
    {
        Task<IEnumerable<KpiReport>> GetAllAsync();
        Task<KpiReport?> GetByIdAsync(Guid reportId);
        Task AddAsync(KpiReport report);
        Task DeleteAsync(Guid reportId);
    }
}