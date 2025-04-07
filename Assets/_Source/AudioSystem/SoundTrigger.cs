using System;
using FMODUnity;
using UnityEngine;
using Zenject;

namespace AudioSystem
{
    public class SoundTrigger: MonoBehaviour
    {
        [SerializeField] private EventReference soundEvent;
        private SoundManager _soundManager;
        private bool _triggered;

        [Inject]
        public void Initialize(SoundManager soundManager)
        {
            _soundManager = soundManager;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_triggered)
            {
                return;
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                _triggered = true;
                _soundManager.PlayOneShot(soundEvent);
            }
        }
    }
}