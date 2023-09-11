using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace LittleFish.Api.IdentityServer4
{
    public class Config
    {
        private IConfiguration _configuration;
        public Config(IConfiguration configuration) => _configuration = configuration;public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("product.read", "Read Access to Weather API"),
                new ApiScope("product.write", "Write Access to Weather API"),
            };
        public static IEnumerable<ApiResource> ApiResources =>
            new List<ApiResource>
             {
                  new ApiResource
                  {
                      Name = "product",
                      DisplayName = "Demo API",
                      Scopes = new List<string> { "product.read", "product.write"},
                      ApiSecrets = new List<Secret> {new Secret("1234567890123456789".Sha256())},
                      UserClaims = new List<string> {"admin"}
                  }
             };public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                 new Client
                 {
                     ClientId = "product",
                     ClientSecrets = { new Secret("1234567890123456789".Sha256()) },
                     AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                     // scopes that client has access to
                     AllowedScopes = { "product.read", "product.write"},
                    // RequirePkce =false
                 }
            };
    }
}