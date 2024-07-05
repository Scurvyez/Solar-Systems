using System.Collections.Generic;
using Saving;
using UnityEngine;
using Utils;

namespace SolarSystem
{
    public static class PlanetGenerator
    {
        public static void GeneratePlanets(List<RockyPlanet> planets)
        {
            GrabFinalizedPlanetData(planets);

            if (SaveManager.Instance.HasLoaded)
            {
                foreach (RockyPlanet rockyPlanet in SaveManager.Instance.ActiveSave.rockyPlanets)
                {
                    planets = SaveManager.Instance.ActiveSave.rockyPlanets;
                }
            }
            else
            {
                SaveManager.Instance.ActiveSave.rockyPlanets = planets;
            }
        }
        
        private static void GrabFinalizedPlanetData(List<RockyPlanet> planets)
        {
            SystemSaveData saveData = SaveManager.Instance.ActiveSave;
            int numPlanets = Random.Range(0, 13); // generate a random number of planets between 1 and 12

            for (int i = 0; i < numPlanets; i++)
            {
                // create a new rocky planet object
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
                rockyPlanet.GenerateMass(saveData.starMass, rockyPlanet.FocusPoint, rockyPlanet.OrbitalPeriod);
                rockyPlanet.GenerateSurfaceGravity(rockyPlanet.Mass, rockyPlanet.Radius);
                rockyPlanet.GenerateEscapeVelocity(rockyPlanet.Mass, rockyPlanet.Radius);
                rockyPlanet.GenerateSurfaceTemperature();
                rockyPlanet.HasLiquidWater(rockyPlanet.SurfaceTemperature, rockyPlanet.SurfacePressure, rockyPlanet.HasAtmosphere);
                rockyPlanet.IsRandomlyHabitable(saveData.habitableRangeInner, saveData.habitableRangeOuter, rockyPlanet.FocusPoint);
                rockyPlanet.GenerateAtmosphereComposition(rockyPlanet.HasAtmosphere, saveData.habitableRangeInner, saveData.habitableRangeOuter, rockyPlanet.SurfaceTemperature);
                rockyPlanet.GenerateComposition();
                rockyPlanet.GenerateMeanDensity(rockyPlanet.Composition);
            
                rockyPlanet.GenerateInnerRingRadius();
                rockyPlanet.GenerateOuterRingRadius();
            
                planets.Add(rockyPlanet);
            }
        }
    }
}