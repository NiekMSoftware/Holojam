using UnityEngine;
using HoloJam.Characters.Player;
namespace HoloJam
{
    public class TriggerAnimationHitbox : MonoBehaviour
    {
        [SerializeField]
        private Animator animatorToAffect;
        [SerializeField]
        private string animationToPlay;
        [SerializeField]
        private bool resetAnim;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!enabled) return;
            if (collision.attachedRigidbody == null) return;
            if (collision.attachedRigidbody.GetComponent<Player>() == null) return;
            if (resetAnim)
            {
                animatorToAffect.Play(animationToPlay, 0, 0);
            } else
            {
                animatorToAffect.Play(animationToPlay);
            }
            
        }
        // Update is called once per frame
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.5f, 0.5f, 0, 0.4f);
            Gizmos.DrawCube(transform.position, transform.localScale);
        }
    }
}
