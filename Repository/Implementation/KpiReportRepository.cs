using Microsoft.EntityFrameworkCore;
using TravelEaseBackend.Models;
using TravelEaseBackend.Repository.Interface;

namespace TravelEaseBackend.Repository.Implementation
{
    public class KpiReportRepository : IKpiReportRepository
    {
        private readonly AppDbContext _context;
        public KpiReportRepository(AppDbContext context) => _context = context;

        public async Task<IEnumerable<KpiReport>> GetAllAsync() =>
            await _context.KpiReports.ToListAsync();

        public async Task<KpiReport?> GetByIdAsync(Guid reportId) =>
            await _context.KpiReports.FindAsync(reportId);

        public async Task AddAsync(KpiReport report)
        {
            await _context.KpiReports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid reportId)
        {
            var report = await _context.KpiReports.FindAsync(reportId);
            if (report != null)
            {
                _context.KpiReports.Remove(report);
                await _context.SaveChangesAsync();
            }
        }
    }
}
