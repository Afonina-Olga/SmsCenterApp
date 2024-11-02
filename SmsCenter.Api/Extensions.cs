using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmsCenter.Api.Options;
using SmsCenter.Api.Providers;
using SmsCenter.Api.Services;

namespace SmsCenter.Api;

public static class SmsCenterHostBuilderExtensions {
    public static void AddSmscService(this WebApplicationBuilder builder) {
        var services = builder.Services;
        var configuration = builder.Configuration;
        services.Configure<SmsCenterOptions>(configuration.GetSection("SmsCenter"),
            options => options.BindNonPublicProperties = true);
        services.AddSingleton<ISmsCenterProvider, SmsCenterProvider>();
        services.AddSingleton<ISmsCenterService, SmsCenterService>();
        services.AddHttpClient(SmsCenterProvider.HttpClientName,
            config => { config.BaseAddress = new("https://smsc.ru"); });
    }
    
    public static void AddSmscService(this IServiceCollection services, IConfiguration configuration) {
        services.Configure<SmsCenterOptions>(configuration.GetSection("SmsCenter"),
            options => options.BindNonPublicProperties = true);
        services.AddSingleton<ISmsCenterProvider, SmsCenterProvider>();
        services.AddSingleton<ISmsCenterService, SmsCenterService>();
        services.AddHttpClient(SmsCenterProvider.HttpClientName,
            config => { config.BaseAddress = new("https://smsc.ru"); });
    }
}
