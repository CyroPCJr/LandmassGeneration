using UnityEngine;

[CreateAssetMenu(fileName = "TerrainTypeData", menuName = "Terrain/Types")]
public class TerrainTypes : ScriptableObject
{
    public string Name;
    public float Height;
    public Color32 Color;
    public Texture2D Texuture;
}
