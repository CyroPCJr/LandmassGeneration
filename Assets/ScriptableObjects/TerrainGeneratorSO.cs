using LandMassGeneration;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainData", menuName = "Terrain/Terrain")]
public class TerrainGeneratorSO : ScriptableObject
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private TerrainTypes[] _terrainTypeList;

    public float heightMultiplier = 0f;
    private Noise _terrainNoise;

    public Noise.NoiseSettings noiseSettings;
    public Color32[] ColorsTerrainHeight { get; private set; }
    public TerrainMesh TerrainMesh { get; private set; }
    public Texture2D TerrainTexture { get; private set; }

    public TerrainTypes[] TerrainTypes => _terrainTypeList;

    public float MinHeight => heightMultiplier * _animationCurve.Evaluate(0);
    public float MaxHeight => heightMultiplier * _animationCurve.Evaluate(1);


    //public float MinHeight;
    //public float MaxHeight;

    private void OnEnable()
    {
        _terrainNoise = new Noise();
    }

    public void Generate()
    {
        float[,] noiseTerrain = _terrainNoise.GenNoise(noiseSettings);
        TerrainMesh = new TerrainMesh(noiseSettings.width, noiseSettings.height);
        TerrainMesh.GenTerrain(noiseTerrain, heightMultiplier, _animationCurve);
        ColorTerrainTypes(noiseTerrain);
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

        // create a new texture and set its pixel colors
        TerrainTexture = new Texture2D(noiseSettings.width, noiseSettings.height)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };
        TerrainTexture.SetPixels32(ColorsTerrainHeight);
        TerrainTexture.Apply();
    }

}
