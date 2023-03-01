namespace Core.Server.ChuBao.Models
{
    public class JwtOptions
    {
        public const string Name = "JwtOptions";
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string Secret { get; set; }
        public string ExpiresByHours { get; set; }
    }
}
