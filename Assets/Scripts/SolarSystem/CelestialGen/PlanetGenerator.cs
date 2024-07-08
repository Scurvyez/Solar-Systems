using System.Collections.Generic;
using System.Linq;
using Saving;
using Utils;

namespace SolarSystem
{
    public static class PlanetGenerator
    {
        private static int _currentOrbitPosition = 400;

        public static void GeneratePlanets<T>(List<T> planets, int planetCount) where T : Planet, new()
        {
            // Reset _currentOrbitPosition to 400 at the start of the method
            _currentOrbitPosition = 400;

            for (int i = 0; i < planetCount; i++)
            {
                T planet = new T
                {
                    Name = SystemNamingUtil.Info_SystemName + "-" + RomanNumConverter.ToRomanNumeral(i + 1),
                    OrbitPosition = _currentOrbitPosition
                };

                // Increment the orbit position for the next planet
                _currentOrbitPosition += 400;

                switch (planet)
                {
                    // Generate planet data based on type
                    case RockyPlanet rockyPlanet:
                        GenerateRockyPlanetData(rockyPlanet);
                        break;
                    case GasGiant gasGiantPlanet:
                        GenerateGasGiantPlanetData(gasGiantPlanet);
                        break;
                }

                planets.Add(planet);
            }

            SavePlanetData(planets);
        }
        
        private static void GenerateRockyPlanetData(RockyPlanet rockyPlanet)
        {
            SystemSaveData sSD = SaveManager.Instance.ActiveSave;
            
            rockyPlanet.GenerateGORadius();
            rockyPlanet.HasRandomRings();
            rockyPlanet.HasRandomAtmosphere();
            rockyPlanet.AtmosphereHeight(rockyPlanet.HasAtmosphere, rockyPlanet.Radius);
            rockyPlanet.GenerateMagneticFieldStrength();
            rockyPlanet.GetColor();
            rockyPlanet.GenerateAlbedo();
            rockyPlanet.GenerateOrbitalPeriod();
            rockyPlanet.GenerateFocusPoint();
            rockyPlanet.GenerateRotationalPeriod();
            rockyPlanet.GenerateAxialTilt();
            rockyPlanet.GenerateSurfacePressure();
            rockyPlanet.GenerateMass(sSD.starMass, rockyPlanet.FocusPoint, rockyPlanet.OrbitalPeriod);
            rockyPlanet.GenerateSurfaceGravity(rockyPlanet.Mass, rockyPlanet.Radius);
            rockyPlanet.GenerateEscapeVelocity(rockyPlanet.Mass, rockyPlanet.Radius);
            rockyPlanet.GenerateNumMoons(rockyPlanet.Mass, rockyPlanet.MagneticFieldStrength);
            rockyPlanet.GenerateSurfaceTemperature();
            rockyPlanet.HasLiquidWater(rockyPlanet.SurfaceTemperature, rockyPlanet.SurfacePressure, rockyPlanet.HasAtmosphere);
            rockyPlanet.IsRandomlyHabitable(sSD.habitableRangeInner, sSD.habitableRangeOuter, rockyPlanet.FocusPoint);
            rockyPlanet.GenerateAtmosphereComposition(rockyPlanet.HasAtmosphere, sSD.habitableRangeInner, sSD.habitableRangeOuter, rockyPlanet.SurfaceTemperature);
            rockyPlanet.GenerateComposition();
            rockyPlanet.GenerateMeanDensity(rockyPlanet.Composition);
            
            rockyPlanet.GenerateInnerRingRadius();
            rockyPlanet.GenerateOuterRingRadius();
        }
        
        private static void GenerateGasGiantPlanetData(GasGiant gasGiantPlanet)
        {
            SystemSaveData sSD = SaveManager.Instance.ActiveSave;
            
            gasGiantPlanet.GenerateGORadius();
            gasGiantPlanet.HasRandomRings();
            gasGiantPlanet.HasRandomAtmosphere();
            gasGiantPlanet.AtmosphereHeight(gasGiantPlanet.HasAtmosphere, gasGiantPlanet.Radius);
            gasGiantPlanet.GenerateMagneticFieldStrength();
            gasGiantPlanet.GetColor();
            gasGiantPlanet.GenerateAlbedo();
            gasGiantPlanet.GenerateOrbitalPeriod();
            gasGiantPlanet.GenerateFocusPoint();
            gasGiantPlanet.GenerateRotationalPeriod();
            gasGiantPlanet.GenerateAxialTilt();
            gasGiantPlanet.GenerateSurfacePressure();
            gasGiantPlanet.GenerateMass(sSD.starMass, gasGiantPlanet.FocusPoint, gasGiantPlanet.OrbitalPeriod);
            gasGiantPlanet.GenerateSurfaceGravity(gasGiantPlanet.Mass, gasGiantPlanet.Radius);
            gasGiantPlanet.GenerateEscapeVelocity(gasGiantPlanet.Mass, gasGiantPlanet.Radius);
            gasGiantPlanet.GenerateNumMoons(gasGiantPlanet.Mass, gasGiantPlanet.MagneticFieldStrength);
            gasGiantPlanet.GenerateSurfaceTemperature();
            gasGiantPlanet.HasLiquidWater(gasGiantPlanet.SurfaceTemperature, gasGiantPlanet.SurfacePressure, gasGiantPlanet.HasAtmosphere);
            gasGiantPlanet.IsRandomlyHabitable(sSD.habitableRangeInner, sSD.habitableRangeOuter, gasGiantPlanet.FocusPoint);
            gasGiantPlanet.GenerateAtmosphereComposition(gasGiantPlanet.HasAtmosphere, sSD.habitableRangeInner, sSD.habitableRangeOuter, gasGiantPlanet.SurfaceTemperature);
            gasGiantPlanet.GenerateComposition();
            gasGiantPlanet.GenerateMeanDensity(gasGiantPlanet.Composition);
            
            gasGiantPlanet.GenerateInnerRingRadius();
            gasGiantPlanet.GenerateOuterRingRadius();
        }
        
        private static void SavePlanetData<T>(List<T> planets) where T : Planet
        {
            if (SaveManager.Instance.HasLoaded)
            {
                if (typeof(T) == typeof(RockyPlanet))
                {
                    SaveManager.Instance.ActiveSave.rockyPlanets = planets.Cast<RockyPlanet>().ToList();
                }
                else if (typeof(T) == typeof(GasGiant))
                {
                    SaveManager.Instance.ActiveSave.gasGiantPlanets = planets.Cast<GasGiant>().ToList();
                }
            }
            else
            {
                if (typeof(T) == typeof(RockyPlanet))
                {
                    SaveManager.Instance.ActiveSave.rockyPlanets = planets.Cast<RockyPlanet>().ToList();
                }
                else if (typeof(T) == typeof(GasGiant))
                {
                    SaveManager.Instance.ActiveSave.gasGiantPlanets = planets.Cast<GasGiant>().ToList();
                }
            }
        }
        
        /*
        public static void GenerateRockyPlanets(List<RockyPlanet> rockyPlanets, int planetCount)
        {
            GrabFinalizedRockyPlanetData(rockyPlanets, planetCount);

            if (SaveManager.Instance.HasLoaded)
            {
                foreach (RockyPlanet rockyPlanet in SaveManager.Instance.ActiveSave.rockyPlanets)
                {
                    rockyPlanets = SaveManager.Instance.ActiveSave.rockyPlanets;
                }
            }
            else
            {
                SaveManager.Instance.ActiveSave.rockyPlanets = rockyPlanets;
            }
        }
        
        public static void GenerateGasGiantPlanets(List<GasGiant> gasGiantPlanets, int planetCount)
        {
            GrabFinalizedGasGiantPlanetData(gasGiantPlanets, planetCount);

            if (SaveManager.Instance.HasLoaded)
            {
                foreach (GasGiant gasGiantPlanet in SaveManager.Instance.ActiveSave.gasGiantPlanets)
                {
                    gasGiantPlanets = SaveManager.Instance.ActiveSave.gasGiantPlanets;
                }
            }
            else
            {
                SaveManager.Instance.ActiveSave.gasGiantPlanets = gasGiantPlanets;
            }
        }
           
        private static void GrabFinalizedRockyPlanetData(List<RockyPlanet> planets, int planetCount)
        {
            SystemSaveData sSD = SaveManager.Instance.ActiveSave;

            for (int i = 0; i < planetCount; i++)
            {
                RockyPlanet rockyPlanet = new ()
                {
                    Name = SystemNamingUtil.Info_SystemName + "-" + RomanNumConverter.ToRomanNumeral(i + 1)
                };
            
                rockyPlanet.GenerateGORadius();
            
                rockyPlanet.HasRandomRings();
                rockyPlanet.HasRandomAtmosphere();
                rockyPlanet.AtmosphereHeight(rockyPlanet.HasAtmosphere, rockyPlanet.Radius);
                rockyPlanet.GenerateMagneticFieldStrength();
                rockyPlanet.GetColor();
                rockyPlanet.GenerateAlbedo();
                rockyPlanet.GenerateOrbitalPeriod();
                rockyPlanet.GenerateFocusPoint();
                rockyPlanet.GenerateRotationalPeriod();
                rockyPlanet.GenerateAxialTilt();
                rockyPlanet.GenerateSurfacePressure();
                rockyPlanet.GenerateMass(sSD.starMass, rockyPlanet.FocusPoint, rockyPlanet.OrbitalPeriod);
                rockyPlanet.GenerateSurfaceGravity(rockyPlanet.Mass, rockyPlanet.Radius);
                rockyPlanet.GenerateEscapeVelocity(rockyPlanet.Mass, rockyPlanet.Radius);
                rockyPlanet.GenerateNumMoons(rockyPlanet.Mass, rockyPlanet.MagneticFieldStrength);
                rockyPlanet.GenerateSurfaceTemperature();
                rockyPlanet.HasLiquidWater(rockyPlanet.SurfaceTemperature, rockyPlanet.SurfacePressure, rockyPlanet.HasAtmosphere);
                rockyPlanet.IsRandomlyHabitable(sSD.habitableRangeInner, sSD.habitableRangeOuter, rockyPlanet.FocusPoint);
                rockyPlanet.GenerateAtmosphereComposition(rockyPlanet.HasAtmosphere, sSD.habitableRangeInner, sSD.habitableRangeOuter, rockyPlanet.SurfaceTemperature);
                rockyPlanet.GenerateComposition();
                rockyPlanet.GenerateMeanDensity(rockyPlanet.Composition);
            
                rockyPlanet.GenerateInnerRingRadius();
                rockyPlanet.GenerateOuterRingRadius();
            
                planets.Add(rockyPlanet);
            }
        }
        
        private static void GrabFinalizedGasGiantPlanetData(List<GasGiant> planets, int planetCount)
        {
            SystemSaveData sSD = SaveManager.Instance.ActiveSave;

            for (int i = 0; i < planetCount; i++)
            {
                GasGiant gasGiantPlanet = new ()
                {
                    Name = SystemNamingUtil.Info_SystemName + "-" + RomanNumConverter.ToRomanNumeral(i + 1)
                };
            
                gasGiantPlanet.GenerateGORadius();
            
                gasGiantPlanet.HasRandomRings();
                gasGiantPlanet.HasRandomAtmosphere();
                gasGiantPlanet.AtmosphereHeight(gasGiantPlanet.HasAtmosphere, gasGiantPlanet.Radius);
                gasGiantPlanet.GenerateMagneticFieldStrength();
                gasGiantPlanet.GetColor();
                gasGiantPlanet.GenerateAlbedo();
                gasGiantPlanet.GenerateOrbitalPeriod();
                gasGiantPlanet.GenerateFocusPoint();
                gasGiantPlanet.GenerateRotationalPeriod();
                gasGiantPlanet.GenerateAxialTilt();
                gasGiantPlanet.GenerateSurfacePressure();
                gasGiantPlanet.GenerateMass(sSD.starMass, gasGiantPlanet.FocusPoint, gasGiantPlanet.OrbitalPeriod);
                gasGiantPlanet.GenerateSurfaceGravity(gasGiantPlanet.Mass, gasGiantPlanet.Radius);
                gasGiantPlanet.GenerateEscapeVelocity(gasGiantPlanet.Mass, gasGiantPlanet.Radius);
                gasGiantPlanet.GenerateNumMoons(gasGiantPlanet.Mass, gasGiantPlanet.MagneticFieldStrength);
                gasGiantPlanet.GenerateSurfaceTemperature();
                gasGiantPlanet.HasLiquidWater(gasGiantPlanet.SurfaceTemperature, gasGiantPlanet.SurfacePressure, gasGiantPlanet.HasAtmosphere);
                gasGiantPlanet.IsRandomlyHabitable(sSD.habitableRangeInner, sSD.habitableRangeOuter, gasGiantPlanet.FocusPoint);
                gasGiantPlanet.GenerateAtmosphereComposition(gasGiantPlanet.HasAtmosphere, sSD.habitableRangeInner, sSD.habitableRangeOuter, gasGiantPlanet.SurfaceTemperature);
                gasGiantPlanet.GenerateComposition();
                gasGiantPlanet.GenerateMeanDensity(gasGiantPlanet.Composition);
            
                gasGiantPlanet.GenerateInnerRingRadius();
                gasGiantPlanet.GenerateOuterRingRadius();
            
                planets.Add(gasGiantPlanet);
            }
        }
        */
    }
}