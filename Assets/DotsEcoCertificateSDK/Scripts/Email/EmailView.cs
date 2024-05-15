using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DotsEcoCertificateSDK
{
    public class EmailView : MonoBehaviour
    {
        private const string REGEX_EMAIL = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

        [SerializeField] private EmailContext _context;
        
        [SerializeField] private Image _border;
        [SerializeField] private GameObject _warningEmail;
        [SerializeField] private TMP_InputField _inputEmail;
        [SerializeField] private Toggle _toggle;
        [SerializeField] private Button _btnSubscribe;

        [SerializeField] private GameObject _rootEmail;
        [SerializeField] private GameObject _emailSuccess;
        [SerializeField] private GameObject _emailFailure;

        public void Subscribe()
        {
            // TODO: backend request
            // _success?.Invoke();
            
            CertificateManagerBehaviour.Instance.SubscribeToEmailNotification(_context.CertificateId, _inputEmail.text, OnSubscribeResultListener);
        }

        private void OnSubscribeResultListener(bool isSubscribeSuccess)
        {
            _rootEmail.SetActive(false);
            if (_emailFailure)
                _emailFailure.SetActive(isSubscribeSuccess == false);
            if (_emailSuccess)
                _emailSuccess.SetActive(isSubscribeSuccess);            
        }

        public void OnInputChanged()
        {
            UpdateView();
        }

        public void ForceUpdateView() => UpdateView();

        private void OnEnable()
        {
            _inputEmail.SetTextWithoutNotify(string.Empty);
            _toggle.SetIsOnWithoutNotify(false);
            _border.color = Color.black;
        }

        private void UpdateView()
        {
            var isValidEmail = IsValidEmail();
            var canSubscribe = CanSubscribe();

            _border.color = isValidEmail ? Color.black : Color.red;
            if (_warningEmail)
                _warningEmail.SetActive(isValidEmail == false);

            _btnSubscribe.interactable = canSubscribe;
        }

        private bool CanSubscribe() => IsValidEmail() && _toggle.isOn;

        private bool IsValidEmail()
        {
            var validator = new Regex(REGEX_EMAIL);
            var d = validator.Match(_inputEmail.text);
            return validator.IsMatch(_inputEmail.text);
        }
    }
}