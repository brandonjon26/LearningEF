using LearningEF.Api.Data;
using LearningEF.Api.Models;
using System;
using System.Threading.Tasks;

namespace LearningEF.Api.Services
{
    public class LogWriterService : LogWriterInterface
    {
        private readonly AppLogContext _context;

        public LogWriterService(AppLogContext context)
        {
            _context = context;
        }

        private async Task WriteLogAsync(string level, string message, Exception? ex)
        {
            var logEntry = new AppLog
            {
                TimeStamp = DateTimeOffset.UtcNow,
                SeverityLevel = level,
                Message = message,
                Exception = ex?.ToString(), // Store the full stack trace
                // You can add structured properties here later if needed (e.g., as JSON string)
                Properties = null
            };

            _context.AppLogs.Add(logEntry);
            await _context.SaveChangesAsync();
        }

        public Task LogErrorAsync(Exception ex, string message, string source)
        {
            // Combine source into the message for context
            var fullMessage = $"[{source}] {message}";
            return WriteLogAsync("Error", fullMessage, ex);
        }

        public Task LogInformationAsync(string message, string source)
        {
            var fullMessage = $"[{source}] {message}";
            return WriteLogAsync("Information", fullMessage, null);
        }
    }
}
