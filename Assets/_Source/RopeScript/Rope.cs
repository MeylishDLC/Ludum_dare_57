using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace RopeScript
{
    public class Rope: MonoBehaviour
    {
        [Header("Rope")]
        [SerializeField] private Transform objectToAttach;
        [SerializeField] private Transform startPoint;
        [SerializeField] private int numOfRopeSegments = 50;
        [SerializeField] private float ropeSegmentLength = 0.225f;
        [SerializeField] private float ropeWidth = 0.01f;
        
        [Header("Physics")]
        [SerializeField] private Vector2 gravityForce = new (0f, -2f);
        [SerializeField] private float dampingFactor = 0.98f;
        [SerializeField] private LayerMask collisionMask;
        [SerializeField] private float collisionRadius = 0.1f;
        [SerializeField] private float bounceFactor = 0.1f;
        [SerializeField] private float correctionClampAmount = 0.1f;
        
        [Header("Constraints")]
        [SerializeField] private int numOfConstraints = 50;

        [Header("Optimization")] 
        [SerializeField] private int collisionSegmentInterval = 2;
        
        private Vector2 _lastAttachedPosition;
        private LineRenderer _lineRenderer;
        private List<RopeSegment> _ropeSegments = new();
        private Vector3 _ropeStartPoint;
    
        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = numOfRopeSegments;
            _lineRenderer.startWidth = ropeWidth;
            _lineRenderer.endWidth = ropeWidth;
            _ropeStartPoint = startPoint.position;

            for (int i = 0; i < numOfRopeSegments; i++)
            {
                _ropeSegments.Add(new RopeSegment(_ropeStartPoint));
                _ropeStartPoint.y -= ropeSegmentLength;
            }
        }
        private void Update()
        {
            DrawRope();
        }

        private void FixedUpdate()
        {
            if (objectToAttach != null)
            {
                var attachedPos = (Vector2)objectToAttach.position;
                var lastSegment = _ropeSegments[^1];

                // Если объект сдвинулся — применим движение к последнему сегменту
                var delta = attachedPos - _lastAttachedPosition;
                if (delta.sqrMagnitude > 0.0001f)
                {
                    lastSegment.CurrentPosition = attachedPos;
                    lastSegment.OldPosition = attachedPos - delta;
                    _ropeSegments[^1] = lastSegment;
                }

                _lastAttachedPosition = attachedPos;
            }
            Simulate();
            for (int i = 0; i < numOfConstraints; i++)
            {
                try
                {
                    ApplyConstraints();
                    if (i % collisionSegmentInterval == 0)
                    {
                        HandleCollisions();
                        if (objectToAttach != null)
                        {
                            objectToAttach.position = _ropeSegments[^1].CurrentPosition;
                        }
                    }
                }
                catch (Exception e)
                {
                    // ignored
                }
            }
        }

        public struct RopeSegment
        {
            public Vector2 CurrentPosition; 
            public Vector2 OldPosition;

            public RopeSegment(Vector2 pos)
            {
                CurrentPosition = pos;
                OldPosition = pos;
            }
        }
        private void DrawRope()
        {
            var ropePos = new Vector3[numOfRopeSegments];
            for (int i = 0; i < _ropeSegments.Count; i++)
            {
                ropePos[i] = _ropeSegments[i].CurrentPosition;
            }
            _lineRenderer.SetPositions(ropePos);
        }

        private void Simulate()
        {
            for (int i = 0; i < _ropeSegments.Count; i++)
            {
                var segment = _ropeSegments[i];
                var velocity = (segment.CurrentPosition - segment.OldPosition) * dampingFactor;
                segment.OldPosition = segment.CurrentPosition;
                segment.CurrentPosition += velocity;
                segment.CurrentPosition += gravityForce * Time.fixedDeltaTime;
                _ropeSegments[i] = segment;
            }
        }

        private void ApplyConstraints()
        {
            var firstSegment = _ropeSegments[0];
            firstSegment.CurrentPosition = startPoint.position;
            _ropeSegments[0] = firstSegment;

            for (int i = 0; i < numOfRopeSegments; i++)
            {
                var currentSeg = _ropeSegments[i];
                var nextSeg = _ropeSegments[i + 1];
                
                var dist = (currentSeg.CurrentPosition - nextSeg.CurrentPosition).magnitude;
                var difference = dist - ropeSegmentLength;
                
                var changeDir = (currentSeg.CurrentPosition - nextSeg.CurrentPosition).normalized;
                var changeVector = changeDir * difference;


                if (i != 0)
                {
                    currentSeg.CurrentPosition -= (changeVector * 0.5f);
                    nextSeg.CurrentPosition += (changeVector * 0.5f);
                }
                else
                {
                    nextSeg.CurrentPosition += changeVector;
                }
                _ropeSegments[i] = currentSeg;
                _ropeSegments[i + 1] = nextSeg;
            }
        }
        private void HandleCollisions()
        {
            for (int i = 0; i < _ropeSegments.Count; i++)
            {
                var segment = _ropeSegments[i];
                var velocity = (segment.CurrentPosition - segment.OldPosition);
                var colliders = Physics2D.OverlapCircleAll(segment.CurrentPosition, collisionRadius, collisionMask);

                foreach (var col in colliders)
                {
                    var closestPoint = col.ClosestPoint(segment.CurrentPosition);
                    var distance = Vector2.Distance(segment.CurrentPosition, closestPoint);

                    if (distance < collisionRadius)
                    {
                        var normal = (segment.CurrentPosition - closestPoint).normalized;
                        if (normal == Vector2.zero)
                        {
                            normal = (segment.CurrentPosition - (Vector2)col.transform.position).normalized;
                        }
                        var depth = collisionRadius - distance;
                        segment.CurrentPosition += normal * depth;
                        
                        velocity = Vector2.Reflect(velocity, normal) * bounceFactor;
                    }
                }
                segment.OldPosition = segment.CurrentPosition - velocity;
                _ropeSegments[i] = segment;
            }
        }
    }
}