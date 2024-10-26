using HoloJam.Parallax;
using UnityEditor;
using UnityEngine;

public class ParallaxEditorWindow : EditorWindow
{
    private ParallaxBackground parallaxBackground;
    private SerializedObject serializedBackground;
    private SerializedProperty layersProperty;
    private int previousLayerCount = 0;

    [MenuItem("Window/Parallax Editor")]
    public static void ShowWindow()
    {
        GetWindow<ParallaxEditorWindow>("Parallax Editor");
    }

    private void OnEnable()
    {
        RefreshBackground();
    }

    private void OnGUI()
    {
        GUILayout.Label("Parallax Layer Manager", EditorStyles.boldLabel);

        if (parallaxBackground == null)
        {
            if (GUILayout.Button("Find Parallax Background"))
            {
                RefreshBackground();
            }
            return;
        }

        serializedBackground.Update();

        // Track the current layer count before displaying the property
        previousLayerCount = layersProperty.arraySize;

        // Display layers array
        EditorGUILayout.PropertyField(layersProperty, new GUIContent("Layers"), true);

        // Detect if the layer count has decreased
        if (layersProperty.arraySize < previousLayerCount)
        {
            RemoveDeletedLayer(previousLayerCount - 1);
        }

        if (GUILayout.Button("Add Layer"))
        {
            GameObject newLayerObj = new GameObject("Parallax Layer");
            newLayerObj.transform.parent = parallaxBackground.transform;
            ParallaxLayer newLayer = newLayerObj.AddComponent<ParallaxLayer>();
            parallaxBackground.layers.Add(newLayer);
            SetLayerName(newLayer, parallaxBackground.layers.Count - 1);
        }

        if (GUILayout.Button("Refresh Layers"))
        {
            parallaxBackground.SetLayers();
        }

        serializedBackground.ApplyModifiedProperties();
    }

    private void RefreshBackground()
    {
        parallaxBackground = FindFirstObjectByType<ParallaxBackground>();
        if (parallaxBackground != null)
        {
            serializedBackground = new SerializedObject(parallaxBackground);
            layersProperty = serializedBackground.FindProperty("layers");
            previousLayerCount = layersProperty.arraySize;
        }
        else
        {
            Debug.LogWarning("No ParallaxBackground found in the scene.");
        }
    }

    private void RemoveDeletedLayer(int removedIndex)
    {
        if (removedIndex >= 0 && removedIndex < parallaxBackground.transform.childCount)
        {
            DestroyImmediate(parallaxBackground.transform.GetChild(removedIndex).gameObject);
            parallaxBackground.layers.RemoveAt(removedIndex);
        }
    }

    private void SetLayerName(ParallaxLayer layer, int index)
    {
        layer.name = "Layer - " + index;
    }
}
