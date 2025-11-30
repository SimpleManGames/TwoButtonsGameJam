namespace Game
{
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.InputSystem;

    public class RopeVerletIntegration : MonoBehaviour
    {
        public struct RopeSegment
        {
            public Vector2 currentPosition;
            
            public Vector2 oldPosition;

            public RopeSegment(Vector2 currentPosition)
            {
                this.currentPosition = currentPosition;
                oldPosition = currentPosition;
            }
        }

        [Header("Rope")]
        [SerializeField]
        private int numberOfSegments = 20;

        [SerializeField]
        private float segmentLength = 1f;

        [SerializeField]
        private Vector3 ropeEndPosition;

        [Header("Physics")]
        [SerializeField]
        private Vector2 gravityForce = new Vector2(0, 2f);

        [SerializeField]
        private float dampingFactor = 0.98f;

        [SerializeField]
        private LayerMask collisionMask = new LayerMask();
        
        [SerializeField]
        private float collisionRadius = 0.2f;
        
        [SerializeField]
        private float bounceFactor = 0.1f;

        [SerializeField]
        private float correctionClampAmount = 0.1f;
        
        [Header("Constraints")]
        [SerializeField]
        private int numberOfConstraintRuns = 50;

        [Header("Optimization")]
        [SerializeField]
        private int collisionSegmentInterval = 2;
        
        private LineRenderer _lineRenderer;
        
        private Vector3 _ropeStartPosition;
        
        private readonly List<RopeSegment> _segmentList = new List<RopeSegment>();

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _lineRenderer.positionCount = numberOfSegments;
            
            for (int i = 0; i < numberOfSegments; i++)
            {
                _segmentList.Add(new RopeSegment(_ropeStartPosition));
                _ropeStartPosition.y -= segmentLength;
            }
        }

        private void LateUpdate()
        {
            DrawRope();
        }

        private void FixedUpdate()
        {
            Simulate();

            for (int i = 0; i < numberOfConstraintRuns; i++)
            {
                ApplyConstraints();

                if (i % collisionSegmentInterval == 0)
                {
                    HandleCollisions();
                }
            }
        }

        private void HandleCollisions()
        {
            for (int i = 1; i < _segmentList.Count; i++)
            {
                RopeSegment segment = _segmentList[i];
                Vector2 velocity = segment.currentPosition - segment.oldPosition;
                Collider2D[] colliderArray = Physics2D.OverlapCircleAll(segment.currentPosition, collisionRadius, collisionMask);

                foreach (Collider2D otherCollider in colliderArray)
                {
                    Vector2 closestPoint = otherCollider.ClosestPoint(segment.currentPosition);
                    float distance = Vector2.Distance(segment.currentPosition, closestPoint);

                    if (distance < collisionRadius)
                    {
                        Vector2 normal = (segment.currentPosition - closestPoint).normalized;

                        if (normal == Vector2.zero)
                        {
                            normal = (segment.currentPosition - (Vector2)otherCollider.transform.position).normalized;
                        }
                        
                        float depth = collisionRadius - distance;
                        segment.currentPosition += normal * depth;
                        
                        velocity = Vector2.Reflect(velocity, normal) * bounceFactor;
                    }
                }
                
                segment.oldPosition = segment.currentPosition - velocity;
                _segmentList[i] = segment;
            }
        }

        private void Simulate()
        {
            for (int i = 0; i < _segmentList.Count; i++)
            {
                RopeSegment segment = _segmentList[i];
                Vector2 velocity = (segment.currentPosition - segment.oldPosition) * dampingFactor;
                
                segment.oldPosition = segment.currentPosition;
                segment.currentPosition += velocity;
                segment.currentPosition += gravityForce * Time.fixedDeltaTime;
                _segmentList[i] = segment;
            }
        }

        private void ApplyConstraints()
        {
            RopeSegment firstSegment = _segmentList[0];
            firstSegment.currentPosition = transform.position;
            _segmentList[0] = firstSegment;

            for (int i = 0; i < numberOfSegments - 1; i++)
            {
                RopeSegment currentSegment = _segmentList[i];
                RopeSegment nextSegment = _segmentList[i + 1];
                
                float distance = (currentSegment.currentPosition - nextSegment.currentPosition).magnitude;
                float difference = distance - segmentLength;
                
                Vector2 changeDirection = (currentSegment.currentPosition - nextSegment.currentPosition).normalized;
                Vector2 changeVector = changeDirection * difference;

                if (i != 0)
                {
                    currentSegment.currentPosition -= changeVector * 0.5f;
                    nextSegment.currentPosition += changeVector * 0.5f;
                }
                else
                {
                    nextSegment.currentPosition += changeVector;
                }
                
                _segmentList[i] = currentSegment;
                _segmentList[i + 1] = nextSegment;
            }
            
            RopeSegment lastSegment = _segmentList[^1];
            lastSegment.currentPosition = ropeEndPosition;
            _segmentList[^1] = lastSegment;
        }

        private void DrawRope()
        {
            Vector3[] positions = new Vector3[numberOfSegments];
            for (int i = 0; i < _segmentList.Count; i++)
            {
                positions[i] = _segmentList[i].currentPosition;
            }
            
            _lineRenderer.SetPositions(positions);
        }
    }
}
