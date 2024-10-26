using UnityEngine;
using System.Collections.Generic;
using SuperTiled2Unity;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using System.Linq;

namespace HoloJam
{
    public enum BlockType { LAYER_ONE_BLUE, LAYER_TWO_PURPLE }
    public class OnOffBlockManager : MonoBehaviour
    {
        public static OnOffBlockManager Instance { get; private set; }
        [SerializeField]
        private bool blockStatus = true;
        private bool blockStatus2 = true;
        private List<GameObject> defaultOnBlocks = new List<GameObject>();
        private List<GameObject> defaultOffBlocks = new List<GameObject>();
        private List<GameObject> defaultOn2Blocks = new List<GameObject>();
        private List<GameObject> defaultOff2Blocks = new List<GameObject>();
        private List<IReactToOnOffToggle> SpecialOnOffReactors = new List<IReactToOnOffToggle>();
        private List<IReactToOnOffToggle> SpecialOnOffReactors2 = new List<IReactToOnOffToggle>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            UpdateOnOffBlocks();
            SceneManager.sceneLoaded += Instance.OnNewSceneLoaded;
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            
        }
        void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            blockStatus = true;
            blockStatus2 = true;
            SpecialOnOffReactors.Clear();
            SpecialOnOffReactors2.Clear();
            UpdateOnOffBlocks();
        }
        private void UpdateOnOffBlocks()
        {
            defaultOnBlocks = new List<GameObject>(GameObject.FindGameObjectsWithTag("onoff-defaulton"));
            defaultOffBlocks = new List<GameObject>(GameObject.FindGameObjectsWithTag("onoff-defaultoff"));
            defaultOn2Blocks = new List<GameObject>(GameObject.FindGameObjectsWithTag("onoff2-defaulton"));
            defaultOff2Blocks = new List<GameObject>(GameObject.FindGameObjectsWithTag("onoff2-defaultoff"));
            SetBlocksStatus(blockStatus,BlockType.LAYER_ONE_BLUE);
            SetBlocksStatus(blockStatus2, BlockType.LAYER_TWO_PURPLE);
        }
        // Update is called once per frame
        void Update()
        {
        
        }
        public static void RegisterToggleObject(IReactToOnOffToggle toggleObj, BlockType bType)
        {
            if (bType == BlockType.LAYER_ONE_BLUE)
            {
                Instance.SpecialOnOffReactors.Add(toggleObj);
            } else
            {
                Instance.SpecialOnOffReactors2.Add(toggleObj);
            }
        }
        public static void SetBlocksStatus(bool on, BlockType blockType)
        {
            if (blockType == BlockType.LAYER_ONE_BLUE)
            {
                Instance.blockStatus = on;
                Instance.defaultOnBlocks.ForEach(obj => Instance.ToggleBlock(obj, on));
                Instance.defaultOffBlocks.ForEach(obj => Instance.ToggleBlock(obj, !on));
                Instance.SpecialOnOffReactors.ForEach(obj => obj.OnToggle(on));
            } else
            {
                Instance.blockStatus2 = on;
                Instance.defaultOn2Blocks.ForEach(obj => Instance.ToggleBlock(obj, on));
                Instance.defaultOff2Blocks.ForEach(obj => Instance.ToggleBlock(obj, !on));
                Instance.SpecialOnOffReactors2.ForEach(obj => obj.OnToggle(on));
            }
            
        }
        private void ToggleBlock(GameObject obj, bool on)
        {
            obj.GetComponent<Tilemap>().color = on ? new Color(1, 1, 1, 1) : new Color(0.25f, 0.25f, 0.25f, 0.4f);
            PolygonCollider2D childObj = obj.GetComponentInChildren<PolygonCollider2D>();
            if (childObj != null)
            {
                childObj.enabled = on;
            }
        }
        
    }
}
