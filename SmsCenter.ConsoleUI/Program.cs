using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SmsCenter.Api;
using SmsCenter.Api.Internal.Exceptions;
using SmsCenter.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.AddSmscService();


var app = builder.Build();

var service = app.Services.GetRequiredService<ISmsCenterService>();
try
{
    var result = await service.SendSms("+79265718860", "Привет");
    Console.WriteLine(result);
    Console.ForegroundColor = ConsoleColor.Magenta;
    Console.WriteLine("OK");
}
catch (SmsCenterException ex) {
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(ex.Message);
}
catch (Exception ex) {
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(ex.Message);
}