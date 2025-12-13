namespace EAM.Web.Portal.Configuration;

public class ApiSettings
{
    public string BaseUrl { get; set; } = "http://localhost:5000";
}

public class AppSettings
{
    public string PublicWebsiteUrl { get; set; } = "http://localhost:5001";
    public string CompanyName { get; set; } = "EAM Company";
}
