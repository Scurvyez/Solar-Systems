using System.Collections.Generic;
using Saving;
using UnityEngine;

namespace SolarSystem
{
    public static class MoonGenerator
    {
        public static void GenerateMoons(List<RockyPlanet> planets, List<Moon> moons)
        {
            GrabFinalizedMoonData(planets, moons);

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
        
        private static void GrabFinalizedMoonData(List<RockyPlanet> planets, List<Moon> moons)
        {
            foreach (RockyPlanet planet in planets)
            {
                int numMoons = Random.Range(0, 7);

                for (int i = 0; i < numMoons; i++)
                {
                    Moon moon = new ()
                    {
                        Name = planet.Name
                    };
                    moon.GetIndex(i + 1);
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