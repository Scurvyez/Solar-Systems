using System.Collections.Generic;
using Saving;

namespace SolarSystem
{
    public static class MoonGenerator
    {
        public static void GenerateMoonData(Moon moon)
        {
            moon.GenerateMass();
            moon.GenerateGORadius();
            moon.GenerateOrbitalPeriod();
            moon.GenerateOrbitalDistanceFrom();
            moon.GenerateRotationPeriod();
            moon.GenerateAxialTilt();
            moon.GenerateSurfaceTemperature();
            moon.HasRandomAtmosphere();
            moon.IsRandomlyHabitable();
            moon.GenerateMeanDensity();
            moon.GenerateSurfaceGravity();
            moon.GenerateEscapeVelocity();
            moon.GenerateAlbedo();
        }

        public static void SaveMoonData(List<Moon> moons)
        {
            if (SaveManager.Instance.HasLoaded)
            {
                SaveManager.Instance.ActiveSave.moons = moons;
            }
            else
            {
                SaveManager.Instance.ActiveSave.moons = moons;
            }
        }
    }
}