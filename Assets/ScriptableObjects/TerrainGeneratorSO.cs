using LandMassGeneration;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainData", menuName = "Terrain/Terrain")]
public class TerrainGeneratorSO : ScriptableObject
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private TerrainTypes[] _terrainTypeList;

    public LandMassGeneration.Noise.NoiseSettings noiseSettings;
    public Color32[] ColorsTerrainHeight { get; private set; }
    public TerrainMesh TerrainMesh { get; private set; }

    private LandMassGeneration.Noise _terrainNoise;
    public float heightMultiplier = 0f;

    private void OnEnable()
    {
        _terrainNoise = new LandMassGeneration.Noise();
    }


    public void Generate()
    {
        float[,] noiseTerrain = _terrainNoise.GenNoise(noiseSettings);
        TerrainMesh = new TerrainMesh(noiseSettings.width, noiseSettings.height);
        ColorTerrainTypes(noiseTerrain);

        TerrainMesh.GenTerrain(noiseTerrain, heightMultiplier, _animationCurve);

        //_terrainMesh.GenTerrain(noiseTerrain, heightMultiplier, _animationCurve);

        // Old
        //_terrainGenerator.Width = width;
        //_terrainGenerator.Height = height;
        //_terrainGenerator.Scale = scale;

        //_terrainGenerator.Amplitude_1 = amplitude_1;
        //_terrainGenerator.Amplitude_2 = amplitude_2;
        //_terrainGenerator.Amplitude_3 = amplitude_3;

        //_terrainGenerator.Frequency_1 = frenquency_1;
        //_terrainGenerator.Frequency_2 = frenquency_2;
        //_terrainGenerator.Frequency_3 = frenquency_3;

        //// Old
        //_terrainGenerator.GenerateVertices(noiseTerrain);
        ////_terrainGenerator.GenerateColors(ref gradientColor);
        //_terrainGenerator.GenerateIndices();
    }

    private void ColorTerrainTypes(float[,] noiseTerrain)
    {
        ColorsTerrainHeight = new Color32[noiseSettings.width * noiseSettings.height];
        for (int y = 0; y < noiseSettings.height; ++y)
        {
            for (int x = 0; x < noiseSettings.width; ++x)
            {
                float currentHeightNoise = noiseTerrain[x, y];
                foreach (TerrainTypes terrain in _terrainTypeList)
                {
                    if (currentHeightNoise <= terrain.Height)
                    {
                        ColorsTerrainHeight[(y * noiseSettings.width) + x] = terrain.Color;
                        break;
                    }
                }
            }
        }
    }

}
