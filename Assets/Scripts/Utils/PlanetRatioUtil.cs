namespace Utils
{
    public static class PlanetRatioUtil
    {
        /// <summary>
        /// Returns the percentage of rocky planets in a generated solar system.
        /// </summary>
        public static float GetRockyPlanetsRatio(Star.SpectralType spectralClass)
        {
            return spectralClass switch
            {
                Star.SpectralType.O => 0.2f,
                Star.SpectralType.B => 0.3f,
                Star.SpectralType.A => 0.4f,
                Star.SpectralType.F => 0.5f,
                Star.SpectralType.G => 0.6f,
                Star.SpectralType.K => 0.7f,
                Star.SpectralType.M => 0.8f,
                _ => 0.5f
            };
        }
    }
}