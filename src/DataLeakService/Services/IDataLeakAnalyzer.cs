using AIWorkforceFirewall.DataLeakService.Models;

namespace AIWorkforceFirewall.DataLeakService.Services;

public interface IDataLeakAnalyzer
{
    AnalyzeResponse Analyze(string text);
}
