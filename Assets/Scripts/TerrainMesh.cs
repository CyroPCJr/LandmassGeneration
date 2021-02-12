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
                    vertices[vertexIndicies] = new Vector3(topLeftX + x, animationCurve.Evaluate(noiseTerrain[x, z]) * Mathf.Pow(heightMultiplier, 2), topLeftZ - z);
                    uvs[vertexIndicies] = new Vector2(x / (float)width, z / (float)height);

                    if ((x < width - 1) && (z < height - 1))
                    {

                        // lt --- rt
                        // | \    |
                        // |  \   |
                        // |   \  |
                        // lb --- rb
                        int leftBotton = vertexIndicies + 0;        // left botton
                        int leftTop = vertexIndicies + width + 1;   // left top
                        int rightBotton = leftBotton + 1;           // right botton
                        int rightTop = leftTop + 1;                 // right top

                        indices[tris + 0] = vertexIndicies;
                        indices[tris + 1] = vertexIndicies  + width + 1;
                        indices[tris + 2] = vertexIndicies + width;
                        indices[tris + 3] = vertexIndicies + width + 1;
                        indices[tris + 4] = vertexIndicies;
                        indices[tris + 5] = vertexIndicies +1 ;
                        tris += 6;
                    }

                    vertexIndicies++;
                }
            }
        }

        //private void GenerateIndices()
        //{
        //    for (int vertices = 0, tris = 0, z = 0; z < _surfaceSize - 1; ++z) // heigth
        //    {
        //        for (int x = 0; x < _surfaceSize - 1; ++x) // width
        //        {
        //            // lt --- rt
        //            // | \    |
        //            // |  \   |
        //            // |   \  |
        //            // lb --- rb
        //            int leftBotton = vertices + 0;        // left botton
        //            int leftTop = vertices + (_surfaceSize - 1) + 1;   // left top
        //            int rightBotton = leftBotton + 1;     // right botton
        //            int rightTop = leftTop + 1;           // right top

        //            indices[tris + 0] = leftBotton;
        //            indices[tris + 1] = leftTop;
        //            indices[tris + 2] = rightBotton;
        //            indices[tris + 3] = leftTop;
        //            indices[tris + 4] = rightTop;
        //            indices[tris + 5] = rightBotton;
        //            vertices++;
        //            tris += 6;
        //        }
        //        vertices++;
        //    }
        //}
        

    }

}

