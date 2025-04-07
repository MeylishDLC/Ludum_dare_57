using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.Serialization;
using STOP_MODE = FMOD.Studio.STOP_MODE;

namespace AudioSystem
{
    public class SoundManager: MonoBehaviour
    {
        [BankRef]
        public List<string> Banks;
        
        [Header("Volume")] 
        [Range(0, 1)] public float MasterVolume = 1;
        [Range(0, 1)] public float MusicVolume = 1;
        [Range(0, 1)] public float SFXVolume = 1;
        [field:SerializeField] public FMODEvents FMODEvents { get; private set; }
        public Bus MasterBus {get; private set;}
        public Bus MusicBus {get; private set;}
        public Bus FfxBus {get; private set;}

        private readonly List<EventInstance> _eventInstances = new();

        private EventInstance _musicEventInstance;
        private void Awake()
        {
            LoadBanks();
        }
        private void Start()
        {
            MasterBus = RuntimeManager.GetBus("bus:/");
            MusicBus = RuntimeManager.GetBus("bus:/Music");
            FfxBus = RuntimeManager.GetBus("bus:/SFX");
        }
        private void Update()
        {
            MasterBus.setVolume(MasterVolume);
            MusicBus.setVolume(MusicVolume);
            FfxBus.setVolume(SFXVolume);
        }
        public void PlayOneShot(EventReference sound)
        {
            RuntimeManager.PlayOneShot(sound);
        }
        private EventInstance CreateInstance(EventReference eventReference)
        {
            var eventInstance = RuntimeManager.CreateInstance(eventReference);
            _eventInstances.Add(eventInstance);
            return eventInstance;
        }

        public void PlayMusicDuringTime(float time, EventReference music)
        {
            var instance = CreateInstance(music);
    
            var wrapper = new EventInstanceWrapper(instance);
    
            wrapper.Instance.start();

            StopMusicAfterTime(wrapper, time).Forget();
        }

        private async UniTask StopMusicAfterTime(EventInstanceWrapper wrapper, float time)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(time));
    
            wrapper.Instance.stop(STOP_MODE.ALLOWFADEOUT);
    
            wrapper.Instance.release();
    
            _eventInstances.Remove(wrapper.Instance);
        }
        public void InitializeMusic(EventReference musicEventReference)
        {
            _musicEventInstance = CreateInstance(musicEventReference);
            _musicEventInstance.start();
        }
        public void CleanUp()
        {
            foreach (var eventInstance in _eventInstances)
            {
                eventInstance.stop(STOP_MODE.IMMEDIATE);
                eventInstance.release();
            }
        }
        private void OnDestroy()
        {
            CleanUp();
        }
        private void LoadBanks()
        {
            foreach (var b in Banks)
            {
                RuntimeManager.LoadBank(b, true);
                Debug.Log("Loaded bank " + b);
            }

            RuntimeManager.CoreSystem.mixerSuspend();
            RuntimeManager.CoreSystem.mixerResume();
        }
    }
    public class EventInstanceWrapper
    {
        public EventInstance Instance { get; }

        public EventInstanceWrapper(EventInstance instance)
        {
            Instance = instance;
        }
    }
}