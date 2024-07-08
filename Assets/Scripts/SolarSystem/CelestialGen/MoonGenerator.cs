using System.Collections.Generic;
using Saving;
using UnityEngine;

namespace SolarSystem
{
    public static class MoonGenerator
    {
        public static void GenerateRockyPlanetMoons(List<RockyPlanet> planets, List<Moon> moons)
        {
            GrabFinalizedRockyPlanetMoonData(planets, moons);

            if (SaveManager.Instance.HasLoaded)
            {
                foreach (Moon moon in SaveManager.Instance.ActiveSave.moons)
                {
                    moons = SaveManager.Instance.ActiveSave.moons;
                }
            }
            else
            {
                SaveManager.Instance.ActiveSave.moons = moons;
            }
        }
        
        private static void GrabFinalizedRockyPlanetMoonData(List<RockyPlanet> planets, List<Moon> moons)
        {
            foreach (RockyPlanet rockyPlanet in planets)
            {
                int numMoons = rockyPlanet.Moons;

                for (int i = 0; i < numMoons; i++)
                {
                    Moon moon = new ()
                    {
                        ParentPlanetName = rockyPlanet.Name,
                        Name = rockyPlanet.Name
                    };
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
                    moons.Add(moon);
                }
            }
        }
    }
}