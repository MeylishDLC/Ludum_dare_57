using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace RopeScript
{
    public class AnchorController : MonoBehaviour
    {
        public bool CanMoveDown { get; set; } = true;

        [SerializeField] private Transform ropeAnchor;
        [SerializeField] private List<Transform> pathPoints;
        [SerializeField] private float moveSpeed = 0.05f; // расстояние на шаг
        [SerializeField] private float holdDelay = 0.05f;

        private float _holdTimer;
        private int _currentSegment = 0;
        private float _segmentProgress = 0f;

        private void Update()
        {
            if (!ropeAnchor || pathPoints.Count < 2 || !CanMoveDown)
                return;

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                _holdTimer += Time.deltaTime;
                if (_holdTimer >= holdDelay)
                {
                    MoveAlongPath();
                    _holdTimer = 0f;
                }
            }
            else
            {
                _holdTimer = 0f;
            }
        }
        public void SetNewPath(List<Transform> newPath)
        {
            if (newPath == null || newPath.Count < 1)
                return;

            pathPoints = new List<Transform>(newPath);
    
            var tempStart = new GameObject("TempStartPoint");
            tempStart.transform.position = ropeAnchor.position;

            pathPoints.Insert(0, tempStart.transform);

            _currentSegment = 0;
            _segmentProgress = 0f;
        }
        private void MoveAlongPath()
        {
            if (_currentSegment >= pathPoints.Count - 1)
                return;

            Transform start = pathPoints[_currentSegment];
            Transform end = pathPoints[_currentSegment + 1];

            float segmentLength = Vector3.Distance(start.position, end.position);
            float step = moveSpeed / segmentLength;

            _segmentProgress += step;

            if (_segmentProgress >= 1f)
            {
                _segmentProgress = 0f;
                _currentSegment++;
                ropeAnchor.position = end.position;

                // Удаляем временную точку
                if (start.name == "TempStartPoint")
                {
                    Destroy(start.gameObject);
                }
            }
            else
            {
                ropeAnchor.position = Vector3.Lerp(start.position, end.position, _segmentProgress);
            }
        }
    }
}