using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AuthServer.Data;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer.Services
{
    public class TokenService
    {
        private readonly TokenSettings config;

        public TokenService(Config config)
        {
            this.config = config.Token;
        }

        public object Generate(AuthUserInfo user)
        {
            var descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, user.FullId),
                    new(ClaimTypes.Name, user.DisplayName),
                    new("raw:provider", user.Provider),
                    new("raw:userId", user.Id)
                }),
                Expires = DateTime.UtcNow.Add(config.TTL),
                SigningCredentials = SigninManager.GetSigningCredentials(config)
            };
            if (config.Issuer != null) descriptor.Issuer = config.Issuer;
            if (config.Audience != null) descriptor.Audience = config.Audience;
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(descriptor);
            var result = handler.WriteToken(token);
            return result;
        }

        private static class SigninManager
        {
            public static SigningCredentials GetSigningCredentials(TokenSettings config)
            {
                var algo = config.Algorithm.ToString();

                if (algo.StartsWith("HS")) return HmacSha(config.Algorithm, config.Secret);
                if (algo.StartsWith("RS")) return HmacRsa(config.Algorithm, config.SecretFile);
                throw new NotImplementedException($"Unknown algorithm: ${algo}");
            }

            private static SigningCredentials HmacSha(TokenSettings.AlgType algoEnum, string? configSecret)
            {
                if (configSecret == null) throw new ArgumentNullException(nameof(configSecret));
                string alg = algoEnum switch
                {
                    TokenSettings.AlgType.HS256 => SecurityAlgorithms.HmacSha256,
                    TokenSettings.AlgType.HS384 => SecurityAlgorithms.HmacSha384,
                    TokenSettings.AlgType.HS512 => SecurityAlgorithms.HmacSha512,
                    _ => throw new ArgumentException($"Unknown algorithm: ${algoEnum}")
                };
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configSecret));
                return new SigningCredentials(securityKey, alg);
            }


            private static SigningCredentials HmacRsa(TokenSettings.AlgType algoEnum, string? secretPath)
            {
                if (secretPath == null) throw new ArgumentNullException(nameof(secretPath));
                if (!File.Exists(secretPath)) throw new FieldAccessException($"Secret file not found: {secretPath}");
                string alg = algoEnum switch
                {
                    TokenSettings.AlgType.RS256 => SecurityAlgorithms.RsaSha256,
                    TokenSettings.AlgType.RS384 => SecurityAlgorithms.RsaSha384,
                    TokenSettings.AlgType.RS512 => SecurityAlgorithms.RsaSha512,
                    _ => throw new ArgumentException($"Unknown algorithm: ${algoEnum}")
                };
                var rsa = RSA.Create();
                var keyBytes = File.ReadAllText(secretPath);
                rsa.ImportFromPem(keyBytes.ToCharArray());
                var securityKey = new RsaSecurityKey(rsa);
                return new SigningCredentials(securityKey, alg);
            }
        }

    }
}
