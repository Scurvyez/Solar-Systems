using System.Collections.Generic;
using Saving;

namespace SolarSystem
{
    public static class PlanetGenerator
    {
        public static int CurrentOrbitPosition { get; set; } = 10;
        
        public static void GeneratePlanetData(Planet planet)
        {
            SystemSaveData sSD = SaveManager.Instance.ActiveSave;

            planet.GenerateGORadius();
            planet.GenerateInfoRadius();
            planet.GenerateComposition();
            planet.GenerateMeanDensity(planet.Info_Composition);
            planet.HasRandomRings();
            planet.HasRandomAtmosphere();
            planet.AtmosphereHeight(planet.Info_HasAtmosphere, planet.Info_Radius);
            planet.GenerateMagneticFieldStrength();
            planet.GetColor();
            planet.GenerateAlbedo();
            planet.GenerateOrbitalPeriod();
            planet.GenerateFocusPoint();
            planet.GenerateRotationalPeriod();
            planet.GenerateAxialTilt();
            planet.GenerateMass(planet.Info_Radius, planet.Info_MeanDensity);
            planet.GenerateSurfaceGravity(planet.Info_Mass, planet.Info_Radius);
            planet.GenerateEscapeVelocity(planet.Info_Mass, planet.Info_Radius);
            planet.GenerateNumMoons();
            planet.GenerateSurfaceTemperature();
            planet.HasLiquidWater(planet.Info_SurfaceTemperature, planet.Info_SurfacePressure, planet.Info_HasAtmosphere);
            planet.IsRandomlyHabitable(sSD.habitableRangeInner, sSD.habitableRangeOuter, planet.Info_FocusPoint);
            planet.GenerateAtmosphereComposition(planet.Info_HasAtmosphere);
            planet.GenerateSurfacePressure(planet.Info_AtmosphereComposition, planet.Info_SurfaceGravity, planet.Info_SurfaceTemperature);
            planet.GenerateInnerRingRadius();
            planet.GenerateOuterRingRadius();
        }

        public static void SavePlanetData(List<Planet> planets)
        {
            if (SaveManager.Instance.HasLoaded)
            {
                SaveManager.Instance.ActiveSave.planets = planets;
            }
            else
            {
                SaveManager.Instance.ActiveSave.planets = planets;
            }
        }
    }
}