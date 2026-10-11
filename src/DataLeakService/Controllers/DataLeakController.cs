using AIWorkforceFirewall.DataLeakService.Models;
using AIWorkforceFirewall.DataLeakService.Services;
using Microsoft.AspNetCore.Mvc;

namespace AIWorkforceFirewall.DataLeakService.Controllers;

[ApiController]
[Route("api/dataleak")]
public sealed class DataLeakController : ControllerBase
{
    private readonly IDataLeakAnalyzer _analyzer;

    public DataLeakController(IDataLeakAnalyzer analyzer) 
    {
        _analyzer = analyzer;

    }

    [HttpPost("analyze")]
    public ActionResult<AnalyzeResponse> Analyze([FromBody] AnalyzeRequest request)
    {
        var result = _analyzer.Analyze(request.Text);
        return Ok(result);
    }
}
