using UnityEngine;

namespace HoloJam.Platform
{
    public class MovingPlatform : MonoBehaviour
    {
        public enum Direction
        {
            Horizontal,
            Vertical
        }

        [Tooltip("Decide upon which direction the platform should move, either Horizontal or Vertical.")]
        public Direction MovementDirection = Direction.Horizontal;

        [Tooltip("Insert anchor points here, these anchor points should include at least two indexes.")]
        public Transform[] AnchorPoints;

        [Space, Tooltip("Set the speed of the moving platform.")]
        public float Speed = 5f;

        private int _currentPointIndex = 0;
        private bool _movingForward = true;

        private void Start()
        {
            // initialize position to the first anchor point
            if (AnchorPoints.Length > 0)
            {
                transform.position = AnchorPoints[0].position;
            }
        }

        private void Update()
        {
            if (AnchorPoints.Length < 2) return;
            MovePlatform();
        }

        private void MovePlatform()
        {
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = AnchorPoints[_currentPointIndex].position;

            transform.position = Vector2.MoveTowards(currentPosition, targetPosition, Speed * Time.deltaTime);

            // Check if we reached target
            if (Vector2.Distance(currentPosition, targetPosition) < 0.1f)
            {
                if (_movingForward)
                {
                    _currentPointIndex++;
                    if (_currentPointIndex >= AnchorPoints.Length)
                    {
                        _movingForward = false;
                        _currentPointIndex = AnchorPoints.Length - 1;
                    }
                }
                else
                {
                    _currentPointIndex--;
                    if (_currentPointIndex < 0)
                    {
                        _movingForward = true;
                        _currentPointIndex = 1;
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.transform.parent.SetParent(transform);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            collision.transform.parent.SetParent(null);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (AnchorPoints.Length > 0)
            {
                Gizmos.color = Color.red;

                Vector2 directionVector = (MovementDirection == Direction.Horizontal) ? Vector2.right : Vector2.up;
                Vector2 arrowPosition = (Vector2)transform.position + directionVector * 0.5f;
                Gizmos.DrawLine(transform.position, arrowPosition);

                for (int i = 0; i < AnchorPoints.Length - 1; i++)
                {
                    Gizmos.DrawLine(AnchorPoints[i].position, AnchorPoints[i + 1].position);
                }
            }
        }
#endif
    }
}
