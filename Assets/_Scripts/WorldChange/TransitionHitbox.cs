using UnityEngine;
using HoloJam.Characters.Player;
namespace HoloJam
{
    public enum TransitionTag { AUTO, LEFT, RIGHT, UP, DOWN, LEFT_2, RIGHT_2, UP_2, DOWN_2, NONE}
    public class TransitionHitbox : MonoBehaviour
    {
        [SerializeField]
        private string sceneToLoad;
        [SerializeField]
        public TransitionTag myTag;
        [SerializeField]
        public TransitionTag targetTag;
        [SerializeField]
        private bool useDirectLocation;
        [SerializeField]
        private Vector3 positionInNewScene;
        [SerializeField]
        private bool useCustomOffset;
        [SerializeField]
        private Vector3 customOffset;
        public Vector3 SetPosition(Player p)
        {
            Vector3 startPos = transform.position;
            if (useCustomOffset)
            {
                startPos += customOffset;
            } else
            {
                Vector3 yOffset = new Vector3(0, 1.5f, 0);
                Vector3 xOffset = new Vector3(1.5f, 0, 0);
                switch (myTag)
                {
                    case TransitionTag.LEFT:
                        startPos += xOffset;
                        break;
                    case TransitionTag.RIGHT:
                        startPos -= xOffset;
                        break;
                    case TransitionTag.UP:
                        startPos -= yOffset;
                        break;
                    case TransitionTag.DOWN:
                        startPos += yOffset;
                        break;
                    case TransitionTag.LEFT_2:
                        startPos += xOffset;
                        break;
                    case TransitionTag.RIGHT_2:
                        startPos -= xOffset;
                        break;
                    case TransitionTag.UP_2:
                        startPos -= yOffset;
                        break;
                    case TransitionTag.DOWN_2:
                        startPos += yOffset;
                        break;
                    default:
                        break;
                }
            }
            
            p.transform.position = startPos;
            return startPos;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!enabled) return;
            if (collision.attachedRigidbody == null) return;
            if (collision.attachedRigidbody.GetComponent<Player>() == null) return;
            if (useDirectLocation)
            {
                WorldManager.LoadNewHubScene(sceneToLoad, positionInNewScene);
            }
            else
            {
                WorldManager.LoadNewHubScene(sceneToLoad, myTag, targetTag);
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.5f, 0.5f, 0, 0.4f);
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}
