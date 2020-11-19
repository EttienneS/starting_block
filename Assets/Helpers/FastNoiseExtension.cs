namespace Assets.Helpers
{
    public static class FastNoiseExtension
    {
        public static float[,] GetNoiseMap(this FastNoiseLite noise, int scale)
        {
            float[,] noiseData = new float[scale, scale];

            for (int y = 0; y < scale; y++)
            {
                for (int x = 0; x < scale; x++)
                {
                    noiseData[x, y] = noise.GetNoise(x, y);
                }
            }

            return noiseData;
        }
    }
}