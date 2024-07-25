using System.Collections.Generic;
using Saving;

namespace SolarSystem
{
    public static class MoonGenerator
    {
        public static void GenerateMoonData(Moon moon)
        {
            moon.GenerateGORadius();
            moon.GenerateInfoRadius();
            moon.GenerateComposition();
            moon.GenerateMeanDensity(moon.Info_Composition);
            moon.HasRandomAtmosphere();
            moon.GenerateMagneticFieldStrength();
            moon.GenerateOrbitalPeriod();
            moon.GenerateOrbitalDistanceFrom();
            moon.GenerateRotationPeriod();
            moon.GenerateAxialTilt();
            moon.GenerateMass(moon.Info_Radius, moon.Info_MeanDensity);
            moon.GenerateSurfaceGravity(moon.Info_Mass, moon.Info_Radius);
            moon.GenerateEscapeVelocity(moon.Info_Mass, moon.Info_Radius);
            moon.GenerateSurfaceTemperature();
            moon.GenerateAtmosphereComposition(moon.Info_HasAtmosphere);
            moon.GenerateSurfacePressure(moon.Info_AtmosphereComposition, moon.Info_SurfaceGravity, moon.Info_SurfaceTemperature);
            moon.HasLiquidWater(moon.Info_SurfaceTemperature, moon.Info_SurfacePressure, moon.Info_HasAtmosphere);
            moon.IsRandomlyHabitable();
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