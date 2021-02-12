using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TerrainGameObject : MonoBehaviour
{
    [SerializeField] private TerrainGeneratorSO _terrainData = null;
    private Mesh _mesh = null;

    public TerrainGeneratorSO TerrainSO => _terrainData;

    private void Awake()
    {
        if (!TryGetComponent<MeshFilter>(out var meshFilter))
        {
            Debug.LogWarning("[WARNING] -- Fail to load Mesh component.");
        }

        _mesh = new Mesh();
        meshFilter.mesh = _mesh;
    }

    public void UpdateMesh()
    {
        if (!_mesh)
        {
            if (!TryGetComponent<MeshFilter>(out var _meshFilter))
            {
                Debug.LogWarning("[WARNING] -- Fail to load Mesh component.");
            }

            _mesh = new Mesh();
            _meshFilter.mesh = _mesh;
        }
        _terrainData.Generate();
        //_meshFilter.mesh = _terrainData.GenerateMesh;

        //_mesh.Clear();
        //_mesh.vertices = _terrainData.GeTerrainMesh.vertices;
        //_mesh.triangles = _terrainData.GeTerrainMesh.indicesVertices;
        //_mesh.colors = _terrainData.GeTerrainMesh.colors;
        //_mesh.Optimize();
        //_mesh.RecalculateNormals();

        _mesh.Clear();
        _mesh.vertices = _terrainData.TerrainMesh.vertices;
        _mesh.triangles = _terrainData.TerrainMesh.indices;
        _mesh.uv = _terrainData.TerrainMesh.uvs;
        _mesh.colors32 = _terrainData.ColorsTerrainHeight;
        _mesh.Optimize();
        _mesh.RecalculateNormals();
    }

}
