using Terrain;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainData", menuName = "Terrain/Terrain")]
public class TerrainGeneratorSO : ScriptableObject
{
    public int width = 1;
    public int height = 1;
    public float heightMap = 1.0f;
    public float scale = 2.0f;

    public float amplitude_1 = 0.0f;
    public float amplitude_2 = 0.0f;
    public float amplitude_3 = 0.0f;
    public float frenquency_1 = 0.0f;
    public float frenquency_2 = 0.0f;
    public float frenquency_3 = 0.0f;

    public Gradient gradientColor;

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
        _terrainGenerator.Scale = scale;

        _terrainGenerator.Amplitude_1 = amplitude_1;
        _terrainGenerator.Amplitude_2 = amplitude_2;
        _terrainGenerator.Amplitude_3 = amplitude_3;
        
        _terrainGenerator.Frequency_1 = frenquency_1;
        _terrainGenerator.Frequency_2 = frenquency_2;
        _terrainGenerator.Frequency_3 = frenquency_3;

        SetHeightMap();
        _terrainGenerator.GenerateVertices();
        _terrainGenerator.GenerateColors(ref gradientColor);
        _terrainGenerator.GenerateIndices();
    }



}
