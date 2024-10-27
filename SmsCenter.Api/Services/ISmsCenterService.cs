using static SmsCenter.Api.Models.SmsCenter;

namespace SmsCenter.Api.Services;

public interface ISmsCenterService
{
    /// <summary>
    /// Получить стоимость рассылки
    /// </summary>
    /// <param name="phones">Список телефонов</param>
    /// <param name="message">Сообщение</param>
    /// <returns>Стоимость рассылки</returns>
    ValueTask<Sms.CostResponse> GetCost(string[] phones, string message);

    /// <summary>
    /// Получить стоимость рассылки
    /// </summary>
    /// <param name="phone">Номер телефона</param>
    /// <param name="message">Сообщение</param>
    /// <returns>Стоимость рассылки</returns>
    ValueTask<Sms.CostResponse> GetCost(string phone, string message);

    /// <summary>
    /// Получить стоимость рассылки
    /// </summary>
    /// <param name="list">Список телефонов и сообщений</param>
    /// <returns>Стоимость рассылки</returns>
    ValueTask<Sms.CostResponse> GetCost(Dictionary<string, string> list);

    /// <summary>
    /// Получить баланс клиента
    /// </summary>
    ValueTask<Balance.Response> GetBalance();

    /// <summary>
    /// Отправить смс на номер
    /// </summary>
    ValueTask<Sms.Response> SendSms(string phone, string message, Sms.AdditionalOptions options);

    /// <summary>
    /// Отправить смс на несколько номеров
    /// </summary>
    ValueTask<Sms.Response> SendSms(string[] phones, string message, Sms.AdditionalOptions options);

    /// <summary>
    /// Отправить на каждый номер отдельное смс
    /// </summary>
    ValueTask<Sms.Response> SendSms(Dictionary<string, string> messagesByPhone, Sms.AdditionalOptions options);

    /// <summary>
    /// Проверка телефона на доступность
    /// </summary>
    /// <param name="phone">Номер телефона</param>
    /// <returns></returns>
    ValueTask<Sms.Response> SendHlr(string phone);

    /// <summary>
    /// Проверка телефона на доступность
    /// </summary>
    /// <param name="phones"></param>
    /// <returns></returns>
    ValueTask<Sms.Response> SendHlr(string[] phones);
    
    /// <summary>
    /// Ping
    /// </summary>
    /// <param name="phone">Номер телефона</param>
    /// <returns></returns>
    ValueTask<Sms.Response> SendPing(string phone);

    /// <summary>
    /// Ping
    /// </summary>
    /// <param name="phones"></param>
    /// <returns></returns>
    ValueTask<Sms.Response> SendPing(string[] phones);
}
