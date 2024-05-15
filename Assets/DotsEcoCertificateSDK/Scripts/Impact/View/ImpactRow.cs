using UnityEngine;

namespace DotsEcoCertificateSDK.Impact
{
    public class ImpactRow : MonoBehaviour
    {
        [SerializeField] private ImpactView prefab;
        [SerializeField] private Transform container;

        public void Setup(ImpactSummaryTotalResponse totalResponse)
        {
            Clear();
            foreach (var item in totalResponse.Items)
            {
                var view = Instantiate(prefab, container);
                view.Setup(item.Value);
            }
        }

        public void Setup(ImpactSummaryProjectResponse projectResponse)
        {
            Clear();
            foreach (var item in projectResponse.Items)
            {
                var view = Instantiate(prefab, container);
                view.Setup(item.Value);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < container.transform.childCount; i++)
            {
                Destroy(container.transform.GetChild(i).gameObject);
            }
        }
    }
}