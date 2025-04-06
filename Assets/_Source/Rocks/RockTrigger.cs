using System;
using UnityEngine;
using Zenject;

namespace Rocks
{
    [RequireComponent(typeof(Collider2D))]
    public class RockTrigger: MonoBehaviour
    {
        [SerializeField] private Transform rockFallPoint;
        [SerializeField] private float delayBeforeRockSpawn;
        private RockSpawnUtility _rockSpawnUtility;
        private bool _isTriggered;
        
        [Inject]
        public void Initialize(RockSpawnUtility rockSpawnUtility)
        {
            _rockSpawnUtility = rockSpawnUtility;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_isTriggered)
            {
                return;
            }
            TriggerRockFall();
        }

        private void TriggerRockFall()
        {
            _isTriggered = true;
            _rockSpawnUtility.SpawnRock(rockFallPoint.position);
        }
    }
}