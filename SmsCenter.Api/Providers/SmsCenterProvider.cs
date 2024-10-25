using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using Ardalis.GuardClauses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SmsCenter.Api.Internal.Converters;
using SmsCenter.Api.Options;
using static SmsCenter.Api.Models.SmsCenter;

namespace SmsCenter.Api.Providers;

internal sealed class SmsCenterProvider(
    IHttpClientFactory httpClientFactory,
    ILogger<SmsCenterProvider> logger,
    IOptions<SmsCenterOptions> options)
    : ISmsCenterProvider
{
    private readonly ILogger<SmsCenterProvider> _logger = logger;
    private readonly SmsCenterOptions _options = options.Value;
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        NumberHandling = JsonNumberHandling.AllowReadingFromString,
        Converters = { DateTimeConverter.Instance }
    };

    private HttpClient CreateClient() => httpClientFactory.CreateClient(HttpClientName);

    private const string HttpClientName = "SmsCenterProviderHttpClient";

    #region Отправка смс

    public async ValueTask<Sms.Response> SendSms(string phones, string message, AdditionalOptions? options = default)
    {
        var request = new Sms.Request(_options.Login, _options.Password)
        {
            Phones = phones,
            Message = message,
            Cost = 3, // обычная отправка смс, но добавить в ответ стоимость и новый баланс Клиента.
            Op = 1, // в ответ добавляется список всех номеров телефонов с соответствующими статусами, значениями mcc и mnc, стоимостью, и, в случае ошибочных номеров, кодами ошибок.
            Err = 1, // в ответ добавляется список ошибочных номеров телефонов с соответствующими статусами
            Sender = options?.Sender,
            Id = options?.Id
        };

        var result = await SendGetRequestAsync<Sms.Request, Sms.Response>(request, _options.SmsUrl);
        return result;
    }

    public async ValueTask<Sms.Response> SendSms(string phoneAndMessageList, AdditionalOptions? options = default)
    {
        var request = new Sms.Request(_options.Login, _options.Password)
        {
            PhoneAndMessageList = phoneAndMessageList,
            Cost = 3, // обычная отправка смс, но добавить в ответ стоимость и новый баланс Клиента.
            Op = 1, // в ответ добавляется список всех номеров телефонов с соответствующими статусами, значениями mcc и mnc, стоимостью, и, в случае ошибочных номеров, кодами ошибок.
            Err = 1, // в ответ добавляется список ошибочных номеров телефонов с соответствующими статусами
            Sender = options?.Sender,
            Id = options?.Id
        };
        
        var result = await SendGetRequestAsync<Sms.Request, Sms.Response>(request, _options.SmsUrl);
        return result;
    }

    public async ValueTask<double> GetSmsSendingCost(string phones, string message)
    {
        var request = new Sms.Request(_options.Login, _options.Password)
        {
            Phones = phones,
            Message = message,
            Cost = 1 // получить стоимость рассылки без реальной отправки
        };

        var result = await SendGetRequestAsync<Sms.Request, Sms.Response>(request, _options.SmsUrl);
        return result.Cost!.Value;
    }

    public async ValueTask<double> GetSmsSendingCost(string phoneAndMessageList)
    {
        var request = new Sms.Request(_options.Login, _options.Password)
        {
            PhoneAndMessageList = phoneAndMessageList,
            Cost = 1, // получить стоимость рассылки без реальной отправки
        };

        var result = await SendGetRequestAsync<Sms.Request, Sms.Response>(request, _options.SmsUrl);
        return result.Cost!.Value;
    }

    #endregion

    #region Получение баланса

    public ValueTask<Balance.Response> GetBalance(byte? currency = default)
    {
        throw new NotImplementedException();
    }

    #endregion
    
    public ValueTask<Status.SmsResponse> GetSmsStatus(string phone, int id, byte? all = default, byte? delete = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Status.HlrResponse> GetHlrStatus(string phone, int id, byte? all = default, byte? delete = default)
    {
        throw new NotImplementedException();
    }

    public ValueTask<History.Response> GetHistory(HistoryOptions options)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Operator.Response> GetOperatorInfo(string phone)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Statistics.Response> GetStatistics(DateOnly? date = default, DateOnly? endDate = default, byte? currency = default,
        byte? balance = default)
    {
        throw new NotImplementedException();
    }
    
    #region private methods

    /// <summary>
    /// Отправить get-запрос
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    /// <param name="request"></param>
    /// <param name="url">Url</param>
    private async ValueTask<TResponse> SendGetRequestAsync<TRequest, TResponse>(TRequest request, string url)
        where TRequest : RequestBase
        where TResponse : ResponseBase
    {
        var responseString = await SendGetRequestRawAsync(request, url);
        var result = JsonSerializer.Deserialize<TResponse>(responseString, JsonSerializerOptions)!;
        result.Error = HandleErrorIfExists(responseString);
        return result;
    }

    private async ValueTask<string> SendGetRequestRawAsync<TRequest>(TRequest request, string url)
        where TRequest : RequestBase
    {
        using var httpClient = CreateClient();
        var query = GetQueryString(request);
        using var response = await httpClient.GetAsync($"{url}?{query}");
        var responseString = await response.Content.ReadAsStringAsync();
        return responseString;
    }

    /// <summary>
    /// Десериализация строки ошибки
    /// </summary>
    /// <param name="responseString">Строка ответа</param>
    private static Error? HandleErrorIfExists(string responseString)
    {
        if (responseString.Contains("error_code"))
            JsonSerializer.Deserialize<Error>(responseString, JsonSerializerOptions);

        return null;
    }

    /// <summary>
    /// Сформировать строку запроса по объекту
    /// </summary>
    /// <param name="baseModel">Объект</param>
    /// <returns>Query string в формате key1=value1&key2=value2...</returns>
    private static string GetQueryString<T>(T baseModel)
        where T : RequestBase
    {
        var jsonObject = Guard.Against.Null(
            JsonSerializer.SerializeToNode(baseModel, JsonSerializerOptions)?.AsObject(),
            "jsonObject",
            "Json object is null");

        return string.Join("&", jsonObject.Select(o => $"{o.Key}={HttpUtility.UrlEncode(o.Value!.ToString())}"));
    }
    
    #endregion
}