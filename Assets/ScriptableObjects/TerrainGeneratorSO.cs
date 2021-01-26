using Terrain;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainData", menuName = "Terrain/Terrain")]
public class TerrainGeneratorSO : ScriptableObject
{
    [Range(1.0f, 100.0f)]
    public int width = 1;
    [Range(1.0f, 100.0f)]
    public int height = 1;
    [Range(1.0f,10.0f)]
    public float heightMap = 1.0f;

    private TerrainGenerator _terrainGenerator = null;
    private void OnEnable()
    {
        _terrainGenerator = new TerrainGenerator()
        {
            Width = width,
            Height = height
        };
        _terrainGenerator.GenerateVertices();
    }
    public TerrainGenerator.TerrainMesh GeTerrainMesh => _terrainGenerator.TerrainMeshes;

    public void SetHeightMap() => _terrainGenerator.HeightMap = heightMap;
    
    public void GenerateShape()
    {
        _terrainGenerator.Width = width;
        _terrainGenerator.Height = height;
        _terrainGenerator.GenerateVertices();
        _terrainGenerator.GenerateIndices();
    }

}
