using UnityEngine;

namespace DotsEcoCertificateSDK.Scripts.Utility
{
    public class LoadingAnimation : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private void OnEnable()
        {
            _animator.SetTrigger("Show");
        }

        public void Play()
        {
            _animator.SetTrigger("Loading");
        }

        public void Stop()
        {
            _animator.SetTrigger("LoadingComplete");
        }
    }
}