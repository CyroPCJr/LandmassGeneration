using LandMassGeneration;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainData", menuName = "Terrain/Terrain")]
public class TerrainGeneratorSO : ScriptableObject
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private TerrainTypes[] _terrainTypeList;
    [SerializeField] private ObjectsPlacementSO _objectsPlacementSO;

    public float heightMultiplier = 0f;
    public bool RandomTrees = false;
    private Noise _terrainNoise;

    public Noise.NoiseSettings noiseSettings;
    public Color32[] ColorsTerrainHeight { get; private set; }
    public TerrainMesh TerrainMesh { get; private set; }
    public Texture2D TerrainTexture { get; private set; }

    public ObjectsPlacementSO TreesPlacement => _objectsPlacementSO;

    public TerrainTypes[] TerrainTypes => _terrainTypeList;

    public float MinHeight => heightMultiplier * _animationCurve.Evaluate(0);

    public float MaxHeight => heightMultiplier * _animationCurve.Evaluate(1);

    public GameObject treePrefabs;

    public List<Vector3> PrefabsPosition;

    private void OnEnable()
    {
        _terrainNoise = new Noise();
        PrefabsPosition = new List<Vector3>();
    }

    public void Generate()
    {
        float[,] noiseTerrain = _terrainNoise.GenNoise(noiseSettings);
        TerrainMesh = new TerrainMesh(noiseSettings.width, noiseSettings.height);
        TerrainMesh.GenTerrain(noiseTerrain, heightMultiplier, _animationCurve);
        ColorTerrainTypes(noiseTerrain);
        //Next feature
        //BuildingTrees();
    }

    public void BuildingTrees()
    {
        if (TerrainMesh != null && TerrainMesh.vertices.Length > 0)
        {
            //BuildTreesTerrain(TerrainMesh.vertices, RandomTrees);
        }
        else
        {
            Debug.LogWarning("<color=red>Terrain</color> doesnt load the Mesh Renderer.");
        }
    }

    public void ExportHeightMap()
    {
        var dirPath = Application.dataPath + "/RenderOutput";
        if (!System.IO.Directory.Exists(dirPath))
        {
            System.IO.Directory.CreateDirectory(dirPath);
        }

        byte[] bytesRaw = TerrainTexture.GetRawTextureData();
        byte[] bytesPNG = TerrainTexture.EncodeToPNG();

        byte[] bytes = new byte[noiseSettings.height * noiseSettings.width];
        for (int y = 0; y < noiseSettings.height; ++y)
        {
            for (int x = 0; x < noiseSettings.width; ++x)
            {
                int srcRow = (noiseSettings.height - y - 1);
                int srcIndex = (x + (srcRow * noiseSettings.width)) * 4;
                int dstRow = y;
                int dstIndex = (x + (dstRow * noiseSettings.width));
                bytes[dstIndex] = bytesRaw[srcIndex];
            }
        }

        int rangeName = Random.Range(0, 100000);
        System.IO.File.WriteAllBytes(dirPath + "/R_" + rangeName + ".raw", bytes);
        System.IO.File.WriteAllBytes(dirPath + "/R_" + rangeName + ".png", bytesPNG);
        Debug.Log($"height: {noiseSettings.height}, width: {noiseSettings.width}");
        Debug.Log($"{bytesRaw.Length / 1024} Kb was saved as: {dirPath}");

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    private void ColorTerrainTypes(float[,] noiseTerrain)
    {
        ColorsTerrainHeight = new Color32[noiseSettings.width * noiseSettings.height];
        // funciona
        for (int y = 0; y < noiseSettings.height; ++y)
        {
            for (int x = 0; x < noiseSettings.width; ++x)
            {
                // colored terrain
                //foreach (TerrainTypes terrain in _terrainTypeList)
                //{
                //    if (currentHeightNoise <= terrain.Height)
                //    {
                //        ColorsTerrainHeight[(y * noiseSettings.width) + x] = terrain.Color;
                //        break;
                //    }
                //}
                // grayscale
                float currentHeightNoise = noiseTerrain[x, y];
                ColorsTerrainHeight[x + (y * noiseSettings.width)] = Color32.Lerp(Color.black, Color.white, currentHeightNoise);
            }
        }


        // create a new texture and set its pixel colors
        TerrainTexture = new Texture2D(noiseSettings.width, noiseSettings.height, TextureFormat.RGBA32, false)
        {
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Clamp
        };
        TerrainTexture.SetPixels32(ColorsTerrainHeight);
        TerrainTexture.Apply();
    }


    private void BuildTreesTerrain(Vector3[] vertices, bool random)
    {
        PrefabsPosition.Clear();
        float maxH = MaxHeight * MaxHeight;

        //First test
        //foreach (var vert in vertices)
        //{
        //    if ((vert.y >= _terrainTypeList[1].Height * maxH) &&
        //        (vert.y <= _terrainTypeList[2].Height * maxH) &&
        //        Random.Range(0, 100) < 100)
        //    {
        //        //esse funciona
        //        var position = new Vector3(vert.x * 10f + Random.Range(-1f, 5f), vert.y + 0.5f, vert.z * 10f);
        //        //_ =Instantiate(treePrefabs, position, Quaternion.identity);

        //        PrefabsPosition.Add(position);
        //    }
        //}

        //Other test
        // _objectsPlacementSO.RecyclePool();
        for (int i = 0; i < vertices.Length; ++i)
        {
            Vector3 vert = vertices[i];
            if ((vert.y >= _terrainTypeList[2].Height * maxH) &&
                (vert.y <= _terrainTypeList[3].Height * maxH - 1.5f))
            {
                for (int j = 0; j < _objectsPlacementSO.ElementsList.Length; ++j)
                {
                    //var elements = _objectsPlacementSO.ElementsList[j];
                    //if (elements.CanPlace)
                    //{
                    //    // Add random elements to element placement.
                    //    Vector3 position = new Vector3(vert.x * 10f, vert.y, vert.z * 10f);
                    //    Vector3 offset = new Vector3(Random.Range(-0.75f, 0.75f), 0f, Random.Range(-0.75f, 0.75f));
                    //    Quaternion rotation = new Quaternion(Random.Range(0, 5f), Random.Range(0, 360f), Random.Range(0, 5f), 1f);
                    //    Vector3 scale = Vector3.one * Random.Range(3f, 7f);

                    //    if (!random)
                    //    {
                    //        GameObject obj = _objectsPlacementSO.GetObject("Fir_v1_2", position + offset, rotation);
                    //        if (obj)
                    //        {
                    //            obj.transform.localScale = scale;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        GameObject obj = _objectsPlacementSO.GetRandomObject(position + offset, rotation);
                    //        if (obj)
                    //        {
                    //            obj.transform.localScale = scale;
                    //        }
                    //    }

                    //    //GameObject newElement = Instantiate(elements.GetRandom());

                    //    //newElement.transform.position = position + offset;
                    //    //newElement.transform.eulerAngles = rotation;
                    //    //newElement.transform.localScale = scale;
                    //    break;
                    //    //esse funciona
                    //    //var position = new Vector3(vert.x * 10f + Random.Range(-1f, 5f), vert.y + 1.5f, vert.z * 10f + Random.Range(-0.5f, 5f));

                    //    ////GameObject obj  = Instantiate(treePrefabs, position, Quaternion.identity);
                    //    ////obj.transform.localScale = new Vector3(4f, 4f, 4f);
                    //    //PrefabsPosition.Add(position);
                    //}

                    //esse funciona
                    Vector3 position = new Vector3(vert.x * 10f, vert.y, vert.z * 10f);
                    Vector3 offset = new Vector3(Random.Range(-0.75f, 0.75f), 0f, Random.Range(-0.75f, 0.75f));
                    Quaternion rotation = new Quaternion(Random.Range(0, 5f), Random.Range(0, 360f), Random.Range(0, 5f), 1f);
                    Vector3 scale = Vector3.one * Random.Range(3f, 7f);

                    //var position = new Vector3(vert.x * 10f + Random.Range(-1f, 5f), vert.y + 1.5f, vert.z * 10f + Random.Range(-0.5f, 5f));
                    GameObject go = Instantiate(treePrefabs, position + offset, rotation);
                    go.transform.localScale = scale;
                    PrefabsPosition.Add(position);

                }
            }
        }

    }


}
