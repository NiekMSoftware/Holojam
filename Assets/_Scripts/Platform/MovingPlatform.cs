using HoloJam.Characters.Player;
using HoloJam.Characters.Player.Utils;
using NUnit.Framework.Constraints;
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

        private PlatformEffector2D _effector;
        private PlayerInput input;

        private void Start()
        {
            _effector = GetComponent<PlatformEffector2D>();

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
            CheckTargetReached();
        }

        private void MovePlatform()
        {
            Vector2 currentPosition = transform.position;
            Vector2 targetPosition = AnchorPoints[_currentPointIndex].position;

            transform.position = Vector2.MoveTowards(currentPosition, targetPosition, Speed * Time.deltaTime);
        }


        private void CheckTargetReached()
        {
            if (Vector2.Distance(transform.position, AnchorPoints[_currentPointIndex].position) < 0.1f)
            {
                UpdateTargetPoint();
            }
        }

        private void UpdateTargetPoint()
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            collision.transform.parent.SetParent(transform);
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            input = collision.GetComponentInParent<PlayerInput>();
            Collider2D playerCol = collision.GetComponent<Collider2D>();
            Player p = playerCol.GetComponentInParent<Player>();
            
            // Check if player is grounded (aka on the platform) and is pressing down
            if (p.SurroundingSensor.Grounded && input.GetUpDownInput() < 0)
            {
                _effector.rotationalOffset = 180;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision != null)
                collision.transform.parent.SetParent(null);

            input = null;
            _effector.rotationalOffset = 0;
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
