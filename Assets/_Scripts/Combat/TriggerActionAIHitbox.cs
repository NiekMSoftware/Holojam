using UnityEngine;
using HoloJam.Characters;
using HoloJam.Characters.Player;

namespace HoloJam
{
    public class TriggerActionAIHitbox : MonoBehaviour
    {
        [SerializeField]
        private Core originEnemy;
        [SerializeField]
        private string actionToPerform = "attack";
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!enabled) return;
            if (collision.attachedRigidbody == null) return;
            if (collision.attachedRigidbody.GetComponent<Player>() == null) return;
            originEnemy.PerformAction(actionToPerform);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.5f, 0.0f, 0.2f, 0.4f);
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}
