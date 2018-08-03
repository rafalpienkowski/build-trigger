using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BuildTrigger
{
    public interface IBuildTrigger
    {
        Task<HttpResponseMessage> TriggerAsync(int buildNumber);
        Task<List<Build>> GetBuildsAsync();
    }
}
