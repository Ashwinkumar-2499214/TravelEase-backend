using Microsoft.AspNetCore.Mvc;
using TravelEaseBackend.Dto;
using TravelEaseBackend.Services.Interface;

namespace TravelEaseBackend.Controllers
{
    [ApiController]
    [Route("api/v1/analytics")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IKpiReportService _kpiService;
        public AnalyticsController(IKpiReportService kpiService) => _kpiService = kpiService;

        // KPI Reports
        [HttpGet("kpi-reports")]
        public async Task<IActionResult> GetAllReports()
        {
            try
            {
                var reports = await _kpiService.GetAllReportsAsync();
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("kpi-reports")]
        public async Task<IActionResult> CreateReport([FromBody] CreateKpiReportRequest request)
        {
            try
            {
                var report = await _kpiService.CreateReportAsync(request);
                return CreatedAtAction(nameof(GetReportById), new { reportId = report.ReportId }, report);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("kpi-reports/{reportId}")]
        public async Task<IActionResult> GetReportById(Guid reportId)
        {
            try
            {
                var report = await _kpiService.GetReportByIdAsync(reportId);
                if (report == null) return NotFound();
                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpDelete("kpi-reports/{reportId}")]
        public async Task<IActionResult> DeleteReport(Guid reportId)
        {
            try
            {
                await _kpiService.DeleteReportAsync(reportId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("kpi-reports/{reportId}/download")]
        public async Task<IActionResult> DownloadReport(Guid reportId)
        {
            try
            {
                var fileBytes = await _kpiService.DownloadReportAsync(reportId);
                return File(fileBytes, "application/pdf", "KpiReport.pdf");
            }
            catch (Exception ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        // Dashboards
        [HttpGet("dashboards/travel-spend")]
        public async Task<IActionResult> TravelSpendDashboard()
        {
            try
            {
                var totalTravelSpend = await _kpiService.GetTotalTravelSpendAsync();
                return Ok(new
                {
                    metric = "Total Travel Spend",
                    value = totalTravelSpend
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("dashboards/booking-volume")]
        public async Task<IActionResult> BookingVolumeDashboard()
        {
            try
            {
                var volume = await _kpiService.GetBookingVolumeAsync();
                return Ok(new
                {
                    metric = "Booking Volume",
                    value = volume
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("dashboards/cancellations")]
        public async Task<IActionResult> CancellationDashboard()
        {
            try
            {
                var rate = await _kpiService.GetCancellationRateAsync();
                return Ok(new
                {
                    metric = "Cancellation Rate",
                    value = rate
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // Trends
        [HttpGet("trends/spend-per-traveler")]
        public async Task<IActionResult> SpendPerTravelerTrend()
        {
            try
            {
                var avgSpend = await _kpiService.GetAverageSpendPerTravelerAsync();
                return Ok(new
                {
                    trend = "Average spend per traveler",
                    value = avgSpend
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpGet("trends/destinations")]
        public IActionResult DestinationTrends() =>
            Ok(new { trend = "Top destinations", values = new[] { "Paris", "Tokyo", "New York" } });
    }
}
