using UnityEngine;

namespace RopeScript
{
    public class AnchorController : MonoBehaviour
    {
        public Transform ropeAnchor;       // Объект якоря верёвки (например, платформа или крючок)
        public float moveStep = 0.1f;      // Насколько опустить за "тык"
        public float holdSpeed = 0.05f;    // Скорость опускания при удержании
        public float holdDelay = 0.05f;    // Частота опускания при удержании

        private float holdTimer = 0f;

        void Update()
        {
            if (ropeAnchor == null)
                return;

            Vector3 position = ropeAnchor.position;

            // Одноразовое нажатие вниз
            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                position.y -= moveStep;
                ropeAnchor.position = position;
            }

            // Удержание кнопки вниз
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                holdTimer += Time.deltaTime;
                if (holdTimer >= holdDelay)
                {
                    position.y -= holdSpeed;
                    ropeAnchor.position = position;
                    holdTimer = 0f;
                }
            }
            else
            {
                holdTimer = 0f;
            }
        }
    }
}