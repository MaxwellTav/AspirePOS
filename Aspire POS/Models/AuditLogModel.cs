namespace Aspire_POS.Models
{
    public class AuditLogModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public string TableName { get; set; }
        public int? RecordId { get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
