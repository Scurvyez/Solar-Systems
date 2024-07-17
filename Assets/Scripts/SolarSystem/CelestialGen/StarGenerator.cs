using Saving;
using UnityEngine;
using Utils;

namespace SolarSystem
{
    public static class StarGenerator
    {
        private static string starClass;

        public static void GenerateStar(int buttonIndex, Star star)
        {
            SystemSaveData sSD = SaveManager.Instance.ActiveSave;
            if (star == null)
            {
                Debug.LogError("[StarGenerator.GenerateStar] Star is null");
                return;
            }

            GrabFinalizedStarData(buttonIndex, star);

            if (SaveManager.Instance.HasLoaded)
            {
                SystemNamingUtil.Info_SystemName = sSD.starSystemName;
                starClass = sSD.starClassAsString;

                star.Info_Age = sSD.starAge;
                star.Info_Radius = sSD.starRadius;
                star.Info_Temperature = sSD.starTemperature;
                star.Info_Rotation = sSD.starRotation;
                star.Info_MagneticField = sSD.starMagneticField;
                star.Info_Metallicity = sSD.starMetallicity;
                star.Info_Variability = sSD.starVariability;
                star.Info_Luminosity = sSD.starLuminosity;
                star.Info_Mass = sSD.starMass;
                star.PlanetCount = sSD.planetCount;
                
                star.ExtrinsicVariability = sSD.hasExtrinsicVariability;
                star.IntrinsicVariability = sSD.hasIntrinsicVariability;
                
                star.GO_Size = sSD.starSize;
                star.GO_Position = sSD.starPosition;
                star.GO_Radius = sSD.starActualRadius;
                star.GO_Chromaticity = sSD.starChromaticity;
                star.GO_CellColor = sSD.starCellColor;
                star.GO_HabitableRangeInner = sSD.habitableRangeInner;
                star.GO_HabitableRangeOuter = sSD.habitableRangeOuter;
            }
            else
            {
                sSD.starSystemName = SystemNamingUtil.Info_SystemName;
                sSD.starClassAsString = DetermineStarClass(star);

                sSD.starAge = star.Info_Age;
                sSD.starRadius = star.Info_Radius;
                sSD.starTemperature = star.Info_Temperature;
                sSD.starRotation = star.Info_Rotation;
                sSD.starMagneticField = star.Info_MagneticField;
                sSD.starMetallicity = star.Info_Metallicity;
                sSD.starVariability = star.Info_Variability;
                sSD.starLuminosity = star.Info_Luminosity;
                sSD.starMass = star.Info_Mass;
                sSD.planetCount = star.PlanetCount;
                
                sSD.hasExtrinsicVariability = star.ExtrinsicVariability;
                sSD.hasIntrinsicVariability = star.IntrinsicVariability;
                
                sSD.starSize = star.GO_Size;
                sSD.starPosition = star.GO_Position;
                sSD.starActualRadius = star.GO_Radius;
                sSD.starChromaticity = star.GO_Chromaticity;
                sSD.starCellColor = star.GO_CellColor;
                sSD.habitableRangeInner = star.GO_HabitableRangeInner;
                sSD.habitableRangeOuter = star.GO_HabitableRangeOuter;
            }
        }
        
        private static string DetermineStarClass(Star star)
        {
            Star.SpectralType spectralType = star.SpectralClass;
            starClass = spectralType switch
            {
                Star.SpectralType.O => spectralType.ToString(),
                Star.SpectralType.B => spectralType.ToString(),
                Star.SpectralType.A => spectralType.ToString(),
                Star.SpectralType.F => spectralType.ToString(),
                Star.SpectralType.G => spectralType.ToString(),
                Star.SpectralType.K => spectralType.ToString(),
                Star.SpectralType.M => spectralType.ToString(),
                _ => starClass
            };
            return starClass;
        }
        
        private static void GrabFinalizedStarData(int buttonIndex, Star star)
        {
            Star.SpectralType spectralType = (Star.SpectralType)buttonIndex;
            star.SpectralClass = spectralType;
            
            SystemNamingUtil.PickNamingMethodAndGenerate();
        
            star.GenerateInfoAge(spectralType);
            star.GenerateInfoRadius(spectralType);
            star.GenerateInfoTemperature(spectralType);
            star.GenerateInfoRotation(spectralType);
            star.GenerateInfoMagneticField(spectralType);
            star.Info_Metallicity = new SerializableDictionary<string, float>();
            star.GenerateInfoMetallicity(spectralType);
            star.GenerateInfoIntrinsicVariability(spectralType);
            star.GenerateInfoLuminosity(star.Info_Radius, star.Info_Temperature);
            star.GenerateInfoMass(star.Info_Luminosity);
            star.GenerateNumPlanets();
        
            star.GenerateGOStartingPosition();
            star.GenerateGORadius();
            star.GenerateGOChromaticity(spectralType);
            star.GenerateGOCellColor();
            star.GenerateGOHabitableRangeInner(spectralType);
            star.GenerateGOHabitableRangeOuter(spectralType);
        }
    }
}