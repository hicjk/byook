using byook.Utility.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace byook.Utility.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthenticationWithProtectedCookie(this IServiceCollection services, TokenOptions tokenOptions, string? applicationDiscriminator = null, AuthUrlOptions? authUrlOptions = null)
        {
            var hostingEnvironment = services.BuildServiceProvider()
                .GetService<IHostEnvironment>();

            var applicationName = $"{applicationDiscriminator ?? hostingEnvironment!.ApplicationName}";

            services.AddDataProtection(options => options.ApplicationDiscriminator = applicationName).SetApplicationName(applicationName);

            services.AddScoped<IDataSerializer<AuthenticationTicket>, TicketSerializer>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>(serviceProvider => new JwtTokenGenerator(tokenOptions));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(tokenOptions.TokenExpiryInMinutes);
                options.TicketDataFormat = new JwtAuthTicketFormat(tokenOptions.ToTokenValidationParams(), services.BuildServiceProvider().GetService<IDataSerializer<AuthenticationTicket>>()!,
                    services.BuildServiceProvider()
                    .GetDataProtector(new[]
                        {
                            $"{applicationName}-Auth1"
                        }));
                
                options.AccessDeniedPath = "/Home/Index";
                //options.ReturnUrlParameter = authUrlOptions?.ReturnUrlParameter ?? "returnUrl";
            });

            return services;
        }
    }

    public sealed class AuthUrlOptions
    {
        public string LoginPath { get; set; } = string.Empty;

        public string LogoutPath { get; set; } = string.Empty;

        public string ReturnUrlParameter { get; set; } = string.Empty;
    }
}
