namespace SimpleAPSViewer
{
    public class ApsSettings
    {
        public string TokenServerUrl { get; set; } = "https://developer.api.autodesk.com/authentication/v2/token";
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
