using Saving;
using UnityEngine;
using Utils;

namespace SolarSystem
{
    public static class StarGenerator
    {
        private static string starClass;

        public static void GenerateStar(int bI, StarProperties props)
        {
            if (props == null)
            {
                Debug.LogError("StarPhysicalProperties is null");
                return;
            }

            GrabFinalizedStarData(bI, props);

            if (SaveManager.Instance.HasLoaded)
            {
                SystemNamingUtil.Info_SystemName = SaveManager.Instance.ActiveSave.starSystemName;
                starClass = SaveManager.Instance.ActiveSave.starClassAsString;

                props.Info_Age = SaveManager.Instance.ActiveSave.starAge;
                props.Info_Radius = SaveManager.Instance.ActiveSave.starRadius;
                props.Info_Temperature = SaveManager.Instance.ActiveSave.starTemperature;
                props.Info_Rotation = SaveManager.Instance.ActiveSave.starRotation;
                props.Info_MagneticField = SaveManager.Instance.ActiveSave.starMagneticField;
                props.Info_Metallicity = SaveManager.Instance.ActiveSave.starMetallicity;
                props.Info_Variability = SaveManager.Instance.ActiveSave.starVariability;
                props.Info_Luminosity = SaveManager.Instance.ActiveSave.starLuminosity;
                props.Info_Mass = SaveManager.Instance.ActiveSave.starMass;
                
                props.ExtrinsicVariability = SaveManager.Instance.ActiveSave.hasExtrinsicVariability;
                props.IntrinsicVariability = SaveManager.Instance.ActiveSave.hasIntrinsicVariability;
                
                props.GO_Size = SaveManager.Instance.ActiveSave.starSize;
                props.GO_Position = SaveManager.Instance.ActiveSave.starPosition;
                props.GO_Radius = SaveManager.Instance.ActiveSave.starActualRadius;
                props.GO_Chromaticity = SaveManager.Instance.ActiveSave.starChromaticity;
                props.GO_CellColor = SaveManager.Instance.ActiveSave.starCellColor;
                props.GO_HabitableRangeInner = SaveManager.Instance.ActiveSave.habitableRangeInner;
                props.GO_HabitableRangeOuter = SaveManager.Instance.ActiveSave.habitableRangeOuter;
            }
            else
            {
                SaveManager.Instance.ActiveSave.starSystemName = SystemNamingUtil.Info_SystemName;
                SaveManager.Instance.ActiveSave.starClassAsString = DetermineStarClass(props);

                SaveManager.Instance.ActiveSave.starAge = props.Info_Age;
                SaveManager.Instance.ActiveSave.starRadius = props.Info_Radius;
                SaveManager.Instance.ActiveSave.starTemperature = props.Info_Temperature;
                SaveManager.Instance.ActiveSave.starRotation = props.Info_Rotation;
                SaveManager.Instance.ActiveSave.starMagneticField = props.Info_MagneticField;
                SaveManager.Instance.ActiveSave.starMetallicity = props.Info_Metallicity;
                SaveManager.Instance.ActiveSave.starVariability = props.Info_Variability;
                SaveManager.Instance.ActiveSave.starLuminosity = props.Info_Luminosity;
                SaveManager.Instance.ActiveSave.starMass = props.Info_Mass;
                
                SaveManager.Instance.ActiveSave.hasExtrinsicVariability = props.ExtrinsicVariability;
                SaveManager.Instance.ActiveSave.hasIntrinsicVariability = props.IntrinsicVariability;
                
                SaveManager.Instance.ActiveSave.starSize = props.GO_Size;
                SaveManager.Instance.ActiveSave.starPosition = props.GO_Position;
                SaveManager.Instance.ActiveSave.starActualRadius = props.GO_Radius;
                SaveManager.Instance.ActiveSave.starChromaticity = props.GO_Chromaticity;
                SaveManager.Instance.ActiveSave.starCellColor = props.GO_CellColor;
                SaveManager.Instance.ActiveSave.habitableRangeInner = props.GO_HabitableRangeInner;
                SaveManager.Instance.ActiveSave.habitableRangeOuter = props.GO_HabitableRangeOuter;
            }
        }
        
        private static string DetermineStarClass(StarProperties props)
        {
            StarProperties.SpectralType spectralType = props.SpectralClass;
            starClass = spectralType switch
            {
                StarProperties.SpectralType.O => spectralType.ToString(),
                StarProperties.SpectralType.B => spectralType.ToString(),
                StarProperties.SpectralType.A => spectralType.ToString(),
                StarProperties.SpectralType.F => spectralType.ToString(),
                StarProperties.SpectralType.G => spectralType.ToString(),
                StarProperties.SpectralType.K => spectralType.ToString(),
                StarProperties.SpectralType.M => spectralType.ToString(),
                _ => starClass
            };
            return starClass;
        }
        
        private static void GrabFinalizedStarData(int buttonIndex, StarProperties props)
        {
            StarProperties.SpectralType spectralType = (StarProperties.SpectralType)buttonIndex;
            props.SpectralClass = spectralType;
            
            SystemNamingUtil.PickNamingMethodAndGenerate();
        
            props.GenerateInfoAge(spectralType);
            props.GenerateInfoRadius(spectralType);
            props.GenerateInfoTemperature(spectralType);
            props.GenerateInfoRotation(spectralType);
            props.GenerateInfoMagneticField(spectralType);
            props.Info_Metallicity = new SerializableDictionary<string, float>();
            props.GenerateInfoMetallicity(spectralType);
            props.GenerateInfoIntrinsicVariability(spectralType);
            props.GenerateInfoLuminosity(props.Info_Radius, props.Info_Temperature);
            props.GenerateInfoMass(props.Info_Luminosity);
        
            props.GenerateGOStartingPosition();
            props.GenerateGORadius();
            props.GenerateGOChromaticity(spectralType);
            props.GenerateGOCellColor();
            props.GenerateGOHabitableRangeInner(spectralType);
            props.GenerateGOHabitableRangeOuter(spectralType);
        }
    }
}