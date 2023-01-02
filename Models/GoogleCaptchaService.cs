using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace PortfolioApp1.Models
{
    public class GoogleCaptchaService
    {
        // first declare and inject the GoogleCaptchaConfig to access it's paremeters
        private readonly IOptionsMonitor<GoogleCaptchaConfig> _config;

        public GoogleCaptchaService(IOptionsMonitor<GoogleCaptchaConfig> config)
        {
            _config = config;
        }

        // Using an API Request, verify the users reponse token
        public async Task<bool> VerifyCaptchaToken(string token)
        {
			try
			{
                // define request url string
                var url = $"https://www.google.com/recaptcha/api/siteverify?secret={_config.CurrentValue.SecretKey}&response={token}";

                // check http status to verify if response is OK
                using (var client = new HttpClient()) { 
                    
                    var httpResult = await client.GetAsync(url); // call the url request

                    // return false if status is not OK
                    if (httpResult.StatusCode != HttpStatusCode.OK)
                    {
                        return false;
                    }

                    // if status is OK, grab the API response string (JSON format)
                    var responseString = await httpResult.Content.ReadAsStringAsync();

                    // deserialize the JSON reponse into local Object, to access properties
                    var captchaResult = JsonConvert.DeserializeObject<GoogleCaptchaResponse>(responseString);

                    // finally check score, and confirm 2 conditions, required to validate recaptcha response
                    return captchaResult.success && captchaResult.score >= 0.5;
                }
            
            }
			catch (Exception)
			{
                return false;
			}
        }
    }
}
