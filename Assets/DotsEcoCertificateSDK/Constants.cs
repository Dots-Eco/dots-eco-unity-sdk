namespace DotsEcoCertificateSDK
{
    public static class Constants
    {
        public const string BaseUrl = "https://impact.dots.eco/";

        public static string ContentType = "application/json";

        public static string GetUrlPath = $"{BaseUrl}api/v1/certificate/";

        //public static string CreateUrlPath = $"{BaseUrl}api/v1/certificate/add";
        public static string CreateUrlPath = $"{BaseUrl}api/v1/certificate/add?format=sdk";

        internal static string ImpactSummaryPath =  $"{BaseUrl}api/v1/impact/summary-totals";
        
        public static string FakeJSONResponse = @"
          {
            ""certificate_id"": ""134411-6-5"",
            ""certificate_url"": ""https://impact.dots.eco/certificate/1ca15b15-dec1-494b-884f-a9fefa953a67"",
            ""certificate_image_url"": ""https://impact.dots.eco/certificate/img/1ca15b15-dec1-494b-884f-a9fefa953a67.jpg"",
            ""app_id"": ""6"",
            ""app_name"": ""Dots.eco"",
            ""remote_user_id"": ""testuser"",
            ""name_on_certificate"": ""John Doe"",
            ""certificate_design"": null,
            ""certificate_info"": null,
            ""impact_qty"": ""100"",
            ""impact_type_id"": ""196"",
            ""impact_type_name"": ""Save a Sea Turtle"",
            ""impact_status"": null,
            ""created_timestamp"": ""1663080297"",
            ""allocation_id"": ""14"",
            ""country"": ""Indonesia"",
            ""geolocation"": [
              {
                ""lat"": -8.1420890000000004,
                ""lng"": 114.65480599999999
              }
            ],
            ""rendering"": {
              ""app"": {
                ""logo_url"": ""https://nginx.staging.dots-eco-drupal.de3.amazee.io/sites/default/files/2024-01/app.png""
              },
              ""impact_title"": ""For saving 100 Sea-Turtle hatchings with Double Hit Bingo"",
              ""theme"": {
                ""impact_type_category"": {
                ""icon_url"": ""https://nginx.staging.dots-eco-drupal.de3.amazee.io/sites/default/files/2024-01/tree_icon.png""
                },
                ""category_theme"": {
                  ""name"": ""land"",
                  ""primary"": ""#126808"",
                  ""secondary"": ""#4eff14eb"",
                  ""tertiary"": ""#a22d0999"",
                  ""background"": ""#bb4214eb"",
                  ""scratch_image_url"": ""https://nginx.staging.dots-eco-drupal.de3.amazee.io/sites/default/files/2024-01/scratch.svg""
                }
              }
            }
          }";
    }
}