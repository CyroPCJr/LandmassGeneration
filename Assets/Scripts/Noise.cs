using UnityEngine;

namespace LandMassGeneration
{

    /// <summary>
    /// Refactoring PerlinNoise fucntion call as Noise class, so that I can add more tunes variables to play around such, octaves, lacunarity, persistence, offset, seed,height multiplier, width and height.
    /// As part as my research for better understand to use all this parameters, is to create more smooth noise transaction. For octaves represent how many layers noise 
    /// I can add, lacunarity tune for each octaves change the frequency witch is the wavelenght in the timeline, persistence is the amplitude witch is the positive sign of the wavelenght.
    /// Seeds is used for Random and give back a random number by a certain seed, so I can use different seed to give different result 
    /// adding with offset to "move" aroung the mesh, but not exatcly the mesh itself but the perlin noise. 
    /// Noise scale is just the proporcion scale for the mesh resolution calculate the proporicion of mesh width and height resolution.
    /// </summary>
    public class Noise
    {

        [System.Serializable]
        public struct NoiseSettings
        {
            public Vector2 offset;
            [Min(0.0001f)]
            public float noiseScale;
            [Range(0.0f, 1.0f)]
            public float persistence;
            [Min(1.0f)]
            public float lacunarity;
            [Range(1, 8)]
            public int octaves;
            public int seed;
            [Range(1, 255)]
            public int width;
            [Range(1, 255)]
            public int height;
        }

        public NoiseSettings NoiseParams;

        /// <summary>
        /// Calculate the noise based on Perlin Noise 2D with octaves to set how many layers, adding persistence with change the amplitude and lacunarity is the frequency
        /// </summary>
        /// <param name="noiseParams"></param>
        /// <returns>total noise</returns>
        public float[,] GenNoise(NoiseSettings noiseParams)
        {
            float[,] noise = new float[noiseParams.width, noiseParams.height];

            System.Random rng = new System.Random(noiseParams.seed);
            Vector2[] octavesOffsets = new Vector2[noiseParams.octaves];
            for (int i = 0; i < noiseParams.octaves; ++i)
            {
                float offsetX = rng.Next(-100000, 100000) + noiseParams.offset.x;
                float offsetY = rng.Next(-100000, 100000) + noiseParams.offset.y;
                octavesOffsets[i] = new Vector2(offsetX, offsetY);
            }

            float maxHeight = float.MinValue;
            float minHeight = float.MaxValue;

            float halfWidth = noiseParams.width * 0.5f;
            float halfHeight = noiseParams.height * 0.5f;

            for (int y = 0; y < noiseParams.height; ++y) // height
            {
                for (int x = 0; x < noiseParams.width; ++x) // width
                {
                    float frequency = 1f; // frequency is the wavelength for certain period of time
                    float amplitude = 1f; // amplitude is the positive sin wave 
                    float currentHeightValue = 0f; // total noises adding thought the sum of layers
                    for (int i = 0; i < noiseParams.octaves; ++i) // octaves make more interesting to add layers to the noises
                    {
                        Vector2 octOffSets = octavesOffsets[i];
                        float yCoord = ((y - halfHeight) / noiseParams.noiseScale * frequency) + octOffSets.y;
                        float xCoord = ((x - halfWidth) / noiseParams.noiseScale * frequency) + octOffSets.x;

                        float perlinNoise = (Mathf.PerlinNoise(xCoord, yCoord) * 2f) - 1f; // Perlin Noise Unity return values between 0..1
                        currentHeightValue += perlinNoise * amplitude;
                        amplitude *= noiseParams.persistence;
                        frequency *= noiseParams.lacunarity;
                    }

                    // get the hightest height from the noise
                    maxHeight = Mathf.Max(maxHeight, currentHeightValue);
                    // get the lowest height from the noise
                    minHeight = Mathf.Min(minHeight, currentHeightValue);

                    noise[x, y] = currentHeightValue;
                }
            }

            for (int y = 0; y < noiseParams.height; ++y)
            {
                for (int x = 0; x < noiseParams.width; ++x)
                {
                    // Normalize the noise back to min 0 and max 1 and 0.5 between
                    noise[x, y] = Mathf.InverseLerp(minHeight, maxHeight, noise[x, y]);
                }
            }

            return noise;
        }

    }
}


