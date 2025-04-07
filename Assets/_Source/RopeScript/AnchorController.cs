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
        public event Action OnEndReached; 
        public bool CanMoveDown { get; set; } = true;

        [SerializeField] private Transform ropeAnchor;
        [SerializeField] private List<Transform> pathPoints;
        [SerializeField] private float moveSpeed = 0.05f;
        [SerializeField] private float holdDelay = 0.05f;

        private bool _endReached;
        private float _holdTimer;
        private int _currentSegment;
        private float _segmentProgress;
        private bool _newPathSet;

        private void Start()
        {
            OnEndReached += DisableMovement;
        }

        private void OnDestroy()
        {
            OnEndReached -= DisableMovement;
        }
        private void Update()
        {
            if (!ropeAnchor || pathPoints.Count < 2 || !CanMoveDown)
            {
                return;
            }

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
            _newPathSet = true;
        }
        private void DisableMovement() => CanMoveDown = false;
        private void MoveAlongPath()
        {
            if (_currentSegment >= pathPoints.Count - 1)
            {
                if (!_endReached)
                {
                    if (_newPathSet)
                    {
                        OnEndReached?.Invoke();
                        _endReached = true;
                    }
                }
                return;
            }

            var start = pathPoints[_currentSegment];
            var end = pathPoints[_currentSegment + 1];

            var segmentLength = Vector3.Distance(start.position, end.position);
            var step = moveSpeed / segmentLength;

            _segmentProgress += step;

            if (_segmentProgress >= 1f)
            {
                _segmentProgress = 0f;
                _currentSegment++;
                ropeAnchor.position = end.position;

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