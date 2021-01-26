using UnityEngine;

public class TerrainGameObject : MonoBehaviour
{
    [SerializeField] private TerrainGeneratorSO _terrainData = null;
    private Mesh _mesh = null;

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
        if (!_mesh) return;

        _mesh.Clear();
        _mesh.vertices = _terrainData.GeTerrainMesh.vertices;
        _mesh.triangles = _terrainData.GeTerrainMesh.indicesVertices;
        _mesh.Optimize();
        _mesh.RecalculateNormals();
    }

    [ContextMenu("ChangeHeightMap")]
    public void ChangeHeightMap()
    {
        _terrainData.SetHeightMap();
        _terrainData.GenerateShape();
    }

    private void OnDrawGizmos()
    {
        _terrainData.GenerateShape();
        if (_terrainData.GeTerrainMesh.vertices == null) return;

        Vector3[] vertices = _terrainData.GeTerrainMesh.vertices;

        for (int x = 0; x < vertices.Length; x++)
        {
            Gizmos.DrawSphere(vertices[x], 0.1f);
        }
    }

}
