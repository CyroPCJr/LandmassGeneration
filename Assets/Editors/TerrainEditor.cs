using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGameObject))]
public class TerrainEditor : Editor
{
    private TerrainGameObject _terrainTarget = null;
    private TerrainGeneratorSO _terrainData = null;

    private void OnEnable()
    {
        _terrainTarget = (TerrainGameObject)target;
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

        _terrainData.width = (int)EditorGUILayout.Slider("Width", _terrainData.width, 1f, 50.0f);
        _terrainData.height = (int)EditorGUILayout.Slider("Height", _terrainData.height, 1f, 50.0f);
        _terrainData.heightMap = (int)EditorGUILayout.Slider("Height Map", _terrainData.heightMap, 0f, 50.0f);

        if (GUILayout.Button("Generate Terrain"))
        {
            _terrainData.GenerateShape();;
            _terrainTarget.GenerateMeshes();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
