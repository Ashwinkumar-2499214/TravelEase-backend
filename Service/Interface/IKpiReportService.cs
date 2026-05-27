
using TravelEaseBackend.Dto;

namespace TravelEaseBackend.Services.Interface
{
    public interface IKpiReportService
    {
        // List all KPI reports
        Task<IEnumerable<KpiReportDto>> GetAllReportsAsync();

        // Get a single report by ID
        Task<KpiReportDto?> GetReportByIdAsync(Guid reportId);

        // Create a new KPI report (compiles metrics into JSON)
        Task<KpiReportDto> CreateReportAsync(CreateKpiReportRequest request);

        // Delete a report
        Task DeleteReportAsync(Guid reportId);

        // Download report file as PDF
        Task<byte[]> DownloadReportAsync(Guid reportId);

        // Returns the total travel spend across all travel-related invoices.
        Task<decimal> GetTotalTravelSpendAsync();

        // Returns the average spend per traveler (total spend / distinct travelers).
        Task<decimal> GetAverageSpendPerTravelerAsync();

        // Returns the total number of bookings (booking volume).
        Task<int> GetBookingVolumeAsync();

        // Returns the cancellation rate as a percentage (0-100).
        Task<decimal> GetCancellationRateAsync();
    }
}
