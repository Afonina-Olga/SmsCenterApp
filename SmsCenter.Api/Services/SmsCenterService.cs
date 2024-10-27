using SmsCenter.Api.Internal;
using SmsCenter.Api.Providers;
using static SmsCenter.Api.Models.SmsCenter;

namespace SmsCenter.Api.Services;

internal sealed class SmsCenterService(ISmsCenterProvider provider) : ISmsCenterService
{
    #region Стоимость рассылки

    public async ValueTask<Sms.CostResponse> GetCost(string[] phones, string message)
    {
        var result = await provider.GetSmsSendingCost(
            phones.ValidatePhonesAndJoin(),
            message);
        return result;
    }

    public async ValueTask<Sms.CostResponse> GetCost(string phone, string message)
    {
        var result = await provider.GetSmsSendingCost(
            phone.ValidatePhone(),
            message);
        return result;
    }

    public async ValueTask<Sms.CostResponse> GetCost(Dictionary<string, string> messagesByPhone)
    {
        var result = await provider.GetSmsSendingCost(messagesByPhone.GetMessagesByPhoneList());
        return result;
    }

    #endregion

    #region Получение баланса

    public async ValueTask<Balance.Response> GetBalance()
    {
        var result = await provider.GetBalance();
        return result;
    }

    #endregion

    #region Отправка смс

    public async ValueTask<Sms.Response> SendSms(string phone, string message, Sms.AdditionalOptions options)
    {
        var result = await provider.SendSms(
            phones: phone.ValidatePhone(),
            message: message,
            options: options
        );

        return result;
    }

    public async ValueTask<Sms.Response> SendSms(string[] phones, string message, Sms.AdditionalOptions options)
    {
        var result = await provider.SendSms(
            phones: phones.ValidatePhonesAndJoin(),
            message: message,
            options: options
        );
       
        return result;
    }

    public async ValueTask<Sms.Response> SendSms(Dictionary<string, string> messagesByPhone,
        Sms.AdditionalOptions options)
    {
        var result = await provider.SendSms(
            phoneAndMessageList: messagesByPhone.GetMessagesByPhoneList(),
            options: options);
        
        return result;
    }

    #endregion

    #region Проверка доступности номера

    public async ValueTask<bool> SendHlrRequest(string phone)
    {
        var result = await provider.SendHlr(phone);

        if (result.Id.HasValue is false) return false;
        var status = await provider.GetHlrStatus(phone, result.Id.Value);
        return status is { Status: 1, ErrorCode: 0 };

    }

    public async ValueTask<bool> Ping(string phone)
    {
        var result = await provider.SendPing(phone);

        if (result.Id.HasValue is false) return false;
        var status = await provider.GetHlrStatus(phone, result.Id.Value);
        return status is { Status: 1, ErrorCode: 0 };

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
}