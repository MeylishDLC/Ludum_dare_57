using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Rocks;
using UnityEngine;

namespace Core
{
    public class PlayerController : MonoBehaviour
    {
        public event Action OnPlayerDeath;
        
        [SerializeField] private float swingForce = 5f;
        [SerializeField] private float delayBeforeFall = 0.2f;
        [SerializeField] private float delayBeforeDeath;

        private CancellationToken _ctOnDestroy;
        private HingeJoint2D _joint;
        private Rigidbody2D _rb;
        private void Start()
        {
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();
            _rb = GetComponent<Rigidbody2D>();
            _joint = GetComponent<HingeJoint2D>();
            Rock.OnRockHitPlayer += Die;
        }
        private void OnDestroy()
        {
            Rock.OnRockHitPlayer -= Die;
        }

        private void Update() 
        {
            HandleMovement();
        }
        private void Die()
        {
            DieAsync(_ctOnDestroy).Forget();
        }
        private async UniTask DieAsync(CancellationToken token)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delayBeforeFall), cancellationToken: token);
            _joint.enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(delayBeforeDeath), cancellationToken: token);
            OnPlayerDeath?.Invoke();
            Debug.Log("DEATH");
            //todo show death screen
        }
        private void HandleMovement()
        {
            var input = Input.GetAxis("Horizontal");
            _rb.AddForce(new Vector2(input * swingForce, 0));
        }
    }
}
