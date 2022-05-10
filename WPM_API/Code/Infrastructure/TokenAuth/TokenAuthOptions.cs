using Microsoft.IdentityModel.Tokens;

namespace WPM_API.Code.Infrastructure.TokenAuth
{
    public class TokenAuthOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public SigningCredentials SigningCredentials { get; set; }
        public RsaSecurityKey IssuerSigningKey { get; set; }
    }
}
