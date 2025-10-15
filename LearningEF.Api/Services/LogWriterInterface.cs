using System;
using System.Threading.Tasks;
using LearningEF.Api.Models;

namespace LearningEF.Api.Services
{
    public interface LogWriterInterface
    {
        Task LogErrorAsync(Exception ex, string message, string source);
        Task LogInformationAsync(string message, string source);
        Task WriteLogAsync(Models.LogLevel level, string message, Exception? ex = null, object? properties  = null);
    }
}
