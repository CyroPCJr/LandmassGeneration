using UnityEngine;

public static class MeshGenerator
{

    public static MeshData GenerateTerrainMesh(float[,] noiseMap, float heightMultiplier, AnimationCurve heigthCurve)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);

        float topLeftX = (width - 1f) * -0.5f;
        float topLeftZ = (height - 1f) * 0.5f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndicies = 0;
        for (int z = 0; z < height; ++z)
        {
            for (int x = 0; x < width; ++x)
            {
                meshData.vertices[vertexIndicies] = new Vector3(topLeftX + x, heigthCurve.Evaluate(noiseMap[x, z]) * heightMultiplier, topLeftZ - z);
                meshData.uvs[vertexIndicies] = new Vector2(x / (float)width, z / (float)height);

                if ((x < width - 1) && (z < height - 1))
                {
                    meshData.AddTriangule(vertexIndicies, vertexIndicies + width + 1, vertexIndicies + width);
                    meshData.AddTriangule(vertexIndicies + width + 1, vertexIndicies, vertexIndicies + 1);
                }

                vertexIndicies++;
            }
        }

        return meshData;
    }

}


public class MeshData
{
    public Vector3[] vertices;
    public Vector2[] uvs;
    public int[] indices;

    int triangulesIndices = 0;
    public MeshData(int width, int height)
    {
        vertices = new Vector3[width * height];
        uvs = new Vector2[width * height];
        indices = new int[(width - 1) * (height - 1) * 6];
    }

    public void AddTriangule(int a, int b, int c)
    {
        indices[triangulesIndices] = a;
        indices[triangulesIndices + 1] = b;
        indices[triangulesIndices + 2] = c;
        triangulesIndices += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh
        {
            vertices = vertices,
            triangles = indices,
            uv = uvs
        };
        mesh.RecalculateNormals();
        return mesh;
    }

}