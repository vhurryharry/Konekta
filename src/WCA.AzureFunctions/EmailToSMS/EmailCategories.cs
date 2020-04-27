namespace WCA.AzureFunctions.EmailToSMS
{
    public static class EmailCategories
    {
        public const string SmsGoFalse = "SMS Go False";
        public const string SmsSent = "SMS Sent";
        public const string SmsSkipped = "SMS Skipped";
        public const string SmsError = "Error Sending SMS";
        public const string GatewayBadRequest = "Gateway Bad Request";
        public const string InvalidJson = "Invalid JSON";
        public const string InvalidField = "Invalid Field";
        public const string MissingRequiredData = "Missing Required Data";
        public const string UnknownError = "Unknown Error";
    }
}