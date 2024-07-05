using System.Collections.Generic;
using UnityEngine;

namespace Saving
{
    [System.Serializable]
    public class SystemSaveData
    {
        //public string saveName;

        // STAR DATA: start
        public string starClassAsString;                                // class
        public string starSystemName;                                   // name
        public double starAge;                                          // age
        public Vector3 starPosition;                                    // position
        public float starMass;                                          // mass
        public float starRadius;                                        // radius
        public float starActualRadius;                                  // actual radius for gae object
        public float starLuminosity;                                    // luminosity
        public float starTemperature;                                   // temperature
        public float starRotation;                                      // rotation
        public float starMagneticField;                                 // magnetic field
        public bool hasIntrinsicVariability;                            // is intrinsic?
        public bool hasExtrinsicVariability;                            // is extrinsic?
        public float starVariability;                                   // variability
        public float habitableRangeInner;                               // inner hab zone boundary
        public float habitableRangeOuter;                               // outer hab zone boundary

        public Vector3 starSize;                                        // scale
        public Color starChromaticity;                                  // primary color (surface)
        public Color starCellColor;                                     // secondary color (solar flare)

        public SerializableDictionary<string, float> starMetallicity;   // metallicity
        // STAR DATA: end

        // PLANET DATA: start
        public List<RockyPlanet> rockyPlanets;
        // PLANET DATA: end

        // PLANET DATA: start
        public List<Moon> moons;
        // PLANET DATA: end
    }
}