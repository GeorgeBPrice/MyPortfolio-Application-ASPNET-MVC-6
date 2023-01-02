using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;

namespace PortfolioApp1.Models
{
    // assigns google reCaptcha credientials found in appsettings JSON, as usable object values
    public class GoogleCaptchaConfig
    {
        public string SiteKey { get; set; }

        public string SecretKey { get; set; }

    }
}
