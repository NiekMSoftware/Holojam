using HoloJam.Platform;
using UnityEditor;
using UnityEngine;

namespace HoloJam.Tools
{
    [CustomEditor(typeof(MovingPlatform))]
    public class MovingPlatformEditor : Editor
    {
        // A variable to store the adjustable distance
        private float adjustableDistance = 2f;

        public override void OnInspectorGUI()
        {
            EditorGUILayout.LabelField("Platform Settings", EditorStyles.boldLabel);
            DrawDefaultInspector();

            EditorGUILayout.Space();

            // Add a slider to control the distance of the second anchor point
            EditorGUILayout.LabelField("Anchor Points Settings", EditorStyles.boldLabel);
            adjustableDistance = EditorGUILayout.Slider("Anchor Distance", adjustableDistance, -50f, 50f);

            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Generate Anchor Points"))
            {
                GenerateAnchorPoints((MovingPlatform)target, adjustableDistance);
            }

            if (GUILayout.Button("Update Anchors"))
            {
                UpdateAnchors((MovingPlatform)target, adjustableDistance);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void GenerateAnchorPoints(MovingPlatform platform, float distance)
        {
            // Get the parent Empty GameObject
            Transform parentGO = platform.transform.parent;
            if (parentGO == null)
            {
                Debug.LogError("Platform does not have a parent object. Cannot generate anchor points.");
                return;
            }

            // Create new anchor points
            platform.AnchorPoints = new Transform[2];
            platform.AnchorPoints[0] = CreateAnchorPoint(parentGO, platform.transform.position, Vector2.zero);  // First anchor at the platform position

            Vector2 direction = (platform.MovementDirection == MovingPlatform.Direction.Horizontal) ? Vector2.right : Vector2.up;
            platform.AnchorPoints[1] = CreateAnchorPoint(parentGO, platform.transform.position, direction * distance);  // Second anchor adjustable by slider
        }

        private void UpdateAnchors(MovingPlatform platform, float distance)
        {
            // Clear existing anchor points
            if (platform.AnchorPoints != null)
            {
                foreach (Transform anchor in platform.AnchorPoints)
                {
                    if (anchor != null)
                    {
                        DestroyImmediate(anchor.gameObject);  // Destroy the anchor GameObjects in the scene
                    }
                }
            }

            // Create new anchors
            GenerateAnchorPoints(platform, distance);
        }

        private Transform CreateAnchorPoint(Transform parentGO, Vector2 basePosition, Vector2 offset)
        {
            // Create a new GameObject as an anchor point
            GameObject anchor = new GameObject("Anchor Point");
            anchor.transform.position = basePosition + offset;

            // Set the parent to the Empty GO
            anchor.transform.parent = parentGO;

            // Assign a gizmo icon (you can change the icon type to suit your needs)
            GUIContent iconContent = EditorGUIUtility.IconContent("sv_label_0");
            if (iconContent != null)
            {
                var gizmoIcon = iconContent.image as Texture2D;
                if (gizmoIcon != null)
                {
                    EditorGUIUtility.SetIconForObject(anchor, gizmoIcon);
                }
            }

            return anchor.transform;
        }
    }
}