using System.Collections.Generic;

namespace DotsEcoCertificateSDK.Impact
{
    public class ImpactSummaryTotalResponse
    {
        public Dictionary<string, ImpactSummaryTotalEntity> Items;
    }

    public class ImpactSummaryProjectResponse
    {
        public Dictionary<string, ImpactSummaryProjectEntity> Items;
    }
}