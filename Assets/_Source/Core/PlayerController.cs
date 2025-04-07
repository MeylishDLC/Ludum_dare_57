using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Rocks;
using RopeScript;
using UnityEngine;
using Zenject;

namespace Core
{
    public class PlayerController : MonoBehaviour
    {
        public event Action OnPlayerDeath;
        
        [SerializeField] private float swingForce = 5f;
        [SerializeField] private float delayBeforeFall = 0.2f;
        [SerializeField] private float delayBeforeDeath;

        private Collider2D _col;
        private AnchorController _anchorController;
        private CancellationToken _ctOnDestroy;
        private HingeJoint2D _joint;
        private Rigidbody2D _rb;
        private bool _canMove = true;

        [Inject]
        public void Initialize(AnchorController anchorController)
        {
            _anchorController = anchorController;
        }
        private void Start()
        {
            _anchorController.OnEndReached += DisableMovement;
            _ctOnDestroy = this.GetCancellationTokenOnDestroy();

            _col = GetComponent<Collider2D>();
            _rb = GetComponent<Rigidbody2D>();
            _joint = GetComponent<HingeJoint2D>();
            Rock.OnRockHitPlayer += Die;
        }
        private void OnDestroy()
        {
            _anchorController.OnEndReached -= DisableMovement;
            Rock.OnRockHitPlayer -= Die;
        }
        private void Update() 
        {
            if (!_canMove)
            {
                return;
            }
            HandleMovement();
        }
        private void DisableMovement() => _canMove = false;
        private void Die()
        {
            DieAsync(_ctOnDestroy).Forget();
        }
        private async UniTask DieAsync(CancellationToken token)
        {
            _col.enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(delayBeforeFall), cancellationToken: token);
            _joint.enabled = false;
            await UniTask.Delay(TimeSpan.FromSeconds(delayBeforeDeath), cancellationToken: token);
            OnPlayerDeath?.Invoke();
        }
        private void HandleMovement()
        {
            var input = Input.GetAxis("Horizontal");
            _rb.AddForce(new Vector2(input * swingForce, 0));
        }
    }
}
