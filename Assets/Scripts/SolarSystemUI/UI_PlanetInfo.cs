using SolarSystem;
using TMPro;
using UnityEngine;
using Utils;

namespace SolarSystemUI
{
    public class UI_PlanetInfo : MonoBehaviour
    {
        public StaticAngledCamera SystemCamera;
        public GameObject UI_SelectedPlanet;
        
        [Header("Planet Info UI Elements")]
        public TextMeshProUGUI NameText;
        public TextMeshProUGUI MassText;
        public TextMeshProUGUI RadiusText;
        public TextMeshProUGUI RotationalPeriodText;
        public TextMeshProUGUI OrbitalPeriodText;
        public TextMeshProUGUI SurfaceTemperatureText;
        public TextMeshProUGUI SurfacePressureText;
        public TextMeshProUGUI SurfaceGravityText;
        public TextMeshProUGUI EscapeVelocityText;
        public TextMeshProUGUI MagneticFieldStrengthText;
        public TextMeshProUGUI MeanDensityText;
        public TextMeshProUGUI AxialTiltText;
        public TextMeshProUGUI NumberOfMoonsText;
        public TextMeshProUGUI HasAtmosphereText;
        public TextMeshProUGUI HasRingsText;
        public TextMeshProUGUI IsHabitableText;
        
        private TextMeshProUGUI[] _textElements;
        private GameObject _currentPlanet;
        private PlanetInfo _currentPlanetInfo;

        private void Start()
        {
            
            PopulateTextElementsArray();
        }
        
        private void Update()
        {
            GameObject selectedPlanet = GetSelectedPlanet();

            if (selectedPlanet != _currentPlanet)
            {
                _currentPlanet = selectedPlanet;
                _currentPlanetInfo = _currentPlanet?.GetComponent<PlanetInfo>();
                
                if (_currentPlanetInfo is not null)
                {
                    ShowUIElements();
                    SetSavedPlanetValues();
                }
                else
                {
                    HideUIElements();
                }
            }
            else if (_currentPlanetInfo is not null && PlanetInfoChanged())
            {
                SetSavedPlanetValues();
            }
        }
        
        private void SetSavedPlanetValues()
        {
            float massInEarthMasses = _currentPlanetInfo.Mass / ConstantsUtil.EARTH_MASS_KG;
            
            NameText.text = "<color=#ff8f8f>Name:</color> " + _currentPlanetInfo.Name;
            MassText.text = "<color=#ff8f8f>Mass:</color> " + massInEarthMasses.ToString("F2")  + " Earth masses";
            RadiusText.text = "<color=#ff8f8f>Radius:</color> " + _currentPlanetInfo.Info_Radius.ToString("F2") + " kilometers";
            RotationalPeriodText.text = "<color=#ff8f8f>Rotational Period:</color> " + _currentPlanetInfo.RotationalPeriod.ToString("F2") + " hours";
            OrbitalPeriodText.text = "<color=#ff8f8f>Orbital Period:</color> " + _currentPlanetInfo.OrbitalPeriod.ToString("F2") + " days";
            SurfaceTemperatureText.text = "<color=#ff8f8f>Surface Temperature:</color> " + _currentPlanetInfo.SurfaceTemperature.ToString("F2") + " Kelvin";
            SurfacePressureText.text = "<color=#ff8f8f>Surface Pressure:</color> " + _currentPlanetInfo.SurfacePressure.ToString("F2") + " Earth atmospheres";
            SurfaceGravityText.text = "<color=#ff8f8f>Surface Gravity:</color> " + _currentPlanetInfo.SurfaceGravity.ToString("F2") + " m/s<sup>2</sup>";
            EscapeVelocityText.text = "<color=#ff8f8f>Escape Velocity:</color> " + _currentPlanetInfo.EscapeVelocity.ToString("F2") + " km/s";
            MagneticFieldStrengthText.text = "<color=#ff8f8f>Magnetic Field Strength:</color> " + _currentPlanetInfo.MagneticFieldStrength.ToString("F2") + " Gauss";
            MeanDensityText.text = "<color=#ff8f8f>Mean Density:</color> " + _currentPlanetInfo.MeanDensity.ToString("F2") + " kg/m<sup>3</sup>";
            AxialTiltText.text = "<color=#ff8f8f>Axial Tilt:</color> " + _currentPlanetInfo.AxialTilt.ToString("F2") + " degrees";
            NumberOfMoonsText.text = "<color=#ff8f8f>Number of Moons:</color> " + _currentPlanetInfo.NumberOfMoons;
            HasAtmosphereText.text = "<color=#ff8f8f>Has Atmosphere:</color> " + _currentPlanetInfo.HasAtmosphere;
            HasRingsText.text = "<color=#ff8f8f>Has Rings:</color> " + _currentPlanetInfo.HasRings;
            IsHabitableText.text = "<color=#ff8f8f>Is Habitable:</color> " + _currentPlanetInfo.IsHabitable;
        }

        private GameObject GetSelectedPlanet()
        {
            return SystemCamera?.SelectedObject;
        }
        
        private void PopulateTextElementsArray()
        {
            _textElements = new [] 
            { 
                NameText,
                MassText,
                RadiusText,
                RotationalPeriodText,
                OrbitalPeriodText,
                SurfaceTemperatureText,
                SurfacePressureText,
                SurfaceGravityText,
                EscapeVelocityText,
                MagneticFieldStrengthText,
                MeanDensityText,
                AxialTiltText,
                NumberOfMoonsText,
                HasAtmosphereText,
                HasRingsText,
                IsHabitableText
            };
        }
        
        private void ShowUIElements()
        {
            UI_SelectedPlanet.SetActive(true);
        }

        private void HideUIElements()
        {
            UI_SelectedPlanet.SetActive(false);
        }

        private bool PlanetInfoChanged()
        {
            if (_currentPlanetInfo is null) return false;

            float massInEarthMasses = _currentPlanetInfo.Mass / ConstantsUtil.EARTH_MASS_KG;
            
            return NameText.text != "<color=#ff8f8f>Name:</color> " + _currentPlanetInfo.Name ||
                   MassText.text != "<color=#ff8f8f>Mass:</color> " + massInEarthMasses.ToString("F2")  + " Earth masses" ||
                   RadiusText.text != "<color=#ff8f8f>Radius:</color> " + _currentPlanetInfo.Info_Radius.ToString("F2") + " kilometers" ||
                   RotationalPeriodText.text != "<color=#ff8f8f>Rotational Period:</color> " + _currentPlanetInfo.RotationalPeriod.ToString("F2") + " hours" ||
                   OrbitalPeriodText.text != "<color=#ff8f8f>Orbital Period:</color> " + _currentPlanetInfo.OrbitalPeriod.ToString("F2") + " days" ||
                   SurfaceTemperatureText.text != "<color=#ff8f8f>Surface Temperature:</color> " + _currentPlanetInfo.SurfaceTemperature.ToString("F2") + " Kelvin" ||
                   SurfacePressureText.text != "<color=#ff8f8f>Surface Pressure:</color> " + _currentPlanetInfo.SurfacePressure.ToString("F2") + " Earth atmospheres" ||
                   SurfaceGravityText.text != "<color=#ff8f8f>Surface Gravity:</color> " + _currentPlanetInfo.SurfaceGravity.ToString("F2") + " m/s<sup>2</sup>" ||
                   EscapeVelocityText.text != "<color=#ff8f8f>Escape Velocity:</color> " + _currentPlanetInfo.EscapeVelocity.ToString("F2") + " km/s" ||
                   MagneticFieldStrengthText.text != "<color=#ff8f8f>Magnetic Field Strength:</color> " + _currentPlanetInfo.MagneticFieldStrength.ToString("F2") + " Gauss" ||
                   MeanDensityText.text != "<color=#ff8f8f>Mean Density:</color> " + _currentPlanetInfo.MeanDensity.ToString("F2") + " kg/m<sup>3</sup>" ||
                   AxialTiltText.text != "<color=#ff8f8f>Axial Tilt:</color> " + _currentPlanetInfo.AxialTilt.ToString("F2") + " degrees" ||
                   NumberOfMoonsText.text != "<color=#ff8f8f>Number of Moons:</color> " + _currentPlanetInfo.NumberOfMoons ||
                   HasAtmosphereText.text != "<color=#ff8f8f>Has Atmosphere:</color> " + _currentPlanetInfo.HasAtmosphere ||
                   HasRingsText.text != "<color=#ff8f8f>Has Rings:</color> " + _currentPlanetInfo.HasRings ||
                   IsHabitableText.text != "<color=#ff8f8f>Is Habitable:</color> " + _currentPlanetInfo.IsHabitable;
        }
    }
}
