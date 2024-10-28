using System.Text.Json.Serialization;

namespace SmsCenter.Api.Models;

public static class SmsCenter
{
    public abstract record RequestBase(
        [property: JsonPropertyName("login")] string Login,
        [property: JsonPropertyName("psw")] string Password)
    {
        /// <summary>
        /// Кодировка переданного сообщения, если используется отличная от кодировки по умолчанию windows-1251
        /// </summary>
        [JsonPropertyName("charset")]
        public string Charset { get; set; } = "UTF-8";

        /// <summary>
        /// Формат ответа сервера:
        /// 0 или 1 – (по умолчанию) в виде строки(250.80).
        /// 2 – в xml формате.
        /// 3 – в json формате.
        /// </summary>
        [JsonPropertyName("fmt")]
        public byte? Format { get; set; } = 3;
    }

    public abstract record ResponseBase
    {
        public Error? Error { get; set; }
    }

    /// <summary>
    /// Ошибка отправки запроса
    /// </summary>
    /// <param name="Id">Идентификатор сообщения, переданный Клиентом или назначенный Сервером автоматически</param>
    /// <param name="Message">Сообщение об ошибке</param>
    /// <param name="Code">Код ошибки</param>
    public sealed record Error(
        [property: JsonPropertyName("id")] int? Id,
        [property: JsonPropertyName("error")] string Message,
        [property: JsonPropertyName("error_code")] byte Code);

    public static class Sms
    {
        /// <summary>
        /// Параметры запроса для отправки/проверки статуса смс
        /// </summary>
        /// <remarks>https://smsc.ru/api/http/send/sms/</remarks>
        public sealed record Request(string Login, string Password) : RequestBase(Login, Password)
        {
            // Обязательными параметрами являются login, psw, phones и mes либо login, psw и list

            #region Обязательные параметры

            ///<summary>
            /// Номер или разделенный запятой или точкой с запятой список номеров мобильных телефонов 
            /// в международном формате, на которые отправляется сообщение
            ///</summary>
            [JsonPropertyName("phones")]
            public string Phones { get; set; } = default!;

            /// <summary>
            /// Текст отправляемого сообщения. Максимальный размер – 1000 символов. 
            /// Сообщение при необходимости будет разбито на несколько SMS
            /// </summary>
            [JsonPropertyName("mes")]
            public string Message { get; set; } = default!;

            /// <summary>
            /// Список номеров телефонов и соответствующих им сообщений,
            /// разделенных двоеточием или точкой с запятой и представленный в виде:
            /// phones1:mes1
            /// phones2:mes2
            /// </summary>
            [JsonPropertyName("list")]
            public string PhoneAndMessageList { get; set; } = default!;

            #endregion

            #region Дополнительные параметры

            /// <summary>
            /// Признак HLR-запроса для получения информации о номере из базы оператора без отправки реального SMS
            /// </summary>
            [JsonPropertyName("hlr")]
            public byte? Hlr { get; set; } = default!;

            /// <summary>
            /// Признак специального SMS, не отображаемого в телефоне,
            /// для проверки номеров на доступность в реальном времени по статусу доставки
            /// </summary>
            [JsonPropertyName("ping")]
            public byte? Ping { get; set; } = default!;

            /// <summary>
            /// Идентификатор сообщения. Назначается Клиентом.
            /// Служит для дальнейшей идентификации сообщения.
            /// Если не указывать, то будет назначен автоматически.
            /// </summary>
            [JsonPropertyName("id")]
            public string? Id { get; set; } = default!;

            /// <summary>
            /// Имя отправителя, отображаемое в телефоне получателя.
            /// Разрешены английские буквы, цифры, пробел и некоторые символы.
            /// Длина – 11 символов или 15 цифр.
            /// </summary>
            [JsonPropertyName("sender")]
            public string? Sender { get; set; } = default!;

            /// <summary>
            /// Время отправки SMS-сообщения абоненту.
            /// Форматы:
            /// DDMMYYhhmm или DD.MM.YY hh:mm.
            /// h1-h2.Задает диапазон времени в часах.
            /// 0ts, где ts – timestamp, время в секундах, прошедшее с 1 января 1970 года.
            /// +m.Задает относительное смещение времени от текущего в минутах.
            /// Если time = 0 или указано уже прошедшее время, то сообщение будет отправлено немедленно.
            /// </summary>
            [JsonPropertyName("time")]
            public string Time { get; set; } = default!;

            /// <summary>
            /// Часовой пояс, в котором задается параметр time
            /// </summary>
            [JsonPropertyName("tz")]
            public string TimeZone { get; set; } = default!;

            /// <summary>
            /// Промежуток времени, в течение которого необходимо отправить рассылку
            /// </summary>
            [JsonPropertyName("period")]
            public float? Period { get; set; } = default!;

            /// <summary>
            /// Интервал или частота, с которой нужно отправлять SMS-рассылку
            /// на очередную группу номеров
            /// </summary>
            [JsonPropertyName("freq")]
            public short? Freq { get; set; } = default!;

            /// <summary>
            /// Признак Flash сообщения, отображаемого сразу на экране телефона
            /// </summary>
            [JsonPropertyName("flash")]
            public byte? Flash { get; set; } = default!;

            /// <summary>
            /// Признак wap-push сообщения, с помощью которого можно отправить интернет-ссылку на телефон
            /// </summary>
            [JsonPropertyName("push")]
            public byte? Push { get; set; } = default!;

            /// <summary>
            /// Признак whatsapp-сообщения
            /// </summary>
            [JsonPropertyName("whatsapp")]
            public byte? Whatsapp { get; set; } = default!;

            /// <summary>
            /// Признак сообщения в telegram
            /// </summary>
            [JsonPropertyName("tg")]
            public byte? Telegram { get; set; } = default!;

            /// <summary>
            /// Имя бота (telegram), в который необходимо отправить сообщение в формате "@botname_bot"
            /// </summary>
            [JsonPropertyName("bot")]
            public byte? Bot { get; set; } = default!;

            /// <summary>
            /// При указании данного параметра, система не будет отображать текст сообщения,
            /// отправленного пользователю и выводить предупреждение о необходимости
            /// подтверждения номера телефона, если с момента последнего подтверждения
            /// прошло больше smsreq дней. Диапазон значений от 10 до 999.
            /// </summary>
            [JsonPropertyName("smsreq")]
            public int SmsReq { get; set; } = default!;

            /// <summary>
            /// Признак необходимости получения стоимости рассылки
            /// </summary>
            [JsonPropertyName("cost")]
            public byte? Cost { get; set; } = default!;

            /// <summary>
            /// Срок "жизни" SMS-сообщения. Определяет время,
            /// в течение которого оператор будет пытаться доставить сообщение абоненту
            /// </summary>
            [JsonPropertyName("valid")]
            public string Valid { get; set; } = default!;

            /// <summary>
            /// Максимальное количество SMS, на которые может разбиться длинное сообщение
            /// </summary>
            [JsonPropertyName("maxsms")]
            public byte? MaxSms { get; set; } = default!;

            /// <summary>
            /// Значение буквенно-цифрового кода, введенного с "captcha" при использовании антиспам проверки
            /// </summary>
            [JsonPropertyName("imgcode")]
            public string ImageCode { get; set; } = default!;

            /// <summary>
            /// Значение IP-адреса, для которого будет действовать лимит на максимальное количество сообщений с одного IP-адреса в сутки
            /// </summary>
            [JsonPropertyName("userip")]
            public string UserIp { get; set; } = default!;

            /// <summary>
            /// Признак необходимости добавления в ответ сервера списка ошибочных номеров.
            /// </summary>
            [JsonPropertyName("err")]
            public byte? Err { get; set; } = default!;

            /// <summary>
            /// Признак необходимости добавления в ответ сервера информации по каждому номеру
            /// </summary>
            [JsonPropertyName("op")]
            public byte? Op { get; set; } = default!;

            /// <summary>
            /// Осуществляет привязку Клиента в качестве реферала к определенному ID партнера для текущего запроса
            /// </summary>
            [JsonPropertyName("pp")]
            public string Pp { get; set; } = default!;

            #endregion
        }

        public sealed record CostResponse
        {
            /// <summary>
            /// Количество смс
            /// </summary>
            [JsonPropertyName("cnt")]
            public int Count { get; set; }
            
            /// <summary>
            /// Cтоимость SMS-сообщения
            /// </summary>
            [JsonPropertyName("cost")]
            public double Cost { get; set; }
            
            /// <summary>
            /// Код статуса SMS-сообщения, заполняется при наличии ошибок
            /// </summary>
            [JsonPropertyName("status")]
            public string? ErrorStatus { get; set; } = default!;

            /// <summary>
            /// Код ошибки в статусе
            /// </summary>
            [JsonPropertyName("error")]
            public string? Error { get; set; } = default!;

            /// <summary>
            /// Числовой код страны абонента плюс числовой код оператора абонента
            /// </summary>
            [JsonPropertyName("mccmnc")]
            public string Mccmnc { get; set; } = default!;
        }

        public sealed record CostResponseWithDetails
        {
            /// <summary>
            /// Cтоимость SMS-сообщения
            /// </summary>
            [JsonPropertyName("cost")]
            public double Cost { get; set; }

            /// <summary>
            /// Количество смс
            /// </summary>
            [JsonPropertyName("cnt")]
            public int Count { get; set; }
            
            /// <summary>
            /// Детализация по номерам телефонов
            /// </summary>
            [JsonPropertyName("phones")]
            public Details[] Details { get; set; } = default!;
        }

        /// <summary>
        /// Базовый класс ответа сервера
        /// </summary>
        public sealed record Response : ResponseBase
        {
            /// <summary>
            /// Количество частей (при отправке SMS-сообщения) либо 5-секундных блоков
            /// (при голосовом сообщении (звонке))
            /// </summary>
            [JsonPropertyName("cnt")]
            public int Count { get; set; } = default!;

            /// <summary>
            /// Идентификатор сообщения, переданный Клиентом или назначенный Сервером автоматически
            /// </summary>
            [JsonPropertyName("id")]
            public int Id { get; set; } = default!;

            /// <summary>
            /// Cтоимость SMS-сообщения
            /// </summary>
            [JsonPropertyName("cost")]
            public double Cost { get; set; } = default!;

            /// <summary>
            /// Новый баланс Клиента
            /// </summary>
            [JsonPropertyName("balance")]
            public double Balance { get; set; } = default!;

            /// <summary>
            /// Детализация по номерам телефонов
            /// </summary>
            [JsonPropertyName("phones")]
            public Details[] Details { get; set; } = default!;
        }

        /// <summary>
        /// Детализация отправки сообщений
        /// </summary>
        public sealed record Details
        {
            /// <summary>
            /// Номер телефона.
            /// </summary>
            [JsonPropertyName("phone")]
            public string Phone { get; set; } = default!;

            /// <summary>
            /// Числовой код страны абонента плюс числовой код оператора абонента
            /// </summary>
            [JsonPropertyName("mccmnc")]
            public string Mccmnc { get; set; } = default!;

            /// <summary>
            /// Стоимость SMS-сообщения
            /// </summary>
            [JsonPropertyName("cost")]
            public double Cost { get; set; } = default!;

            /// <summary>
            /// Код статуса SMS-сообщения, заполняется при наличии ошибок
            /// </summary>
            [JsonPropertyName("status")]
            public string? ErrorStatus { get; set; } = default!;

            /// <summary>
            /// Код ошибки в статусе
            /// </summary>
            [JsonPropertyName("error")]
            public string? Error { get; set; } = default!;
        }

        /// <summary>
        /// Дополнительные параметры запроса смс
        /// </summary>
        public sealed record AdditionalOptions
        {
            /// <summary>
            /// Идентификатор сообщения. Назначается Клиентом.
            /// Служит для дальнейшей идентификации сообщения.
            /// Если не указывать, то будет назначен автоматически.
            /// </summary>
            [JsonPropertyName("id")]
            public string? Id { get; set; } = default!;

            /// <summary>
            /// Имя отправителя, отображаемое в телефоне получателя.
            /// Разрешены английские буквы, цифры, пробел и некоторые символы.
            /// Длина – 11 символов или 15 цифр.
            /// </summary>
            [JsonPropertyName("sender")]
            public string? Sender { get; set; } = default!;
        }
    }

    /// <summary>
    /// Параметры запроса для получения баланса
    /// </summary>
    public sealed class Balance
    {
        /// <summary>
        /// Запрос
        /// </summary>
        public sealed record Request(string Login, string Password) :
            RequestBase(Login, Password)
        {
            /// <summary>
            /// Флаг, указывающий на необходимость добавления в ответ сервера названия валюты Клиента.
            /// </summary>
            [JsonPropertyName("cur")]
            public byte? Currency { get; set; }
        }

        /// <summary>
        /// Ответ сервера
        /// </summary>
        public sealed record Response : ResponseBase
        {
            /// <summary>
            /// Текущее состояние баланса
            /// </summary>
            [JsonPropertyName("balance")]
            public double Balance { get; set; }

            /// <summary>
            /// Текущее состояние установленного кредита
            /// </summary>
            [JsonPropertyName("credit")]
            public string? Credit { get; set; } = default!;

            /// <summary>
            /// Валюта Клиента
            /// </summary>
            [JsonPropertyName("currency")]
            public string? Currency { get; set; } = default!;
        }
    }

    /// <summary>
    /// Статусы сообщений
    /// </summary>
    public static class Status
    {
        public sealed record Request(string Login, string Password) : RequestBase(Login, Password)
        {
            /// <summary>
            /// Номер телефона или список номеров через запятую при запросе статусов нескольких SMS. 
            /// </summary>
            /// <remarks>
            /// При множественном запросе номера в списке должны быть перечислены в порядке, соответствующем идентификаторам сообщений. 
            /// Для сохранения формата множественного запроса при запросе статуса одного сообщения укажите запятую после номера телефона. 
            /// Это описание подходит и для e-mail-сообщений.
            /// </remarks>
            [JsonPropertyName("phone")]
            public string Phone { get; set; } = default!;

            /// <summary>
            /// Идентификатор сообщения или список идентификаторов через запятую при запросе статусов нескольких сообщений
            /// </summary>
            /// <remarks>
            /// Для сохранения формата множественного запроса при запросе статуса одного сообщения укажите запятую после идентификатора сообщения.
            /// </remarks>
            [JsonPropertyName("id")]
            public int Id { get; set; }

            /// <summary>
            /// 0 – (по умолчанию) получить статус сообщения в обычном формате.
            /// 1 – получить полную информацию об отправленном сообщении.
            /// 2 – добавить в информацию о сообщении данные о стране, операторе и регионе абонента.
            /// </summary>
            [JsonPropertyName("all")]
            public byte? All { get; set; }

            /// <summary>
            /// 1 – удалить ранее отправленное сообщение
            /// </summary>
            /// <remarks>https://smsc.ru/api/http/status_messages/delete/</remarks>
            [JsonPropertyName("del")]
            public byte? Delete { get; set; }
        }

        public record Response : ResponseBase
        {
            /// <summary>
            /// Код статуса
            /// </summary>
            [JsonPropertyName("status")]
            public byte Status { get; set; }

            /// <summary>
            /// Дата последнего изменения статуса. Формат DD.MM.YYYY hh:mm:ss.
            /// </summary>
            [JsonPropertyName("last_date")]
            public DateTime LastDate { get; set; }

            /// <summary>
            /// Штамп времени последнего изменения статуса
            /// </summary>
            [JsonPropertyName("last_timestamp")]
            public long LastTimestamp { get; set; }

            /// <summary>
            /// Код HLR-ошибки или статуса абонента
            /// </summary>
            [JsonPropertyName("err")]
            public int ErrorCode { get; set; }

            /// <summary>
            /// Дата отправки сообщения (формат DD.MM.YYYY hh:mm:ss)
            /// </summary>
            [JsonPropertyName("send_date")]
            public DateTime? SendDate { get; set; }

            /// <summary>
            /// Штамп времени отправки сообщения
            /// </summary>
            [JsonPropertyName("send_timestamp")]
            public long? SendTimestamp { get; set; }

            /// <summary>
            /// Номер телефона абонента
            /// </summary>
            [JsonPropertyName("phone")]
            public string? Phone { get; set; }

            /// <summary>
            /// Стоимость сообщения
            /// </summary>
            [JsonPropertyName("cost")]
            public double? Cost { get; set; }

            /// <summary>
            /// Имя отправителя
            /// </summary>
            [JsonPropertyName("sender_id")]
            public string? SenderId { get; set; }

            /// <summary>
            /// Название статуса
            /// </summary>
            [JsonPropertyName("status_name")]
            public string? StatusName { get; set; }

            /// <summary>
            /// Текст сообщения
            /// </summary>
            [JsonPropertyName("message")]
            public string? Message { get; set; }

            /// <summary>
            /// Тип сообщения (0 – SMS, 1 – Flash-SMS, 2 – Бинарное SMS, 3 – Wap-push, 4 – HLR-запрос, 5 – Ping-SMS, 6 – MMS, 7 – Звонок, 8 – E-mail, 10 – Viber, 12 – Соцсети)
            /// </summary>
            [JsonPropertyName("type")]
            public byte? Type { get; set; }

            /// <summary>
            /// Название страны регистрации номера абонента
            /// </summary>
            [JsonPropertyName("country")]
            public string? Country { get; set; }

            /// <summary>
            /// Название оператора абонента
            /// </summary>
            [JsonPropertyName("operator")]
            public string? Operator { get; set; }

            /// <summary>
            /// регион регистрации номера абонента
            /// </summary>
            [JsonPropertyName("region")]
            public string? Region { get; set; }
        }

        public record SmsResponse : Response
        {
            /// <summary>
            /// Комментарий сообщения
            /// </summary>
            [JsonPropertyName("comment")]
            public string? Comment { get; set; }

            /// <summary>
            /// Числовой код страны абонента плюс числовой код оператора абонента
            /// </summary>
            [JsonPropertyName("mccmnc")]
            public string? Mccmnc { get; set; }

            [JsonPropertyName("operator_orig")] public string? OperatorOriginal { get; set; }

            /// <summary>
            /// Количество частей в SMS-сообщении (либо секунд в голосовом сообщении)
            /// </summary>
            [JsonPropertyName("sms_cnt")]
            public int? Count { get; set; }
        }

        /// <summary>
        /// Ответ для hlr запросов
        /// </summary>
        public record HlrResponse : Response
        {
            /// <summary>
            /// Уникальный код IMSI SIM-карты абонента
            /// </summary>
            [JsonPropertyName("imsi")]
            public string Imsi { get; set; } = default!;

            /// <summary>
            /// Номер сервис-центра оператора, в сети которого находится абонент
            /// </summary>
            [JsonPropertyName("msc")]
            public string Msc { get; set; } = default!;

            /// <summary>
            /// Числовой код страны абонента
            /// </summary>
            [JsonPropertyName("mcc")]
            public string Mcc { get; set; } = default!;

            /// <summary>
            /// Числовой код оператора абонента
            /// </summary>
            [JsonPropertyName("mnc")]
            public string Mnc { get; set; } = default!;

            /// <summary>
            /// Название страны регистрации абонента
            /// </summary>
            [JsonPropertyName("cn")]
            public string Cn { get; set; } = default!;

            /// <summary>
            /// Название оператора регистрации абонента
            /// </summary>
            [JsonPropertyName("net")]
            public string Net { get; set; } = default!;

            /// <summary>
            /// Название роуминговой страны абонента при нахождении в чужой сети
            /// </summary>
            [JsonPropertyName("rcn")]
            public string Rcn { get; set; } = default!;

            /// <summary>
            /// Название роумингового оператора абонента при нахождении в чужой сети
            /// </summary>
            [JsonPropertyName("rnet")]
            public string Rnet { get; set; } = default!;
        }
    }

    /// <summary>
    /// История отправленных сообщений
    /// </summary>
    public static class History
    {
        public sealed record Request(string Login, string Password) : RequestBase(Login, Password)
        {
            [JsonPropertyName("get_message")] public byte GetMessage { get; set; } = 1;

            /// <summary>
            /// Начальная дата в периоде, за который запрашивается история. Формат: 'дд.мм.гггг'
            /// </summary>
            [JsonPropertyName("start")]
            public string? StartDate { get; set; }

            /// <summary>
            /// Конечная дата в периоде. Формат: 'дд.мм.гггг'
            /// </summary>
            [JsonPropertyName("end")]
            public string? EndDate { get; set; }

            /// <summary>
            /// Номер или разделенный запятыми список номеров телефонов, для которых необходимо получить историю отправленных SMS-сообщений.
            /// </summary>
            [JsonPropertyName("phone")]
            public string? Phone { get; set; }

            /// <summary>
            /// E-mail адрес или разделенный запятыми список адресов, для которых необходимо получить историю отправленных e-mail сообщений
            /// </summary>
            [JsonPropertyName("email")]
            public string? Email { get; set; }

            /// <summary>
            /// Количество возвращаемых в ответе сообщений. Максимальное значение равно 1000.
            /// </summary>
            [JsonPropertyName("cnt")]
            public int? Count { get; set; }

            /// <summary>
            /// Глобальный идентификатор сообщения (параметр int_id в ответе Сервера), назначаемый Сервером автоматически. 
            /// </summary>
            [JsonPropertyName("prev_id")]
            public int? PrevId { get; set; }

            /// <summary>
            /// Признак запроса e-mail сообщений.
            /// 0 (по умолчанию) – запрос SMS-сообщений.
            /// 8 – запрос e-mail сообщений.
            /// </summary>
            [JsonPropertyName("format")]
            public byte? EmailFormat { get; set; }
        }

        public record Response : ResponseBase
        {
            public Details[]? Details { get; set; }
        }

        public record Details
        {
            /// <summary>
            /// Код статуса
            /// </summary>
            [JsonPropertyName("status")]
            public byte Status { get; set; }

            /// <summary>
            /// Дата последнего изменения статуса. Формат DD.MM.YYYY hh:mm:ss.
            /// </summary>
            [JsonPropertyName("last_date")]
            public DateTime LastDate { get; set; }

            /// <summary>
            /// Штамп времени последнего изменения статуса
            /// </summary>
            [JsonPropertyName("last_timestamp")]
            public long LastTimestamp { get; set; }

            /// <summary>
            /// Код HLR-ошибки или статуса абонента
            /// </summary>
            [JsonPropertyName("err")]
            public int ErrorCode { get; set; }

            /// <summary>
            /// Дата отправки сообщения (формат DD.MM.YYYY hh:mm:ss)
            /// </summary>
            [JsonPropertyName("send_date")]
            public DateTime? SendDate { get; set; }

            /// <summary>
            /// Штамп времени отправки сообщения
            /// </summary>
            [JsonPropertyName("send_timestamp")]
            public long? SendTimestamp { get; set; }

            /// <summary>
            /// Номер телефона абонента
            /// </summary>
            [JsonPropertyName("phone")]
            public string? Phone { get; set; }

            /// <summary>
            /// Стоимость сообщения
            /// </summary>
            [JsonPropertyName("cost")]
            public double? Cost { get; set; }

            /// <summary>
            /// Имя отправителя
            /// </summary>
            [JsonPropertyName("sender_id")]
            public string? SenderId { get; set; }

            /// <summary>
            /// Название статуса
            /// </summary>
            [JsonPropertyName("status_name")]
            public string? StatusName { get; set; }

            /// <summary>
            /// Текст сообщения
            /// </summary>
            [JsonPropertyName("message")]
            public string? Message { get; set; }

            /// <summary>
            /// Тип сообщения (0 – SMS, 1 – Flash-SMS, 2 – Бинарное SMS, 3 – Wap-push, 4 – HLR-запрос, 5 – Ping-SMS, 6 – MMS, 7 – Звонок, 8 – E-mail, 10 – Viber, 12 – Соцсети)
            /// </summary>
            [JsonPropertyName("type")]
            public byte? Type { get; set; }

            /// <summary>
            /// Название страны регистрации номера абонента
            /// </summary>
            [JsonPropertyName("country")]
            public string? Country { get; set; }

            /// <summary>
            /// Название оператора абонента
            /// </summary>
            [JsonPropertyName("operator")]
            public string? Operator { get; set; }

            /// <summary>
            /// регион регистрации номера абонента
            /// </summary>
            [JsonPropertyName("region")]
            public string? Region { get; set; }

            /// <summary>
            /// Идентификатор сообщения
            /// </summary>
            [JsonPropertyName("id")]
            public int Id { get; set; }

            /// <summary>
            /// Глобальный идентификатор сообщения
            /// </summary>
            [JsonPropertyName("int_id")]
            public int IntId { get; set; }

            /// <summary>
            /// Количество частей в сообщении
            /// </summary>
            [JsonPropertyName("sms_cnt")]
            public int Count { get; set; }

            /// <summary>
            /// Формат сообщения
            /// </summary>
            [JsonPropertyName("format")]
            public byte EmailFormat { get; set; }

            /// <summary>
            /// Контрольная сумма сообщения
            /// </summary>
            [JsonPropertyName("crc")]
            public int Crc { get; set; }

            /// <summary>
            /// Мобильный код страны и мобильный код оператора
            /// </summary>
            [JsonPropertyName("mccmnc")]
            public string MccMnc { get; set; } = default!;
        }
    }

    /// <summary>
    /// Получение данных об операторе абонента
    /// </summary>
    public static class Operator
    {
        public sealed record Request(string Login, string Password) : RequestBase(Login, Password)
        {
            public string Phone { get; set; } = default!;
        }

        public sealed record Response : ResponseBase
        {
            /// <summary>
            /// Название страны регистрации номера абонента
            /// </summary>
            [JsonPropertyName("country")]
            public string Country { get; set; } = default!;

            /// <summary>
            /// Мобильный оператор абонента
            /// </summary>
            [JsonPropertyName("operator")]
            public string Operator { get; set; } = default!;

            /// <summary>
            /// Регион регистрации номера абонента
            /// </summary>
            [JsonPropertyName("region")]
            public string Region { get; set; } = default!;

            /// <summary>
            /// Числовой код страны абонента
            /// </summary>
            [JsonPropertyName("mcc")]
            public string Mcc { get; set; } = default!;

            /// <summary>
            /// Числовой код оператора абонента
            /// </summary>
            [JsonPropertyName("mnc")]
            public string Mnc { get; set; } = default!;

            /// <summary>
            /// Часовой пояс региона регистрации номера абонента
            /// </summary>
            [JsonPropertyName("tz")]
            public string Tz { get; set; } = default!;
        }
    }

    /// <summary>
    /// Получение статистики
    /// </summary>
    public static class Statistics
    {
        public sealed record Request(string Login, string Password) : RequestBase(Login, Password)
        {
            [JsonPropertyName("get_stat")] public byte GetStatistics { get; set; } = 1;

            /// <summary>
            /// Начальная дата в периоде, за который запрашивается статистика.Формат: 'дд.мм.гггг'
            /// </summary>
            [JsonPropertyName("start")]
            public string? StartDate { get; set; }

            /// <summary>
            /// Конечная дата в периоде.Если не указана, то возвращаются данные с начальной даты. Формат: 'дд.мм.гггг'
            /// </summary>
            [JsonPropertyName("end")]
            public string? EndDate { get; set; }

            /// <summary>
            /// Флаг, позволяющий выводить статистику в текущей валюте Клиента
            /// </summary>
            [JsonPropertyName("mycur")]
            public byte? Currency { get; set; }

            /// <summary>
            /// Флаг, позволяющий получить статистику по отправленным сообщениям, оплаченным с электронного баланса
            /// </summary>
            [JsonPropertyName("balance2")]
            public byte? Balance2 { get; set; }
        }

        public sealed record Response : ResponseBase
        {
            /// <summary>
            /// Логин Клиента
            /// </summary>
            [JsonPropertyName("login")]
            public byte? Login { get; set; }

            /// <summary>
            /// Количество сообщений
            /// </summary>
            [JsonPropertyName("sms")]
            public int? Count { get; set; }

            /// <summary>
            /// Расход
            /// </summary>
            [JsonPropertyName("credit")]
            public double? Credit { get; set; }

            /// <summary>
            /// Приход
            /// </summary>
            [JsonPropertyName("debit")]
            public double? Debit { get; set; }

            /// <summary>
            /// Трехсимвольный код валюты
            /// </summary>
            [JsonPropertyName("currency")]
            public string? Currency { get; set; }
        }
    }

    /// <summary>
    /// Дополнительные параметры запроса истории
    /// </summary>
    public sealed record HistoryOptions
    {
        public DateOnly? StartDate { get; set; } = default!;

        public DateOnly? EndDate { get; set; } = default!;

        public string? Phone { get; set; } = default!;

        public string? Email { get; set; } = default!;

        public int? Count { get; set; } = default!;

        public int? PrevId { get; set; } = default!;

        public byte? EmailFormat { get; set; } = default!;
    }
}