using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Framework.Logging;
using Newtonsoft.Json.Linq;

namespace vNextApplication.Services
{
    public class CoordinateService
    {
        private ILogger<CoordinateService> _logger;

        public CoordinateService(ILogger<CoordinateService> logger)
        {
            _logger = logger;
        }

        public async Task<CoordinateServiceResult> Lookup(string location)
        {
            var result = new CoordinateServiceResult
            {
                Success = false,
                Message = "Undetermined failure while looking up coordinates"
            };

            var bingKey = Startup.Configuration["AppSettings:BingKey"];

            var encodedName = WebUtility.UrlEncode(location);

            var url = $"http://dev.virtualearth.net/REST/v1/Locations?q={encodedName}&key={bingKey}";

            var client = new HttpClient();

            var json = await client.GetStringAsync(url);

            var results = JObject.Parse(json);

            var resources = results["resourceSets"][0]["resources"];
            if (!resources.HasValues)
            {
                result.Message = $"Could not find {location} as location";
            }

            else
            {
                var confidence = (string) resources[0]["confidence"];
                if (confidence != "High")
                {
                    result.Message = $"Could not find a confident match for {location} for the location provided";
                }
                else
                {
                    var coordinates = resources[0]["geocodePoints"][0]["coordinates"];
                    result.Latitude = (double) coordinates[0];
                    result.Longitude = (double) coordinates[1];
                    result.Success = true;
                    result.Message = "Massive Success";
                }
            }
            return result;
        }
    }
}