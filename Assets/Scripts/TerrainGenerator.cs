using UnityEngine;

namespace Terrain
{

    public class TerrainGenerator
    {
        public int Width { get; set; }
        public int Height { get; set; }

        public float HeightMap { get; set; } = 0.0f;

        public struct TerrainMesh
        {
            public Vector3[] vertices;
            public int[] indicesVertices;
        }

        public TerrainMesh TerrainMeshes;

        public void GenerateVertices()
        {
            TerrainMeshes.vertices = new Vector3[(Width + 1) * (Height + 1)];
            for (int i = 0, z = 0; z <= Height; ++z) // rolls
            {
                for (int x = 0; x <= Width; ++x) // columns
                {
                    TerrainMeshes.vertices[i++] = new Vector3(x, HeightMap, z);
                    //TerrainMeshes.vertices[i++] = new Vector3(x, Random.Range(0.0f, HeightMap), z);
                }
            }
        }

        public void GenerateIndices()
        {
            TerrainMeshes.indicesVertices = new int[Width * Height * 6];
            for (int vertices = 0, tris = 0, z = 0; z < Height; ++z) // rolls
            {
                for (int x = 0; x < Width; ++x) // columns
                {
                    // lt --- rt
                    // | \    |
                    // |  \   |
                    // |   \  |
                    // lb --- rb
                    int leftBotton = vertices + 0;      // left botton
                    int leftTop = vertices + Width + 1; // left top
                    int rightBotton = vertices + 1;     // right botton
                    int rightTop = leftTop + 1;         // right top

                    TerrainMeshes.indicesVertices[tris + 0] = leftBotton;
                    TerrainMeshes.indicesVertices[tris + 1] = leftTop;
                    TerrainMeshes.indicesVertices[tris + 2] = rightBotton;
                    TerrainMeshes.indicesVertices[tris + 3] = leftTop;
                    TerrainMeshes.indicesVertices[tris + 4] = rightTop;
                    TerrainMeshes.indicesVertices[tris + 5] = rightBotton;
                    vertices++;
                    tris += 6;
                }
                vertices++;
            }
        }

    }

}