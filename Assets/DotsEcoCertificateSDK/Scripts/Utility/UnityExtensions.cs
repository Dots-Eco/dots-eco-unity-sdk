using UnityEngine.UI;

namespace DotsEcoCertificateSDK.Scripts.Utility
{
    internal static class UnityExtensions
    {
        public static Image SetAlpha(this Image target, float alpha)
        {
            var c = target.color;
            c.a = alpha;
            target.color = c;
            return target;
        }
    }
}