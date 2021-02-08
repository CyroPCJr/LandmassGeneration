using System;
using UnityEngine;

namespace Terrain
{
    public class TerrainGenerator
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public float Scale { get; set; }

        public float HeightMap { get; set; } = 0.0f;

        public float Amplitude_1 { get; set; } = 0.0f;
        public float Amplitude_2 { get; set; } = 0.0f;
        public float Amplitude_3 { get; set; } = 0.0f;
        public float Frequency_1 { get; set; } = 0.0f;
        public float Frequency_2 { get; set; } = 0.0f;
        public float Frequency_3 { get; set; } = 0.0f;

        private float _minTerrainHeight;
        private float _maxTerrainHeight;

        public struct TerrainMesh
        {
            public Vector3[] vertices;
            public Vector2[] uvs;
            public Color[] colors;
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
                    //float NoisePosY = Mathf.PerlinNoise((float)x/Width, (float)z/Height) * 2.0f -1.0f;
                    //TerrainMeshes.vertices[i++] = new Vector3(x, NoisePosY * Scale, z);
                    TerrainMeshes.vertices[i++] = new Vector3(x, Noise(x, z), z);
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
                    int leftBotton = vertices + 0;        // left botton
                    int leftTop = vertices + Width + 1;   // left top
                    int rightBotton = leftBotton + 1;     // right botton
                    int rightTop = leftTop + 1;           // right top

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

        public void GenerateColors(ref Gradient gradient)
        {
            TerrainMeshes.colors = new Color[TerrainMeshes.vertices.Length];
            for (int i = 0, z = 0; z <= Height; ++z) // rolls
            {
                for (int x = 0; x <= Width; ++x) // columns
                {
                    float y = Mathf.InverseLerp(_minTerrainHeight, _maxTerrainHeight, TerrainMeshes.vertices[i].y);
                    TerrainMeshes.colors[i] = gradient.Evaluate(y);
                    i++;
                }
            }
        }

        private float Noise(int x, int z)
        {
            float decimalProportionX = (float)x / Width;
            float decimalProportionY = (float)z / Height;
            //float noise = Mathf.PerlinNoise(decimalProportionX, decimalProportionY);
            //_maxTerrainHeight = Mathf.Max(_maxTerrainHeight, noise);
            //_minTerrainHeight = Mathf.Min(_minTerrainHeight, noise);
            //return noise * Scale;

            float noiseLayers =
                    Amplitude_1 * Mathf.PerlinNoise(decimalProportionX * Frequency_1, decimalProportionY * Frequency_1)
                    + Amplitude_2 * Mathf.PerlinNoise(decimalProportionX * Frequency_2, decimalProportionY * Frequency_2)
                    + Amplitude_3 * Mathf.PerlinNoise(decimalProportionX * Frequency_3, decimalProportionY * Frequency_3)
                        * Scale;
            _maxTerrainHeight = Mathf.Max(_maxTerrainHeight, noiseLayers);
            _minTerrainHeight = Mathf.Min(_minTerrainHeight, noiseLayers);
            return noiseLayers;
        }
    }

}