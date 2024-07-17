using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    [System.Serializable]
    public class SystemSaveData
    {
        //public string saveName;

        // STAR DATA: start
        public string starClassAsString;
        public string starSystemName;
        public double starAge;
        public Vector3 starPosition;
        public float starMass;
        public float starRadius;
        public float starActualRadius;
        public float starLuminosity;
        public float starTemperature;
        public float starRotation;
        public float starMagneticField;
        public bool hasIntrinsicVariability;
        public bool hasExtrinsicVariability;
        public float starVariability;
        public float habitableRangeInner;
        public float habitableRangeOuter;
        public int planetCount;

        public Vector3 starSize;
        public Color starChromaticity; // primary color (surface)
        public Color starCellColor; // secondary color (solar flare)

        public SerializableDictionary<string, float> starMetallicity;
        // STAR DATA: end

        // PLANET DATA: start
        public List<Planet> planets;
        public List<RockyPlanet> rockyPlanets;
        public List<GasGiant> gasGiantPlanets;
        // PLANET DATA: end

        // PLANET DATA: start
        public List<Moon> moons;
        // PLANET DATA: end
    }
}