using System;
using UnityEngine;
using UnityEngine.UI;

namespace QuickTimeEvents
{
    public class QTEManager : MonoBehaviour
    {
        [SerializeField] private int requiredClicks = 10;
        [SerializeField] private float timeLimit = 3f;

        [Header("UI")] 
        [SerializeField] private Slider progressSlider;
        [SerializeField] private GameObject sliderObject;
        
        private int _currentClicks;
        private float _timer;
        private bool _isActive;

        private Action _onSuccess;
        private void Update()
        {
            if (!_isActive)
            {
                return;
            }

            _timer -= Time.deltaTime;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _currentClicks++;
                UpdateUI();
            }

            if (_currentClicks >= requiredClicks)
            {
                EndQTE(true);
            }
            else if (_timer <= 0f)
            {
                RestartQTEAttempt();
            }
        }
        public void StartQTE(Action successCallback)
        {
            _onSuccess = successCallback;
            _isActive = true;

            sliderObject.SetActive(true);
            RestartQTEAttempt();
        }
        private void RestartQTEAttempt()
        {
            _currentClicks = 0;
            _timer = timeLimit;
            UpdateUI();
        }
        private void EndQTE(bool success)
        {
            _isActive = false;
            sliderObject.SetActive(false);

            if (success)
            {
                _onSuccess?.Invoke();
            }
        }

        private void UpdateUI()
        {
            if (progressSlider)
            {
                var progress = (float)_currentClicks / requiredClicks;
                progressSlider.value = Mathf.Clamp01(progress);
            }
        }
    }
}