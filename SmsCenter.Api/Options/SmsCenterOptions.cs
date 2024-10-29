namespace SmsCenter.Api.Options;

/// <summary>
/// Опции SmsCenter
/// </summary>
internal sealed class SmsCenterOptions
{
    /// <summary>
    /// Url для отправки sms
    /// </summary>
    public string SmsUrl { get; private set; } = default!;

    /// <summary>
    /// Url для отправки email
    /// </summary>
    public string EmailUrl { get; private set; } = default!;

    /// <summary>
    /// Url для получения баланса
    /// </summary>
    public string BalanceUrl { get; private set; } = default!;

    /// <summary>
    /// Url для получения статуса
    /// </summary>
    public string StatusUrl { get; private set; } = default!;

    /// <summary>
    /// Url для получения информации об операторе абонента
    /// </summary>
    public string OperatorUrl { get; private set; } = default!;

    /// <summary>
    /// Url для получения дополнительной информации (статистики, истории и т.д.)
    /// </summary>
    public string GetUrl { get; private set; } = default!;

    /// <summary>
    /// Логин
    /// </summary>
    public string Login { get; private set; } = default!;

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; private set; } = default!;

    /// <summary>
    /// Отправитель email 
    /// </summary>
    public string EmailSender { get; private set; } = default!;
    
    /// <summary>
    /// Задержка между последовательными запросами
    /// </summary>
    public int DelayBetweenRequests { get; private set; } 
}