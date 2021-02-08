using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGameObject)), CanEditMultipleObjects]
public class TerrainEditor : Editor
{
    private TerrainGameObject _terrainTarget = null;
    private TerrainGeneratorSO _terrainData = null;

    private void OnEnable()
    {
        _terrainTarget = target as TerrainGameObject;
        _terrainData = serializedObject.FindProperty("_terrainData").objectReferenceValue as TerrainGeneratorSO;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        GUIStyle _labelStyle = new GUIStyle(EditorStyles.label)
        {
            fontStyle = FontStyle.BoldAndItalic,
            fontSize = 14
        };
        _labelStyle.normal.textColor = new Color(47f, 47f, 79f);

        GUILayout.Label("Terrain Properties from ScriptableObject:", _labelStyle);

        _terrainData.width = (int)EditorGUILayout.Slider("Width", _terrainData.width, 1f, 255.0f);
        _terrainData.height = (int)EditorGUILayout.Slider("Height", _terrainData.height, 1f, 255.0f);
        _terrainData.heightMap = EditorGUILayout.Slider("Height map", _terrainData.heightMap, 1f, 50.0f);
        _terrainData.scale = EditorGUILayout.Slider("Scale Noise Strength", _terrainData.scale, 0f, 255.0f);

        GUILayout.Space(10.0f);

        _terrainData.amplitude_1 = EditorGUILayout.Slider("Amplitude 1", _terrainData.amplitude_1, 1f, 50.0f);
        _terrainData.amplitude_2 = EditorGUILayout.Slider("Amplitude 2", _terrainData.amplitude_2, 1f, 50.0f);
        _terrainData.amplitude_3 = EditorGUILayout.Slider("Amplitude 3", _terrainData.amplitude_3, 1f, 50.0f);

        _terrainData.frenquency_1 = EditorGUILayout.Slider("Frequency 1", _terrainData.frenquency_1, 1f, 50.0f);
        _terrainData.frenquency_2 = EditorGUILayout.Slider("Frequency 2", _terrainData.frenquency_2, 1f, 50.0f);
        _terrainData.frenquency_3 = EditorGUILayout.Slider("Frequency 3", _terrainData.frenquency_3, 1f, 50.0f);

        if (GUILayout.Button("Generate Terrain"))
        {
            _terrainData.GenerateShape();
            _terrainTarget.GenerateMeshes();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
