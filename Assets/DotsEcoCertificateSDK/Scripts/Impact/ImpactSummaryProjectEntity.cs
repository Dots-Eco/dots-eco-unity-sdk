using System;

namespace DotsEcoCertificateSDK.Impact
{
    [Serializable]
    public class ImpactSummaryProjectEntity
    {
        public int project_id;
        public string project_name;
        public string project_description;
        public string project_image_url;
        public int impact_type_id;
        public string impact_type_name;
        public string impact_description;
        public string impact_unit;
        public int impact_country;
        public int total;
        public string logo;
        public string image;
        public string unit_text;
        public string unit_html;
        public object geolocation;
    }
}