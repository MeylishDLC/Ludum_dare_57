using Core;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace UI
{
    public class FollowUI: MonoBehaviour
    {
        [SerializeField] private RectTransform followingObject;
        [SerializeField] private Transform followedObject;
        [SerializeField] private Canvas canvas;
        [SerializeField] private float smoothTime = 0.1f;

        private Camera _mainCamera;
        private Vector2 _velocity = Vector2.zero;

        [Inject]
        public void Construct(Camera mainCamera)
        {
            _mainCamera = mainCamera;
        }
        private void OnEnable()
        {
            MoveToFollowedObjectInstantly();
        }
        private void Update()
        {
            if (followedObject)
            {
                FollowObjectSmoothly();
            }
        }
        private void FollowObjectSmoothly()
        {
            var canvasPosition = GetCanvasPosition();
            followingObject.anchoredPosition = Vector2.SmoothDamp(followingObject.anchoredPosition, 
                canvasPosition, ref _velocity, smoothTime);
        }
        private void MoveToFollowedObjectInstantly()
        {
            followingObject.anchoredPosition = GetCanvasPosition();
        }
        private Vector2 GetCanvasPosition()
        {
            var playerScreenPosition = _mainCamera.WorldToScreenPoint(followedObject.position);

            var canvasRect = canvas.GetComponent<RectTransform>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, playerScreenPosition, _mainCamera, 
                out var canvasPosition);

            return canvasPosition;
        }
    }
}