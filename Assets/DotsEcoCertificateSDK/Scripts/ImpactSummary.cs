
namespace DotsEcoCertificateSDK
{
    [System.Serializable]
    public class ImpactSummary
    {
        public string impact_type_category_name;
        public string impact_unit;
        public string total;
        public string unit;

        public ImpactSummary(string impact_type_category_name, string impact_unit, string total, string unit)
        {
            this.impact_type_category_name = impact_type_category_name;
            this.impact_unit = impact_unit;
            this.total = total;
            this.unit = unit;
        }
    }
}