using UnityEngine;

namespace HoloJam
{
    public class CorruptionNegationHitbox : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.attachedRigidbody == null) return;
            CorruptableObject co = collision.attachedRigidbody.GetComponent<CorruptableObject>();
            if (co != null)
            {
                co.NegationFieldsOverlapped++;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.attachedRigidbody == null) return;
            CorruptableObject co = collision.attachedRigidbody.GetComponent<CorruptableObject>();
            if (co != null)
            {
                co.NegationFieldsOverlapped --;
            }
        }
    }
}
