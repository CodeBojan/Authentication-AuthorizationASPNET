namespace Authentication_Authorization.Models
{
    public class IdentityServerSettings
    {
        public string DiscoveryUrl { get; set; }
        public string ClientName { get; set; }
        public string ClientSecret { get; set; }
        public bool UseHttps { get; set; }
    }
}
