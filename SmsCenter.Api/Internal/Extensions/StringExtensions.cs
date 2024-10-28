using Ardalis.GuardClauses;

namespace SmsCenter.Api.Internal.Extensions;

internal static class StringExtensions
{
    private const char Separator = ',';

    public static string ValidatePhone(this string phone) =>
        Guard.Against.InvalidFormat(phone, nameof(phone), @"^\+7\d{10}$", "Phone format is not valid");

    public static string ValidatePhonesAndJoin(this string[] phones) =>
        string.Join(Separator, phones.Select(ValidatePhone));

    public static string GetMessagesByPhoneList(this Dictionary<string, string> messagesByPhone) =>
        string.Join(
            Environment.NewLine,
            messagesByPhone.Select(l => $"{l.Key}:{l.Value}"));
}