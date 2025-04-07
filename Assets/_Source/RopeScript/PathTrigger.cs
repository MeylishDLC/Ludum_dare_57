using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RopeScript
{
    public class PathTrigger : MonoBehaviour
    {
        [SerializeField] private List<Transform> newPath;

        private AnchorController _anchorController;
        private bool _triggered;
        [Inject]
        public void Initialize(AnchorController anchorController)
        {
            _anchorController = anchorController;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_triggered)
            {
                return;
            }
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (_anchorController != null)
                {
                    _anchorController.SetNewPath(newPath);
                    _triggered = true;
                }
            }
        }
    }
}