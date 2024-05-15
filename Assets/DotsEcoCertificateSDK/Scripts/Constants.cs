namespace DotsEcoCertificateSDK
{
    public static class Constants
    {
        public const string BaseUrl = "https://impact.dots.eco/";

        public static string ContentType = "application/json";

        public static string GetUrlPath = $"{BaseUrl}api/v1/certificate/";

        public static string EmailNotificationPath = $"{BaseUrl}api/v1/notificaiton-subscription";
        
        // $"{BaseUrl}api/v1/certificate/list/{app_token}/{remote_user_id}";
        public static string GetListUrlPath = $"{BaseUrl}api/v1/certificate/list/";
        
        public static string CreateUrlPath = $"{BaseUrl}api/v1/certificate/add?format=sdk";

        public static string ImpactSummaryTotalsPath =  $"{BaseUrl}api/v1/impact/summary-totals";

        public static string ImpactSummaryProjectPath = $"{BaseUrl}api/v1/impact/summary";

        public static string CertificateIDName = "CertificateID";
    }
}