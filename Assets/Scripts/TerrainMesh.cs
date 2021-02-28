using UnityEngine;

namespace LandMassGeneration
{

    public class TerrainMesh
    {

        public Vector3[] vertices;
        public Vector2[] uvs;
        public int[] indices;

        public TerrainMesh(int width, int height)
        {
            vertices = new Vector3[width * height];
            uvs = new Vector2[width * height];
            indices = new int[(width - 1) * (height - 1) * 6];
        }

        public void GenTerrain(float[,] noiseTerrain, float heightMultiplier, AnimationCurve animationCurve)
        {
            int width = noiseTerrain.GetLength(0);
            int height = noiseTerrain.GetLength(1);

            float topLeftX = (width - 1f) * -0.5f;
            float topLeftZ = (height - 1f) * 0.5f;

            int vertexIndicies = 0;
            int tris = 0;
            for (int z = 0; z < height; ++z)
            {
                for (int x = 0; x < width; ++x)
                {
                    // This using animation curve for better looking
                    //vertices[vertexIndicies] = new Vector3(topLeftX + x, animationCurve.Evaluate(noiseTerrain[x, z]) * Mathf.Pow(heightMultiplier, 2f), topLeftZ - z);
                    //This height is corret to export
                    vertices[vertexIndicies] = new Vector3(topLeftX + x, noiseTerrain[x, z] * Mathf.Pow(heightMultiplier, 2f), topLeftZ - z);
                    uvs[vertexIndicies] = new Vector2(x / (float)width, z / (float)height);
                    //uvs[vertexIndicies] = new Vector2(0, vertices[vertexIndicies].y);

                    if ((x < width - 1) && (z < height - 1))
                    {

                        // lt --- rt
                        // | \    |
                        // |  \   |
                        // |   \  |
                        // lb --- rb
                        //int leftBotton = vertexIndicies + 0;        // left botton
                        //int leftTop = vertexIndicies + width + 1;   // left top
                        //int rightBotton = leftBotton + width;           // right botton
                        //int rightTop = leftTop + 1;                 // right top

                        indices[tris + 0] = vertexIndicies;
                        indices[tris + 1] = vertexIndicies + width + 1;
                        indices[tris + 2] = vertexIndicies + width;
                        indices[tris + 3] = vertexIndicies + width + 1;
                        indices[tris + 4] = vertexIndicies;
                        indices[tris + 5] = vertexIndicies + 1;
                        tris += 6;
                    }

                    vertexIndicies++;
                }
            }
        }

    }

}