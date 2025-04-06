using UnityEngine;

namespace RopeScript
{
    public class AnchorController : MonoBehaviour
    {
        [SerializeField] private Transform ropeAnchor;
        [SerializeField] private float moveStep = 0.1f;
        [SerializeField] private float holdSpeed = 0.05f;
        [SerializeField] private float holdDelay = 0.05f;

        private float _holdTimer;

        private void Update()
        {
            if (!ropeAnchor)
            {
                return;
            }

            var position = ropeAnchor.position;

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                position.y -= moveStep;
                ropeAnchor.position = position;
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                _holdTimer += Time.deltaTime;
                if (_holdTimer >= holdDelay)
                {
                    position.y -= holdSpeed;
                    ropeAnchor.position = position;
                    _holdTimer = 0f;
                }
            }
            else
            {
                _holdTimer = 0f;
            }
        }
    }
}