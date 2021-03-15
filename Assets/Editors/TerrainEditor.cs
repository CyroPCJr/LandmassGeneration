using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGameObject)), CanEditMultipleObjects]
public class TerrainEditor : Editor
{
    private TerrainGameObject _terrainTarget = null;
    [HideInInspector, SerializeField] private TerrainGeneratorSO _terrainData = null;
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

        GUILayout.Label("Terrain ScriptableObject:", _labelStyle);

        _terrainData.noiseSettings.width = (int)EditorGUILayout.Slider("Width", _terrainData.noiseSettings.width, 1f, 255.0f);
        _terrainData.noiseSettings.height = (int)EditorGUILayout.Slider("Height", _terrainData.noiseSettings.height, 1f, 255.0f);

        _terrainData.heightMultiplier = EditorGUILayout.FloatField("Height multiplier", _terrainData.heightMultiplier);

        _terrainData.noiseSettings.noiseScale = EditorGUILayout.Slider("Noise scale", _terrainData.noiseSettings.noiseScale, 0.001f, 100.0f);

        _terrainData.noiseSettings.persistence = EditorGUILayout.Slider("Persistence", _terrainData.noiseSettings.persistence, 0.0f, 1.0f);
        _terrainData.noiseSettings.lacunarity = EditorGUILayout.FloatField("Lacunarity", _terrainData.noiseSettings.lacunarity);
        _terrainData.noiseSettings.octaves = (int)EditorGUILayout.Slider("Octaves", _terrainData.noiseSettings.octaves, 1.0f, 8.0f);
        _terrainData.noiseSettings.seed = EditorGUILayout.IntField("Seed", _terrainData.noiseSettings.seed);
        _terrainData.noiseSettings.offset = EditorGUILayout.Vector2Field("Offset", _terrainData.noiseSettings.offset);

        if (GUILayout.Button("Generate"))
        {
            _terrainTarget.UpdateMesh();
        }

        if (GUILayout.Button("Export Heightmap"))
        {
            //reference: https://docs.unity3d.com/Manual/terrain-Heightmaps.html
            _terrainData.ExportHeightMap();
        }

        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(_terrainData);
    }

}
