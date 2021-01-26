using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TerrainGameObject))]
[CanEditMultipleObjects]
public class TerrainEditor : Editor
{
    private TerrainGameObject _terrainTarget = null;
    private void OnEnable()
    {
        _terrainTarget = (TerrainGameObject)target;
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Label("Terrain Properties from ScriptableObject:");
        _terrainTarget.TerrainSO.width = (int)EditorGUILayout.Slider("Width", _terrainTarget.TerrainSO.width, 1f, 50.0f);
        _terrainTarget.TerrainSO.height = (int)EditorGUILayout.Slider("Height", _terrainTarget.TerrainSO.height, 1f, 50.0f);
        _terrainTarget.TerrainSO.heightMap = (int)EditorGUILayout.Slider("Height Map", _terrainTarget.TerrainSO.heightMap, 0f, 50.0f);

        if (GUILayout.Button("Generate Terrain"))
        {
            _terrainTarget.ChangeHeightMap();
            _terrainTarget.GenerateMeshes();
        }
    }
}
