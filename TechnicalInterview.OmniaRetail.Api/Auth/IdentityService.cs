using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TechincalInterview.OmniaRetail.Contracts.Requests;

namespace TechnicalInterview.OmniaRetail.Api.Auth
{
    public class IdentityService : IIdentityService
    {
        private static readonly TimeSpan TokenLifetime = TimeSpan.FromHours(8);
        private const string TokenSecret = "InProductionThisShouldBeInASecureLocation";
        private const string Issuer = "https://omnia-retail-interview.technical.com/timosvalais";
        private const string Audience = "https://www.omnia-retail-clients.com";
        private static readonly List<Guid> RetailerIds =
        [
            new Guid("e3a87e20-36d3-42ee-8993-c8dfbfa01c3b"),
            new Guid("bc024a7e-c4f6-42a5-a67d-76b8c9efd432"),
            new Guid("6e94c46e-a0b1-4831-bea9-fa48900c3065"),
            new Guid("3a66173d-ff1f-4458-9bd9-2fc27887ad35"),
        ];

        public Task<string> GenerateToken(TokenGenerationRequest tokenRequest)
        {


            JwtSecurityTokenHandler tokenHandler = new();
            byte[] key = Encoding.UTF8.GetBytes(TokenSecret);


            List<Claim> claims =
            [
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            ];
            if (RetailerIds.Contains(tokenRequest.RetailerId))
            {
                claims.Add(new("retailerId", tokenRequest.RetailerId.ToString()));
            }
            if (tokenRequest.CustomClaims is not null)
            {
                foreach (KeyValuePair<string, object> claimPair in tokenRequest.CustomClaims)
                {
                    JsonElement jsonElement = (JsonElement)claimPair.Value;
                    string valueType = jsonElement.ValueKind switch
                    {
                        JsonValueKind.True => ClaimValueTypes.Boolean,
                        JsonValueKind.False => ClaimValueTypes.Boolean,
                        JsonValueKind.Number => ClaimValueTypes.Double,
                        _ => ClaimValueTypes.String
                    };

                    Claim claim = new(claimPair.Key, claimPair.Value.ToString()!, valueType);
                    claims.Add(claim);
                }
            }


            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(TokenLifetime),
                Issuer = Issuer,
                Audience = Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            string jwt = tokenHandler.WriteToken(token);
            return Task.FromResult(jwt);
        }
    }
}
