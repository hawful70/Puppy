﻿#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Author> Top </Author>
//     <Project> TopCore.Auth.Service </Project>
//     <File>
//         <Name> SeedAuthService </Name>
//         <Created> 06 Apr 17 2:28:35 AM </Created>
//         <Key> 6d71a61d-7d35-4fc3-ba5f-fea3036f6250 </Key>
//     </File>
//     <Summary>
//         SeekAuthService
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using IdentityModel;
using IdentityServer4;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Threading.Tasks;
using TopCore.Auth.Domain.Entities;
using TopCore.Auth.Domain.Interfaces.Data;
using TopCore.Auth.Domain.Interfaces.Services;
using TopCore.Framework.DependencyInjection.Attributes;

namespace TopCore.Auth.Service
{
    [PerRequestDependency(ServiceType = typeof(ISeedAuthService))]
    public class SeedAuthService : ISeedAuthService
    {
        private readonly IDbContext _dbContext;
        private readonly UserManager<User> _userManager;

        public SeedAuthService(
            IDbContext dbContext,
            UserManager<User> userManager
            )
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public Task SeedAuthDatabaseAsync()
        {
            // Run migrate first
            var migrate = _dbContext.MigrateDatabaseAsync();
            migrate.Wait();
            SeedScope_APIResource();
            SeedClientAuth();
            SeedClientMobileAndroid();
            SeedClientMobileiOs();
            SeedScope_IdentityResource();
            SeedUserTopNguyen();
            SeedUserHungNguyen();
            SeedUserDungNguyen();
            return migrate;
        }

        private void SeedUserTopNguyen()
        {
            var user = new User
            {
                UserName = "topnguyen",
                NormalizedUserName = "topnguyen",
                Email = "topnguyen92@gmail.com",
                NormalizedEmail = "topnguyen92@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0945188299",
                PhoneNumberConfirmed = true,
                Claims =
                {
                    new IdentityUserClaim<string>
                    {
                        ClaimType =  JwtClaimTypes.Name,
                        ClaimValue = "Top Nguyen",
                    },
                    new IdentityUserClaim<string>
                    {
                        ClaimType = JwtClaimTypes.BirthDate,
                        ClaimValue = "20/11/1992"
                    }
                }
            };

            var isExist = _dbContext.Any<User>(x => x.UserName == user.UserName);

            if (!isExist)
            {
                _userManager.CreateAsync(user, "123456").Wait();
            }
        }

        private void SeedUserHungNguyen()
        {
            var user = new User
            {
                UserName = "hungnguyen",
                NormalizedUserName = "hungnguyen",
                Email = "hungnguyen@gmail.com",
                NormalizedEmail = "hungnguyen@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "123456",
                PhoneNumberConfirmed = true,
                Claims =
                {
                    new IdentityUserClaim<string>
                    {
                        ClaimType =  JwtClaimTypes.Name,
                        ClaimValue = "Hung Nguyen",
                    },
                    new IdentityUserClaim<string>
                    {
                        ClaimType = JwtClaimTypes.BirthDate,
                        ClaimValue = "20/11/1991"
                    }
                }
            };

            var isExist = _dbContext.Any<User>(x => x.UserName == user.UserName);

            if (!isExist)
            {
                _userManager.CreateAsync(user, "123456").Wait();
            }
        }

        private void SeedUserDungNguyen()
        {
            var user = new User
            {
                UserName = "dungnguyen",
                NormalizedUserName = "dungnguyen",
                Email = "dungnguyen@gmail.com",
                NormalizedEmail = "dungnguyen@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "123456",
                PhoneNumberConfirmed = true,
                Claims =
                {
                    new IdentityUserClaim<string>
                    {
                        ClaimType =  JwtClaimTypes.Name,
                        ClaimValue = "Dung Nguyen",
                    },
                    new IdentityUserClaim<string>
                    {
                        ClaimType = JwtClaimTypes.BirthDate,
                        ClaimValue = "20/11/1991"
                    }
                }
            };

            var isExist = _dbContext.Any<User>(x => x.UserName == user.UserName);

            if (!isExist)
            {
                _userManager.CreateAsync(user, "123456").Wait();
            }
        }

        private void SeedClientAuth()
        {
            var webClient = new Client
            {
                Enabled = true,
                AccessTokenType = AccessTokenType.Jwt,
                ClientId = "topcore_auth",
                ClientName = "Top Core Auth",
                ClientSecrets = { new Secret("topcoreauth".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = true,
                AllowOfflineAccess = true,
                AllowRememberConsent = true,
                EnableLocalLogin = true,
                UpdateAccessTokenClaimsOnRefresh = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                RequireConsent = false,
                AllowedCorsOrigins = { "http://eatup.vn" },
                AllowedScopes =
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.OfflineAccess
                }
            }.ToEntity();

            var isExist = _dbContext.Any<IdentityServer4.EntityFramework.Entities.Client>(x => x.ClientId == webClient.ClientId);

            if (!isExist)
            {
                _dbContext.Add(webClient);
            }
        }

        private void SeedClientMobileAndroid()
        {
            var mobileAndroidClient = new Client
            {
                Enabled = true,
                AccessTokenType = AccessTokenType.Jwt,
                ClientId = "topcore_mobile_android",
                ClientName = "topcore mobile android",
                ClientSecrets = { new Secret("topcoremobileandroid".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = true,
                AllowOfflineAccess = true,
                AllowRememberConsent = true,
                EnableLocalLogin = true,
                UpdateAccessTokenClaimsOnRefresh = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                RequireConsent = false,
                AllowedCorsOrigins = { "http://eatup.vn" },
                AllowedScopes =
                {
                    "topcore_api",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.OfflineAccess
                }
            }.ToEntity();

            var isExist = _dbContext.Any<IdentityServer4.EntityFramework.Entities.Client>(x => x.ClientId == mobileAndroidClient.ClientId);

            if (!isExist)
            {
                _dbContext.Add(mobileAndroidClient);
            }
        }

        private void SeedClientMobileiOs()
        {
            var mobileiOsClient = new Client
            {
                Enabled = true,
                AccessTokenType = AccessTokenType.Jwt,
                ClientId = "topcore_mobile_iOS",
                ClientName = "topcore mobile iOS",
                ClientSecrets = { new Secret("topcoremobileiOS".Sha256()) },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = true,
                AllowOfflineAccess = true,
                AllowRememberConsent = true,
                EnableLocalLogin = true,
                UpdateAccessTokenClaimsOnRefresh = true,
                RefreshTokenUsage = TokenUsage.ReUse,
                RequireConsent = false,
                AllowedCorsOrigins = { "http://eatup.vn" },
                AllowedScopes =
                {
                    "topcore_api",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.OfflineAccess
                }
            }.ToEntity();

            var isExist = _dbContext.Any<IdentityServer4.EntityFramework.Entities.Client>(x => x.ClientId == mobileiOsClient.ClientId);

            if (!isExist)
            {
                _dbContext.Add(mobileiOsClient);
            }
        }

        private void SeedScope_APIResource()
        {
            var apiResource = new ApiResource("topcore_api", "Top Core API").ToEntity();

            var isExist = _dbContext.Any<IdentityServer4.EntityFramework.Entities.ApiResource>(x => x.Name == apiResource.Name);

            if (!isExist)
            {
                _dbContext.Add(apiResource);
            }
        }

        private void SeedScope_IdentityResource()
        {
            // open id resource is always required
            var openIdResource = new IdentityResource
            {
                Name = IdentityServerConstants.StandardScopes.OpenId,
                Description = $"{IdentityServerConstants.StandardScopes.OpenId} is always required for openid protocol",
                DisplayName = "Open Identity",
                Emphasize = true,
                Enabled = true,
                Required = true,
                ShowInDiscoveryDocument = true,
                UserClaims =
                {
                    JwtClaimTypes.Subject,
                    JwtClaimTypes.PreferredUserName,
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified,
                    JwtClaimTypes.PhoneNumber,
                    JwtClaimTypes.PhoneNumberVerified,
                    JwtClaimTypes.Name,
                    JwtClaimTypes.BirthDate
                }
            }.ToEntity();

            var isExistOpenIdResource = _dbContext.Any<IdentityServer4.EntityFramework.Entities.IdentityResource>(x => x.Name == openIdResource.Name);

            if (!isExistOpenIdResource)
            {
                _dbContext.Add(openIdResource);
            }
        }
    }
}