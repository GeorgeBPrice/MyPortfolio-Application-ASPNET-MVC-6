namespace PortfolioApp1.Models
{
    public class GoogleCaptchaResponse
    {
        // validation of reponse token (in controller for the contact form)
        public bool success { get; set; }
        
        // used to return a value, anything over 0.5 is a pass
        public double score { get; set; }
    }
}
