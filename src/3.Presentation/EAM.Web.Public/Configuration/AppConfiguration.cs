namespace EAM.Web.Public.Configuration;

public class ApiSettings
{
    public string BaseUrl { get; set; } = "http://localhost:5000";
}

public class AppSettings
{
    public string PortalUrl { get; set; } = "http://localhost:5002";
    public string CompanyName { get; set; } = "EAM Company";
    public string AuthorName { get; set; } = "Emanuel Macêdo";
    public SocialMediaSettings SocialMedia { get; set; } = new();
}

public class SocialMediaSettings
{
    public string LinkedIn { get; set; } = "https://www.linkedin.com/in/emanuel-a-macedo/";
    public string GitHub { get; set; } = "https://github.com/ContatoEmanuel";
    public string YouTube { get; set; } = "https://www.youtube.com/channel/UCQbZlecPawGlx0F6Oo56LDw";
    public string Linktree { get; set; } = "https://linktr.ee/emanuel.macedo";
    public string Credly { get; set; } = "https://www.credly.com/users/emanuel-a-macedo/badges";
    public string MicrosoftLearn { get; set; } = "https://learn.microsoft.com/pt-br/users/emanuelarrudasmacedo-1105/";
    public string WhatsApp { get; set; } = "https://wa.me/5538988601152";
}
