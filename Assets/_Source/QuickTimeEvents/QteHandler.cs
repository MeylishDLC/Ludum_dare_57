using System;
using UnityEngine;
using UnityEngine.UI;

namespace QuickTimeEvents
{
    public class QteHandler : MonoBehaviour
    {
        public bool EventIsActive { get; private set; }
        
        [SerializeField] private int requiredClicks = 10;
        [SerializeField] private float timeLimit = 3f;
        [SerializeField] private KeyCode clickKey;
        
        [Header("UI")] 
        [SerializeField] private GameObject uiIcon;
        
        private int _currentClicks;
        private float _timer;
        private Action _onSuccess;
        private void Update()
        {
            if (!EventIsActive)
            {
                return;
            }

            _timer -= Time.deltaTime;

            if (Input.GetKeyDown(clickKey))
            {
                _currentClicks++;
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
            EventIsActive = true;

            uiIcon.SetActive(true);
            RestartQTEAttempt();
        }
        private void RestartQTEAttempt()
        {
            _currentClicks = 0;
            _timer = timeLimit;
        }
        private void EndQTE(bool success)
        {
            EventIsActive = false;
            uiIcon.SetActive(false);

            if (success)
            {
                _onSuccess?.Invoke();
            }
        }
    }
}