using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SmsCenter.Api;
using SmsCenter.Api.Services;

var builder = WebApplication.CreateBuilder(args);
builder.AddSmscService();

var app = builder.Build();

var service = app.Services.GetRequiredService<ISmsCenterService>();

const byte lineWidth = 100;
const string phone = "+79265718860";
const string unrealPhone = "+79261234567";
var phones = new[] { "+79265718860", "+79261234657", "+79251547412" };
const string message = "Привет!";
const string longMessage =
    "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged.";

PrintHeader("Получить стоимость рассылки");

#region Стоимость рассылки

try
{
    var result = await service.GetCost(phones, message);
    PrintSubHeader("Стоимость отправки одного и того же сообщения на несколько номеров телефонов");
    PrintRequestFromArray(phones, message);
    Console.WriteLine();
    PrintResponse(result);
}
catch (Exception e)
{
    PrintErrorMessage(e.Message);
}

try
{
    var result = await service.GetCost(phone, message);
    PrintSubHeader($"Стоимость отправки сообщения \"{message}\" на номер {phone}");
    PrintRequest(phone, message);
    Console.WriteLine();
    PrintResponse(result);
}
catch (Exception e)
{
    PrintErrorMessage(e.Message);
}

try
{
    var result = await service.GetCost(phone, longMessage);
    PrintSubHeader($"Стоимость отправки длинного сообщения на номер {phone}");
    PrintRequest(phone, longMessage);
    Console.WriteLine();
    PrintResponse(result);
}
catch (Exception e)
{
    PrintErrorMessage(e.Message);
}

try
{
    var list = new Dictionary<string, string>()
    {
        { "+79265718860", longMessage },
        { "+79251547412", message },
        { "+79271234567", message },
        { "+79262638751", message }
    };
    var result = await service.GetCost(list);
    PrintSubHeader($"Стоимость отправки различных сообщений на различные номера");
    PrintRequestFromDictionary(list);
    Console.WriteLine();
    PrintResponse(result);
}

catch (Exception e)
{
    PrintErrorMessage(e.Message);
}

#endregion

#region Баланс

try
{
    var result = await service.GetBalance();
    PrintSubHeader("Получение баланса");
    Console.WriteLine($"Остаток: {result} руб.");
}
catch (Exception e)
{
    PrintErrorMessage(e.Message);
}

#endregion

#region Отправка смс

try
{
    var result = await service.SendSms(phones, message);
    PrintSubHeader("Отправка одного и того же сообщения на несколько номеров телефонов");
    PrintRequestFromArray(phones, message);
    Console.WriteLine();
    PrintResponse(result);
}
catch (Exception e)
{
    PrintErrorMessage(e.Message);
}

try
{
    var result = await service.SendSms(phone, message);
    PrintSubHeader($"Отправка сообщения \"{message}\" на номер {phone}");
    PrintRequest(phone, message);
    Console.WriteLine();
    PrintResponse(result);
}
catch (Exception e)
{
    PrintErrorMessage(e.Message);
}

try
{
    var result = await service.SendSms(phone, longMessage);
    PrintSubHeader($"Отправка длинного сообщения на номер {phone}");
    PrintRequest(phone, longMessage);
    Console.WriteLine();
    PrintResponse(result);
}
catch (Exception e)
{
    PrintErrorMessage(e.Message);
}

try
{
    var list = new Dictionary<string, string>()
    {
        { "+79265718860", longMessage },
        { "+79251547412", message },
        { "+79271234567", message },
        { "+79262638751", message }
    };
    var result = await service.SendSms(list);
    PrintSubHeader($"Отправка различных сообщений на различные номера");
    PrintRequestFromDictionary(list);
    Console.WriteLine();
    PrintResponse(result);
}

catch (Exception e)
{
    PrintErrorMessage(e.Message);
}

#endregion

return;

void PrintHeader(string msg)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine(new string('*', lineWidth));
    Console.Write(new string(' ', 6));
    Console.WriteLine(msg);
    Console.WriteLine(new string('*', lineWidth));
    Console.ForegroundColor = ConsoleColor.White;
}

void PrintSubHeader(string msg)
{
    Console.ForegroundColor = ConsoleColor.DarkMagenta;
    PrintLine();
    Console.WriteLine(msg);
    PrintLine();
    Console.ForegroundColor = ConsoleColor.White;
}

void PrintLine()
{
    Console.WriteLine(new string('-', lineWidth));
}

void PrintErrorMessage(string msg)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(msg);
    Console.ForegroundColor = ConsoleColor.White;
}

void PrintResponse<T>(T value) where T : class
{
    Console.WriteLine("Ответ:");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    var serializerOptions = new JsonSerializerOptions
    {
        WriteIndented = true
    };
    var jsonString = JsonSerializer.Serialize(value, serializerOptions);
    Console.WriteLine(jsonString);
    Console.ForegroundColor = ConsoleColor.White;
}

void PrintRequestFromDictionary(Dictionary<string, string> dict)
{
    Console.WriteLine("Запрос: ");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    var listResponse = string.Join(
        Environment.NewLine,
        dict.Select(l => $"Номер: {l.Key}, Сообщение: {l.Value}"));
    Console.WriteLine($"{listResponse}");
    Console.ForegroundColor = ConsoleColor.White;
}

void PrintRequest(string number, string msg)
{
    Console.WriteLine("Запрос: ");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine($"Номер телефона: {number}");
    Console.WriteLine($"Сообщение:      {msg}");
    Console.ForegroundColor = ConsoleColor.White;
}

void PrintRequestFromArray(string[] numbers, string msg)
{
    Console.WriteLine("Запрос: ");
    Console.ForegroundColor = ConsoleColor.DarkGray;
    Console.WriteLine($"Номера телефонов: {string.Join(',', numbers)}");
    Console.WriteLine($"Сообщение:        {msg}");
    Console.ForegroundColor = ConsoleColor.White;
}