using UnityEngine;

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

    [ContextMenu("GenerateMeshes")]
    public void GenerateMeshes()
    {
        if (!_mesh)
        {
            if (!TryGetComponent<MeshFilter>(out var meshFilter))
            {
                Debug.LogWarning("[WARNING] -- Fail to load Mesh component.");
            }

            _mesh = new Mesh();
            meshFilter.mesh = _mesh;
        }

        _mesh.Clear();
        _mesh.vertices = _terrainData.GeTerrainMesh.vertices;
        _mesh.triangles = _terrainData.GeTerrainMesh.indicesVertices;
        _mesh.colors = _terrainData.GeTerrainMesh.colors;
        _mesh.Optimize();
        _mesh.RecalculateNormals();
    }

}
