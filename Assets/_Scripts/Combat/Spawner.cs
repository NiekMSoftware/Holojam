using UnityEngine;

namespace HoloJam
{
    public class Spawner : MonoBehaviour
    {
        public GameObject spawnObjPrefab;
        public bool spawnNow;
        private bool lastSpawnNow;
        [SerializeField]
        private Attackable parentAttackable;

        // Update is called once per frame
        void Update()
        {
            if (spawnNow != lastSpawnNow && spawnNow)
            {
                GameObject newInstance = Instantiate(spawnObjPrefab, transform.position, Quaternion.identity);
                if (parentAttackable != null)
                {
                    Hitbox hb = newInstance.GetComponent<Hitbox>();
                    if (hb != null)
                    {
                        hb.SetParentAttackable(parentAttackable);
                    }
                    Projectile p = newInstance.GetComponent<Projectile>();
                    if (p != null && parentAttackable.transform.localScale.x < 0)
                    {
                        p.FlipVelocity();
                    }
                }
            }
            lastSpawnNow = spawnNow;
        }
    }
}
