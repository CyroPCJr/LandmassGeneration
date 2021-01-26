using Terrain;
using UnityEditor;

[CustomEditor(typeof(TerrainGameObject))]
public class TerrainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Width"));
        serializedObject.ApplyModifiedProperties();


      
    }
}
