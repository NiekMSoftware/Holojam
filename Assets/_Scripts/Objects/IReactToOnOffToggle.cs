using UnityEngine;

namespace HoloJam
{
    public class IReactToOnOffToggle : MonoBehaviour
    {
        public BlockType mBlockType;
        public virtual void OnToggle(bool toggleValue) { }
    }
}
