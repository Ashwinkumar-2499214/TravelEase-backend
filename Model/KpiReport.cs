using System.ComponentModel.DataAnnotations;

namespace TravelEaseBackend.Models
{
    public class KpiReport
    {
        [Key]
        public Guid ReportId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string Metrics { get; internal set; }
        public string Scope { get; internal set; }
    }
}
