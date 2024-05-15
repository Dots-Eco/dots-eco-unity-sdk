using System;

namespace DotsEcoCertificateSDK.Impact
{
    [Serializable]
    public class ImpactSummaryTotalEntity
    {
        public string impact_type_category_name;
        public string impact_unit;
        public int total;
        public string unit_text;
        public string unit_html;
        public string action_in_past_tense;
        public string icon;
    }
}