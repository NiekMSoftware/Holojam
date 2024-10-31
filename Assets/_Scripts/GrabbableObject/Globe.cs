using UnityEngine;
using HoloJam.Managers;
namespace HoloJam
{
    public class Globe : MonoBehaviour
    {
        [SerializeField]
        private GameObject SpawnObj;
        private Grabbable mGrab;
        public string pillarSFX;
        private void Start()
        {
            mGrab = GetComponent<Grabbable>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {

            if (!mGrab.thrown) return;
            if (collision.isTrigger) return;
            if (collision.attachedRigidbody != null)
            {
                if (collision.GetComponent<Hitbox>() != null) return;
                if (collision.attachedRigidbody.GetComponent<Attackable>() != null) return;
            }
            
            float yDiff = collision.ClosestPoint(transform.position).y - transform.position.y;
            float xDiff = collision.ClosestPoint(transform.position).x - transform.position.x;
            
            if (Mathf.Abs(xDiff) > Mathf.Abs(yDiff))
            {
                GameObject newObj = Instantiate(SpawnObj, transform.position, Quaternion.identity);
                newObj.transform.localRotation = Quaternion.EulerAngles(0, 0, xDiff > 0 ? 90 : 270);
            } else
            {
                GameObject newObj = Instantiate(SpawnObj, transform.position, Quaternion.identity);
                newObj.transform.localRotation = Quaternion.EulerAngles(0, 0, yDiff > 0 ? 180 : 0);
            }
            AudioManager.Instance.Play(pillarSFX);
            Destroy(gameObject);
        }
    }
}
