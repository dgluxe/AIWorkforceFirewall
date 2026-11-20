using AIWorkforceFirewall.DataLeakService.Models;

namespace AIWorkforceFirewall.DataLeakService.IServices;

public interface IDataLeakAnalyzer
{
    AnalyzeResponse Analyze(string text);
}
