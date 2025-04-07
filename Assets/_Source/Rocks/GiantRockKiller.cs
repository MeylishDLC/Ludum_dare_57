using System;
using UnityEngine;

namespace Rocks
{
    public class GiantRockKiller: MonoBehaviour
    {
        public event Action OnFall;

        private bool _isFallen;
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_isFallen)
            {
                return;
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                OnFall?.Invoke();
                _isFallen = true;
            }
        }
    }
}