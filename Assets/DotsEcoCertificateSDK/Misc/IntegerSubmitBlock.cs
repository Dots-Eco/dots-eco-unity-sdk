using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DotsEcoCertificateSDK.Misc
{
    public class IntegerSubmitBlock : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private Button _submit;
        [Space]
        public UnityEvent<int> Submitted;

        private void OnEnable()
        {
            OnInputChanged();
        }

        public void Submit()
        {
            if (CanSubmit(out var value) == false)
            {
                Debug.LogError("Integer input failed to submit", gameObject);
                return;
            }
            
            Submitted?.Invoke(value);
        }

        public void OnInputChanged()
        {
            _submit.interactable = CanSubmit();
        }

        private bool CanSubmit() => CanSubmit(out var _);
        
        private bool CanSubmit(out int value)
        {
            value = 0;
            if (_inputField == false)
                return false;

            if (string.IsNullOrEmpty(_inputField.text))
                return false;

            if (int.TryParse(_inputField.text, out value) == false)
                return false;
            
            return true;
        }
    }
}