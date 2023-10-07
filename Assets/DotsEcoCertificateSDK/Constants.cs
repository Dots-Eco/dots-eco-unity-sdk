namespace DotsEcoCertificateSDK
{
    public static class Constants
    {
        public const string BaseUrl = "https://impact.dots.eco/";

        public static string ContentType = "application/json";

        public static string GetUrlPath = $"{BaseUrl}api/v1/certificate/";

        public static string CreateUrlPath = $"{BaseUrl}api/v1/certificate/add";

        internal static string ImpactSummaryPath =  $"{BaseUrl}api/v1/impact/summary-totals";
    }
}