using SmsCenter.Api.Internal.Exceptions;
using SmsCenter.Api.Internal.Extensions;
using SmsCenter.Api.Providers;
using static SmsCenter.Api.Models.SmsCenter;

namespace SmsCenter.Api.Services;

internal sealed class SmsCenterService(ISmsCenterProvider provider) : ISmsCenterService
{
    #region Стоимость рассылки

    public async ValueTask<Sms.CostResponseWithDetails> GetCost(string[] phones, string message)
    {
        var result = await provider.GetSmsSendingCost(
            phones.ValidatePhonesAndJoin(),
            message);

        ThrowErrorIfExists(result.Error?.Message);

        return new Sms.CostResponseWithDetails
        {
            Cost = result.Cost,
            Count = result.Count,
            Details = result.Details
        };
    }

    public async ValueTask<Sms.CostResponse> GetCost(string phone, string message)
    {
        var result = await provider.GetSmsSendingCost(
            phone.ValidatePhone(),
            message);

        ThrowErrorIfExists(result.Error?.Message);

        var details = result.Details.FirstOrDefault();

        return new Sms.CostResponse
        {
            Count = result.Count,
            Cost = result.Cost,
            Error = details?.Error,
            ErrorStatus = details?.ErrorStatus,
            Mccmnc = details!.Mccmnc
        };
    }

    public async ValueTask<Sms.CostResponseWithDetails> GetCost(Dictionary<string, string> messagesByPhone)
    {
        var result = await provider.GetSmsSendingCost(messagesByPhone.GetMessagesByPhoneList());

        ThrowErrorIfExists(result.Error?.Message);

        return new Sms.CostResponseWithDetails
        {
            Cost = result.Cost,
            Count = result.Count,
            Details = result.Details
        };
    }

    #endregion

    #region Получение баланса

    public async ValueTask<double> GetBalance()
    {
        var result = await provider.GetBalance();
        ThrowErrorIfExists(result.Error?.Message);
        return result.Balance;
    }

    #endregion

    #region Отправка смс

    public async ValueTask<Sms.Response> SendSms(string phone, string message, Sms.AdditionalOptions? options = default)
    {
        var result = await provider.SendSms(
            phones: phone.ValidatePhone(),
            message: message,
            options: options
        );

        ThrowErrorIfExists(result.Error?.Message);

        return result;
    }

    public async ValueTask<Sms.Response> SendSms(string[] phones, string message,
        Sms.AdditionalOptions? options = default)
    {
        var result = await provider.SendSms(
            phones: phones.ValidatePhonesAndJoin(),
            message: message,
            options: options
        );

        ThrowErrorIfExists(result.Error?.Message);

        return result;
    }

    public async ValueTask<Sms.Response> SendSms(Dictionary<string, string> messagesByPhone,
        Sms.AdditionalOptions? options = default)
    {
        var result = await provider.SendSms(
            phoneAndMessageList: messagesByPhone.GetMessagesByPhoneList(),
            options: options);

        ThrowErrorIfExists(result.Error?.Message);

        return result;
    }

    #endregion

    #region Проверка доступности номера

    public async ValueTask<bool> SendHlrRequest(string phone)
    {
        var result = await provider.SendHlr(phone);

        throw new NotImplementedException();
        // if (result.Id is false) return false;
        // var status = await provider.GetHlrStatus(phone, result.Id.Value);
        // return status is { Status: 1, ErrorCode: 0 };
    }

    public async ValueTask<bool> Ping(string phone)
    {
        var result = await provider.SendPing(phone);
        throw new NotImplementedException();

        // if (result.Id.HasValue is false) return false;
        // var status = await provider.GetHlrStatus(phone, result.Id.Value);
        // return status is { Status: 1, ErrorCode: 0 };
    }

    public ValueTask<Sms.Response> SendHlr(string phone)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Sms.Response> SendHlr(string[] phones)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Sms.Response> SendPing(string phone)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Sms.Response> SendPing(string[] phones)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Private methods

    private static void ThrowErrorIfExists(string? message)
    {
        if (message is not null)
        {
            throw new SmsCenterException(message);
        }
    }

    #endregion
}