using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGameObject : MonoBehaviour
{
    [SerializeField] private TerrainGeneratorSO _terrainData = null;

    private Mesh _mesh = null;
    private MeshRenderer _meshRenderer = null;
    private MeshFilter _meshFilter = null;
    public TerrainGeneratorSO TerrainSO => _terrainData;

    private void Awake()
    {
        LoadComponents();
    }

    public void UpdateMesh()
    {
        if (!_mesh || !_meshRenderer || !_meshFilter)
        {
            LoadComponents();
        }

        _terrainData.Generate();
        SetMeshTerrain(_mesh);
        SetTextureTerrain(_terrainData.TerrainTexture);

        UpateMaterial(_meshRenderer.sharedMaterial);
    }

    private void LoadComponents()
    {
        if (!TryGetComponent(out _meshFilter))
        {
            Debug.LogWarning("[WARNING] -- Fail to load Mesh component.");
        }

        if (!TryGetComponent(out _meshRenderer))
        {
            Debug.LogWarning("[WARNING] -- Fail to load Mesh Renderer component.");
        }

        _mesh = new Mesh();
    }

    private void SetTextureTerrain(Texture2D texture)
    {
        _meshFilter.sharedMesh = _mesh;
        _meshRenderer.sharedMaterial.mainTexture = texture;
    }

    private void SetMeshTerrain(Mesh mesh)
    {
        mesh.Clear();
        mesh.vertices = _terrainData.TerrainMesh.vertices;
        mesh.triangles = _terrainData.TerrainMesh.indices;
        mesh.uv = _terrainData.TerrainMesh.uvs;
        mesh.Optimize();
        mesh.RecalculateNormals();
    }

    private void UpateMaterial(Material material)
    {
        material.SetFloat("_minHeight", _terrainData.MinHeight);
        material.SetFloat("_maxHeight", _terrainData.MaxHeight);

        material.SetFloat("_waterHeight", _terrainData.TerrainTypes[0].Height * _terrainData.MaxHeight);
        material.SetTexture("_waterTexture", _terrainData.TerrainTypes[0].Texuture);

        material.SetFloat("_sandHeight", _terrainData.TerrainTypes[1].Height * _terrainData.MaxHeight);
        material.SetTexture("_sandTexture", _terrainData.TerrainTypes[1].Texuture);

        material.SetFloat("_grassHeight", _terrainData.TerrainTypes[2].Height * _terrainData.MaxHeight);
        material.SetTexture("_grassTexture", _terrainData.TerrainTypes[2].Texuture);

        material.SetFloat("_rockHeight", _terrainData.TerrainTypes[3].Height * _terrainData.MaxHeight);
        material.SetTexture("_rockTexture", _terrainData.TerrainTypes[3].Texuture);

        material.SetFloat("_mountainHeight", _terrainData.TerrainTypes[4].Height * _terrainData.MaxHeight);
        material.SetTexture("_mountainTexture", _terrainData.TerrainTypes[4].Texuture);

        material.SetFloat("_snowHeight", _terrainData.TerrainTypes[5].Height * _terrainData.MaxHeight);
        material.SetTexture("_snowTexture", _terrainData.TerrainTypes[5].Texuture);

    }

}
