using System.Linq;
using TravelEaseBackend.Dto;
using TravelEaseBackend.Models;
using TravelEaseBackend.Repository.Interface;
using TravelEaseBackend.Services.Interface;


namespace TravelEaseBackend.Services.Implementation
{
    public class KpiReportService : IKpiReportService
    {
        private readonly IKpiReportRepository _repository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly TravelEase.Repository.Interface.IBookingRepository _bookingRepository;

        public KpiReportService(IKpiReportRepository repository, IInvoiceRepository invoiceRepository, TravelEase.Repository.Interface.IBookingRepository bookingRepository)
        {
            _repository = repository;
            _invoiceRepository = invoiceRepository;
            _bookingRepository = bookingRepository;
        }

        public async Task<IEnumerable<KpiReportDto>> GetAllReportsAsync()
        {
            var reports = await _repository.GetAllAsync();
            return reports.Select(r => new KpiReportDto
            {
                ReportId = r.ReportId,
                Title = r.Title,
                GeneratedAt = r.GeneratedAt
            });
        }

        public async Task<KpiReportDto?> GetReportByIdAsync(Guid reportId)
        {
            var report = await _repository.GetByIdAsync(reportId);
            return report == null ? null : new KpiReportDto
            {
                ReportId = report.ReportId,
                Title = report.Title,
                GeneratedAt = report.GeneratedAt
            };
        }
        
        public async Task<decimal> GetTotalTravelSpendAsync()
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            return invoices.Sum(i => i.Amount);
        }

        public async Task<decimal> GetAverageSpendPerTravelerAsync()
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            if (!invoices.Any()) return 0;

            var totalSpend = invoices.Sum(i => i.Amount);
            var distinctTravelers = invoices.DistinctBy(i => i.BookingId).Count();

            return distinctTravelers > 0 ? totalSpend / distinctTravelers : 0;
        }

        public Task<int> GetBookingVolumeAsync()
        {
            var bookings = _bookingRepository.GetAllBookings();
            var count = bookings?.Count() ?? 0;
            return Task.FromResult(count);
        }

        public Task<decimal> GetCancellationRateAsync()
        {
            var bookings = _bookingRepository.GetAllBookings() ?? Enumerable.Empty<TravelEase.Model.BookingModel>();
            var total = bookings.Count();
            if (total == 0) return Task.FromResult(0m);

            var cancelled = bookings.Count(b => !string.IsNullOrEmpty(b.Status) &&
                                               (b.Status.Equals("cancelled", StringComparison.OrdinalIgnoreCase) ||
                                                b.Status.Equals("canceled", StringComparison.OrdinalIgnoreCase) ||
                                                b.Status.Equals("cancel", StringComparison.OrdinalIgnoreCase) ||
                                                b.Status.Equals(" Cancelled", StringComparison.OrdinalIgnoreCase)));

            var rate = total > 0 ? (decimal)cancelled * 100m / total : 0m;
            return Task.FromResult(Math.Round(rate, 2));
        }

        public async Task<KpiReportDto> CreateReportAsync(CreateKpiReportRequest request)
        {
            var report = new KpiReport
            {
                ReportId = Guid.NewGuid(),
                Title = request.Title,
                GeneratedAt = DateTime.UtcNow,
                FilePath = $"reports/{Guid.NewGuid()}.pdf"
            };

            await _repository.AddAsync(report);

            return new KpiReportDto
            {
                ReportId = report.ReportId,
                Title = report.Title,
                GeneratedAt = report.GeneratedAt
            };
        }

        public async Task DeleteReportAsync(Guid reportId) =>
            await _repository.DeleteAsync(reportId);

        public async Task<byte[]> DownloadReportAsync(Guid reportId)
        {
            var report = await _repository.GetByIdAsync(reportId);
            if (report == null || !File.Exists(report.FilePath))
                throw new FileNotFoundException("Report file not found.");

            return await File.ReadAllBytesAsync(report.FilePath);
        }
    }
}
