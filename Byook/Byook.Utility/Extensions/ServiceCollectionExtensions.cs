using Byook.Utility.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Byook.Utility.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtAuthenticationWithProtectedCookie(this IServiceCollection services, TokenOptions tokenOptions, string? applicationDiscriminator = null)
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
                options.LogoutPath = "/Member/Logout";
                options.LoginPath = "/Member/ConsumerLogin";
                options.AccessDeniedPath = "/Home/Index";
                options.ReturnUrlParameter = "/Home/Index";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(tokenOptions.TokenExpiryInMinutes);
                options.TicketDataFormat = new JwtAuthTicketFormat(tokenOptions.ToTokenValidationParams(), services.BuildServiceProvider().GetService<IDataSerializer<AuthenticationTicket>>()!,
                    services.BuildServiceProvider()
                    .GetDataProtector(new[]
                        {
                            $"{applicationName}-Auth1"
                        }));

            });

            return services;
        }
    }
}