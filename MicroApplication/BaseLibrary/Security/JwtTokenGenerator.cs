using Microsoft.IdentityModel.Tokens;

namespace BaseLibrary.Security
{
    public class JwtTokenGenerator
    {
        public static JwtToken GenerateJwtToken(Guid userId, string language, string sessionId, string secureKey, string tokenIssuer, string tokenAudience)
        {
            SymmetricSecurityKey SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secureKey));
            var SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
            var TokenExpiration = TimeSpan.FromMinutes(10000);
            DateTime now = DateTime.UtcNow;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, tokenIssuer),
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUnixTimeSeconds().ToString(),ClaimValueTypes.Integer64),
                new Claim("language", language),
                new Claim("session",sessionId)
            };

            // Create the JWT and write it to a string
            var token = new JwtSecurityToken(
                issuer: tokenIssuer,
                audience: tokenAudience,
                claims: claims,
                expires: now.Add(TokenExpiration),
                signingCredentials: SigningCredentials);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            // build the json response
            var jwt = new JwtToken(encodedToken, now.Add(TokenExpiration), new List<string>());

            return jwt;
        }
    }

    public class JwtToken
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public Guid? DefaultStudyId { get; set; }
        public DateTime Expire { get; set; }
        public List<string> Permissions { get; set; }
        public JwtToken(string token, DateTime expire, List<string> permissions)
        {
            Token = token;
            Expire = expire;
            Permissions = permissions;
        }
    }
}
