using static SmsCenter.Api.Models.SmsCenter;

namespace SmsCenter.Api.Providers;

internal interface ISmsCenterProvider
{
    #region Отправка смс https: //smsc.ru/api/http/send

    /// <summary>
    /// Отправка одного и того же сообщения смс на один или несколько номеров
    /// </summary>
    /// <param name="phones">Телефонные номера, разделенные запятой</param>
    /// <param name="message">Сообщение</param>
    /// <param name="options">Дополнительные параметры запроса</param>
    ValueTask<Sms.Response> SendSms(
        string phones,
        string message,
        Sms.AdditionalOptions? options = default);

    /// <summary>
    /// Отправка различных сообщений смс на несколько номеров
    /// </summary>
    /// <param name="phoneAndMessageList">Список номеров телефонов и соответствующих им сообщений, разделенных двоеточием или точкой с запятой</param>
    /// <param name="options">Дополнительные параметры запроса</param>
    ValueTask<Sms.Response> SendSms(
        string phoneAndMessageList,
        Sms.AdditionalOptions? options = default);

    /// <summary>
    /// Получить стоимость отправки смс
    /// </summary>
    /// <param name="phones">Телефонные номера, разделенные запятой</param>
    /// <param name="message">Сообщение</param>
    ValueTask<Sms.CostResponse> GetSmsSendingCost(string phones, string message);

    /// <summary>
    /// Получить стоимость отправки смс
    /// </summary>
    /// <param name="phoneAndMessageList">Список номеров телефонов и соответствующих им сообщений, разделенных двоеточием или точкой с запятой</param>
    /// <returns></returns>
    ValueTask<Sms.CostResponse> GetSmsSendingCost(string phoneAndMessageList);

    ValueTask<Sms.Response> SendHlr(string phone);
    
    ValueTask<Sms.Response> SendPing(string phone);

    #endregion


    /// <summary>
    /// Получение баланса
    /// </summary>
    /// <param name="currency">Валюта</param>
    /// <returns></returns>
    ValueTask<Balance.Response> GetBalance(byte? currency = default);

    /// <summary>
    /// Статус доставки смс сообщения
    /// </summary>
    /// <param name="phone">Номер телефона или список номеров через запятую при запросе статусов нескольких SMS.</param>
    /// <param name="id">
    /// Идентификатор сообщения или список идентификаторов через
    /// запятую при запросе статусов нескольких сообщений.
    /// Для сохранения формата множественного запроса при запросе статуса
    /// одного сообщения укажите запятую после идентификатора сообщения.
    /// </param>
    /// <param name="all">
    /// 0 – (по умолчанию) получить статус сообщения в обычном формате.
    /// 1 – получить полную информацию об отправленном сообщении.
    /// 2 – добавить в информацию о сообщении данные о стране, операторе и регионе абонента.
    /// </param>
    /// <param name="delete">
    /// 1 – удалить ранее отправленное сообщение. Используется совместно с параметрами phone и id.
    /// </param>
    ValueTask<Status.SmsResponse> GetSmsStatus(
        string phone,
        int id,
        byte? all = default,
        byte? delete = default);

    // Статус проверки доступности номера
    ValueTask<Status.HlrResponse> GetHlrStatus(
        string phone,
        int id,
        byte? all = default,
        byte? delete = default);

    // Получение истории отправленных сообщений
    ValueTask<History.Response> GetHistory(HistoryOptions options);

    // Получение информации об операторе абонента
    ValueTask<Operator.Response> GetOperatorInfo(string phone);

    // Получение статистики
    ValueTask<Statistics.Response> GetStatistics(
        DateOnly? date = default,
        DateOnly? endDate = default,
        byte? currency = default,
        byte? balance = default);
}