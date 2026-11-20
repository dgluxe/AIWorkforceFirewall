using AIWorkforceFirewall.DataLeakService.Models.Policies;

namespace AIWorkforceFirewall.DataLeakService.IServices.Policies;

public interface IPolicyEngine
{
    PolicyDecision Evaluate(
        IReadOnlyCollection<string> detectedTypes);
}
