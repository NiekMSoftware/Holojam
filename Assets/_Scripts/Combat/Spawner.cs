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
            if (spawnNow != lastSpawnNow)
            {
                GameObject newInstance = Instantiate(spawnObjPrefab, transform.position, Quaternion.identity);
                if (parentAttackable != null)
                {
                    Hitbox hb = newInstance.GetComponent<Hitbox>();
                    if (hb != null)
                    {
                        hb.SetParentAttackable(parentAttackable);
                    }
                }
                lastSpawnNow = spawnNow;
            }
        }
    }
}
