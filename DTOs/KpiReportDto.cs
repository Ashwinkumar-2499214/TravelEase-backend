namespace TravelEaseBackend.Dto
{
    public class KpiReportDto
    {
        public Guid ReportId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
    }
}