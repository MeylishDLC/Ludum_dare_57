using System;
using UnityEngine;

namespace Rocks
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class Rock: MonoBehaviour
    {
        public static event Action OnRockHitPlayer;
        
        [field:SerializeField] public RockTypes RockType {get; private set;}
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (RockType == RockTypes.KillerRock)
                {
                    OnRockHitPlayer?.Invoke();
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
        }
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (RockType == RockTypes.KillerRock)
                {
                    OnRockHitPlayer?.Invoke();
                }
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                Destroy(gameObject);
            }
        }
    }
}