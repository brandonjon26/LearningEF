using System;
using System.Threading.Tasks;

namespace LearningEF.Api.Services
{
    public interface LogWriterInterface
    {
        Task LogErrorAsync(Exception ex, string message, string source);
        Task LogInformationAsync(string message, string source);
    }
}
