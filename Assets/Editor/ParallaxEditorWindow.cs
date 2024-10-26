using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using HoloJam.Parallax;

public class ParallaxEditorWindow : EditorWindow
{
    // parallax editor properties
    private ParallaxBackground parallaxBackground;
    private SerializedObject serializedBackground;
    private SerializedProperty layersProperty;

    private int previousLayerCount = 0;
    private int selectedBackgroundIndex = 0;
    private List<ParallaxBackground> availableBackgrounds = new List<ParallaxBackground>();
    private string[] backgroundNames;
    private string newBackgroundName = "Parallax Background"; 
    
    private Sprite selectedSprite;

    // styling
    private GUIStyle titleStyle;
    private GUIStyle customButtonStyle;

    private void InitializeStyles()
    {
        titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 16,
            alignment = TextAnchor.MiddleLeft,
            normal = { textColor = Color.white },
        };

        customButtonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 12,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.MiddleCenter,
            normal = { textColor = Color.white },
            hover = { textColor = Color.red },
            active = { textColor = Color.green },
            padding = new RectOffset(10, 10, 15, 15)
        };
    }

    [MenuItem("Window/Parallax Editor")]
    public static void ShowWindow()
    {
        GetWindow<ParallaxEditorWindow>("Parallax Editor");
    }

    private void OnEnable()
    {
        RefreshAvailableBackgrounds();
    }

    private void OnGUI()
    {
        InitializeStyles();

        EditorGUILayout.LabelField("Parallax Manager", titleStyle, GUILayout.Height(20));
        EditorGUILayout.Space(20);

        #region Creation of Backgrounds
        if (availableBackgrounds.Count == 0)
        {
            // Input field for custom background name
            newBackgroundName = EditorGUILayout.TextField("New Background Name", newBackgroundName);
            
            EditorGUILayout.Space(10);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Create Background", customButtonStyle, GUILayout.Width(200), GUILayout.Height(40)))
            {
                CreateBackground();
            }

            if (GUILayout.Button("Find Background", customButtonStyle, GUILayout.Width(200), GUILayout.Height(40)))
            {
                RefreshAvailableBackgrounds();
            }
            EditorGUILayout.EndHorizontal();
            return;
        }
        #endregion

        #region Adjusting Backgrounds
        // Dropdown for available backgrounds
        selectedBackgroundIndex = EditorGUILayout.Popup("Select Background", selectedBackgroundIndex, backgroundNames);

        if (GUILayout.Button("Load Selected Background"))
        {
            LoadSelectedBackground();
            RefreshAvailableBackgrounds();
        }

        newBackgroundName = EditorGUILayout.TextField("New Background Name", newBackgroundName);

        if (GUILayout.Button("Create New Parallax Background"))
        {
            CreateBackground();
        }

        if (parallaxBackground == null)
        {
            GUILayout.Label("No background selected.");
            return;
        }

        // Existing editor UI for layers
        serializedBackground.Update();
        previousLayerCount = layersProperty.arraySize;
        EditorGUILayout.PropertyField(layersProperty, new GUIContent("Layers"), true);

        if (layersProperty.arraySize < previousLayerCount)
        {
            RemoveDeletedLayer(previousLayerCount - 1);
        }

        selectedSprite = (Sprite)EditorGUILayout.ObjectField("Select Sprite", selectedSprite, typeof(Sprite), allowSceneObjects: false);
        if (GUILayout.Button("Add Layer"))
        {
            AddNewLayerWithSprite();
        }

        if (GUILayout.Button("Refresh Layers"))
        {
            parallaxBackground.SetLayers();
            RefreshAvailableBackgrounds();
        }

        serializedBackground.ApplyModifiedProperties();
        #endregion
    }

    #region Toolkit

    private void RefreshAvailableBackgrounds()
    {
        availableBackgrounds.Clear();
        availableBackgrounds.AddRange(Resources.FindObjectsOfTypeAll<ParallaxBackground>());
        backgroundNames = new string[availableBackgrounds.Count];
        for (int i = 0; i < availableBackgrounds.Count; i++)
        {
            backgroundNames[i] = availableBackgrounds[i].name;
        }
    }

    private void LoadSelectedBackground()
    {
        parallaxBackground = availableBackgrounds[selectedBackgroundIndex];
        serializedBackground = new SerializedObject(parallaxBackground);
        layersProperty = serializedBackground.FindProperty("layers");
        previousLayerCount = layersProperty.arraySize;
    }

    private void CreateBackground()
    {
        // Check if the name already exists
        foreach (var background in availableBackgrounds)
        {
            if (background.name == newBackgroundName)
            {
                EditorUtility.DisplayDialog("Name Already Exists",
                    "A parallax background with this name already exists. Please choose a different name.",
                    "OK");
                return;
            }
        }

        GameObject newBg = new GameObject(newBackgroundName);
        parallaxBackground = newBg.AddComponent<ParallaxBackground>();

        serializedBackground = new SerializedObject(parallaxBackground);
        layersProperty = serializedBackground.FindProperty("layers");
        previousLayerCount = layersProperty.arraySize;

        RefreshAvailableBackgrounds(); // Refresh after creating new background
    }

    private void AddNewLayerWithSprite()
    {
        // Check for duplicate layer names
        string layerName = "Layer - " + parallaxBackground.layers.Count;
        foreach (var layer in parallaxBackground.layers)
        {
            if (layer.name == layerName)
            {
                EditorUtility.DisplayDialog("Layer Name Already Exists",
                    "A layer with this name already exists. Please rename the layer.",
                    "OK");
                return;
            }
        }

        // Create new layer if name is unique
        GameObject newLayerObj = new GameObject(layerName);
        newLayerObj.transform.parent = parallaxBackground.transform;
        ParallaxLayer newLayer = newLayerObj.AddComponent<ParallaxLayer>();
        parallaxBackground.layers.Add(newLayer);
        SetLayerName(newLayer, parallaxBackground.layers.Count - 1);

        // Add child object with the selected sprite
        AddChildObjectWithSelectedSprite(newLayerObj);
    }

    private void AddChildObjectWithSelectedSprite(GameObject layerObj)
    {
        // Create a new child GameObject for the layer
        GameObject childObject = new GameObject("Sprite Child");
        childObject.transform.parent = layerObj.transform;

        // Add SpriteRenderer to the child
        SpriteRenderer spriteRenderer = childObject.AddComponent<SpriteRenderer>();

        // Assign the selected sprite if one is chosen
        if (selectedSprite != null)
        {
            spriteRenderer.sprite = selectedSprite;
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

    #endregion

    #region Styling

    #endregion
}
