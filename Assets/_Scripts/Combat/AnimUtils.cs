using UnityEngine;

namespace HoloJam
{
    public class AnimUtils : MonoBehaviour
    {
        public float normalizedAnimOffeset;
        public float animSpeed = 1;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            GetComponent<Animator>().Play("idle", 0, normalizedAnimOffeset);
            GetComponent<Animator>().speed = animSpeed;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
