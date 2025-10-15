using LearningEF.Api.Data;
using LearningEF.Api.Models;
using System;
using System.Text.Json;
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

        private async Task WriteLogCoreAsync(Models.LogLevel level, string message, Exception? ex, object? properties)
        {
            string propertiesJson = string.Empty;
            if (properties != null)
            {
                try
                {
                    // Use JsonSerializer to convert the object
                    propertiesJson = JsonSerializer.Serialize(properties, new JsonSerializerOptions { WriteIndented = false });
                }
                catch (Exception serializeEx)
                {
                    // If serialization fails, log an error about the log attempt itself!
                    propertiesJson = $"LOGGING ERROR: Could not serialize properties object. {serializeEx.Message}";
                }
            }

            var logEntry = new AppLog
            {
                TimeStamp = DateTimeOffset.UtcNow,
                SeverityLevel = level.ToString(),
                Message = message,
                Exception = (ex?.ToString() == null ? "" : ex.ToString()), // Store the full stack trace
                Properties = propertiesJson,
            };

            _context.AppLogs.Add(logEntry);
            await _context.SaveChangesAsync();
        }

        public Task LogErrorAsync(Exception ex, string message, string source)
        {
            // Combine source into the message for context
            var fullMessage = $"[{source}] {message}";
            return WriteLogCoreAsync(Models.LogLevel.Error, fullMessage, ex, null);
        }

        public Task LogInformationAsync(string message, string source)
        {
            var fullMessage = $"[{source}] {message}";
            return WriteLogCoreAsync(Models.LogLevel.Information, fullMessage, null, null);
        }

        public Task WriteLogAsync(Models.LogLevel level, string message, Exception? ex = null, object? properties = null)
        {
            return WriteLogCoreAsync(level, message, ex, properties);
        }
    }
}
