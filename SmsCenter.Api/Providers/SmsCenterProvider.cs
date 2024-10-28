using System.Text;
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
        Converters =
        {
            DateTimeConverter.Instance,
            StringConverter.Instance
        }
    };

    private HttpClient CreateClient() => httpClientFactory.CreateClient(HttpClientName);

    public const string HttpClientName = "SmsCenterProviderHttpClient";

    #region Стоимость рассылки
    
    public async ValueTask<Sms.Response> GetSmsSendingCost(string phones, string message)
    {
        var request = new Sms.Request(_options.Login, _options.Password)
        {
            Phones = phones,
            Message = message,
            Cost = 1, // получить стоимость рассылки без реальной отправки
            Op = 1
        };

        var result = await SendGetRequestAsync<Sms.Request, Sms.Response>(request, _options.SmsUrl);

        return result;
    }

    public async ValueTask<Sms.Response> GetSmsSendingCost(string phoneAndMessageList)
    {
        var request = new Sms.Request(_options.Login, _options.Password)
        {
            PhoneAndMessageList = phoneAndMessageList,
            Cost = 1, // получить стоимость рассылки без реальной отправки
            Op = 1
        };

        var result = await SendGetRequestAsync<Sms.Request, Sms.Response>(request, _options.SmsUrl);
        return result;
    }
    
    #endregion
    
    #region Получение баланса

    public async ValueTask<Balance.Response> GetBalance(byte? currency = default)
    {
        var request = new Balance.Request(_options.Login, _options.Password)
        {
            Currency = currency
        };
        var result = await SendGetRequestAsync<Balance.Request, Balance.Response>(request, _options.BalanceUrl);
        return result;
    }

    #endregion
    
    #region Отправка смс

    public async ValueTask<Sms.Response> SendSms(string phones, string message,
        Sms.AdditionalOptions? options = default)
    {
        var request = new Sms.Request(_options.Login, _options.Password)
        {
            Phones = phones,
            Message = message,
            Cost = 3, // обычная отправка смс, но добавить в ответ стоимость и новый баланс Клиента.
            Op = 1, // в ответ добавляется список всех номеров телефонов с соответствующими статусами, значениями mcc и mnc, стоимостью, и, в случае ошибочных номеров, кодами ошибок.
            Sender = options?.Sender,
            Id = options?.Id
        };

        var result = await SendGetRequestAsync<Sms.Request, Sms.Response>(request, _options.SmsUrl);
        return result;
    }

    public async ValueTask<Sms.Response> SendSms(string phoneAndMessageList, Sms.AdditionalOptions? options = default)
    {
        var request = new Sms.Request(_options.Login, _options.Password)
        {
            PhoneAndMessageList = phoneAndMessageList,
            Cost = 3, // обычная отправка смс, но добавить в ответ стоимость и новый баланс Клиента.
            Op = 1, // в ответ добавляется список всех номеров телефонов с соответствующими статусами, значениями mcc и mnc, стоимостью, и, в случае ошибочных номеров, кодами ошибок.
            Sender = options?.Sender,
            Id = options?.Id
        };

        var result = await SendGetRequestAsync<Sms.Request, Sms.Response>(request, _options.SmsUrl);
        return result;
    }

    public async ValueTask<Sms.Response> SendHlr(string phones)
    {
        var request = new Sms.Request(_options.Login, _options.Password)
        {
            Phones = phones,
            Cost = 3, // обычная отправка смс, но добавить в ответ стоимость и новый баланс Клиента.
            Op = 1, // в ответ добавляется список всех номеров телефонов с соответствующими статусами, значениями mcc и mnc, стоимостью, и, в случае ошибочных номеров, кодами ошибок.
            Err = 1, // в ответ добавляется список ошибочных номеров телефонов с соответствующими статусами
            Hlr = 1
        };

        var result = await SendGetRequestAsync<Sms.Request, Sms.Response>(request, _options.SmsUrl);
        return result;
    }

    public async ValueTask<Sms.Response> SendPing(string phones)
    {
        var request = new Sms.Request(_options.Login, _options.Password)
        {
            Phones = phones,
            Cost = 3, // обычная отправка смс, но добавить в ответ стоимость и новый баланс Клиента.
            Op = 1, // в ответ добавляется список всех номеров телефонов с соответствующими статусами, значениями mcc и mnc, стоимостью, и, в случае ошибочных номеров, кодами ошибок.
            Err = 1, // в ответ добавляется список ошибочных номеров телефонов с соответствующими статусами
            Ping = 1
        };

        var result = await SendGetRequestAsync<Sms.Request, Sms.Response>(request, _options.SmsUrl);
        return result;
    }

    #endregion

    

    #region Статус доставки

    public async ValueTask<Status.SmsResponse> GetSmsStatus(string phone, int id, byte? all = null, byte? delete = null)
    {
        var request = new Status.Request(_options.Login, _options.Password)
        {
            Phone = phone,
            Id = id,
            All = all,
            Delete = delete
        };

        var result = await SendGetRequestAsync<Status.Request, Status.SmsResponse>(request, _options.StatusUrl);
        return result;
    }

    public async ValueTask<Status.HlrResponse> GetHlrStatus(string phone, int id, byte? all = null, byte? delete = null)
    {
        var request = new Status.Request(_options.Login, _options.Password)
        {
            Phone = phone,
            Id = id,
            All = all,
            Delete = delete
        };

        var result = await SendGetRequestAsync<Status.Request, Status.HlrResponse>(request, _options.StatusUrl);
        return result;
    }

    #endregion

    #region Получение истории

    public async ValueTask<History.Response> GetHistory(HistoryOptions options)
    {
        var request = new History.Request(_options.Login, _options.Password)
        {
            StartDate = options.StartDate.ToString(),
            EndDate = options.EndDate.ToString(),
            EmailFormat = options.EmailFormat,
            Phone = options.Phone,
            Email = options.Email,
            PrevId = options.PrevId,
            Count = options.Count
        };
        var result = await SendGetRequestAsync<History.Request, History.Response>(request, _options.GetUrl);
        return result;
    }

    #endregion

    #region Получение информации об операторе

    public async ValueTask<Operator.Response> GetOperatorInfo(string phone)
    {
        var request = new Operator.Request(_options.Login, _options.Password)
        {
            Phone = phone
        };

        var result = await SendGetRequestAsync<Operator.Request, Operator.Response>(request, _options.OperatorUrl);
        return result;
    }

    #endregion

    #region Получение статистики

    public async ValueTask<Statistics.Response> GetStatistics(DateOnly? startDate = null, DateOnly? endDate = null,
        byte? currency = null, byte? balance2 = null)
    {
        var request = new Statistics.Request(_options.Login, _options.Password)
        {
            StartDate = startDate?.ToString(),
            EndDate = endDate?.ToShortDateString(),
            Balance2 = balance2,
            Currency = currency
        };

        var result = await SendGetRequestAsync<Statistics.Request, Statistics.Response>(request, _options.GetUrl);
        return result;
    }

    #endregion

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

    /// <summary>
    /// Отправка get-запроса
    /// </summary>
    /// <param name="request">Параметры запроса</param>
    /// <param name="url"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <returns></returns>
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
    /// Отправка post-запроса
    /// </summary>
    /// <param name="request"></param>
    /// <param name="url"></param>
    /// <typeparam name="TRequest"></typeparam>
    /// <returns></returns>
    private async ValueTask<string> SendPostRequestRawAsync<TRequest>(TRequest request, string url)
    {
        using var httpClient = CreateClient();
        using var response = await httpClient.PostAsync(
            url,
            new StringContent(
                JsonSerializer.Serialize(request, JsonSerializerOptions),
                Encoding.UTF8,
                "application/json")
        );
        var responseString = await response.Content.ReadAsStringAsync();
        return responseString;
    }

    /// <summary>
    /// Десериализация строки ошибки
    /// </summary>
    /// <param name="responseString">Строка ответа</param>
    private static Error? HandleErrorIfExists(string responseString)
    {
        return responseString.Contains("error_code")
            ? JsonSerializer.Deserialize<Error>(responseString, JsonSerializerOptions)
            : null;
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