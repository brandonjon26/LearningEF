using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningEF.Api.Models
{
    [Table("AppLog")]
    public class AppLog
    {
        [Key]
        public long AppLogId { get; set; }
        public DateTimeOffset TimeStamp { get; set; } = new DateTimeOffset(new DateTime(1900, 01, 01));
        [MaxLength(12)]
        public string SeverityLevel { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; } = string.Empty;
        public string? Properties {  get; set; } = string.Empty;
    }
}
