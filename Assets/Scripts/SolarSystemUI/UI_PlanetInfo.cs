using SolarSystem;
using TMPro;
using UnityEngine;

namespace SolarSystemUI
{
    public class UI_PlanetInfo : MonoBehaviour
    {
        public StaticAngledCamera SystemCamera;
        
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
            NameText.text = "<color=#ff8f8f>Name:</color> " + _currentPlanetInfo.Name;
            MassText.text = "<color=#ff8f8f>Mass:</color> " + _currentPlanetInfo.Mass;
            RadiusText.text = "<color=#ff8f8f>Radius:</color> " + _currentPlanetInfo.Radius;
            RotationalPeriodText.text = "<color=#ff8f8f>Rotational Period:</color> " + _currentPlanetInfo.RotationalPeriod;
            OrbitalPeriodText.text = "<color=#ff8f8f>Orbital Period:</color> " + _currentPlanetInfo.OrbitalPeriod;
            SurfaceTemperatureText.text = "<color=#ff8f8f>Surface Temperature:</color> " + _currentPlanetInfo.SurfaceTemperature;
            SurfacePressureText.text = "<color=#ff8f8f>Surface Pressure:</color> " + _currentPlanetInfo.SurfacePressure;
            SurfaceGravityText.text = "<color=#ff8f8f>Surface Gravity:</color> " + _currentPlanetInfo.SurfaceGravity;
            EscapeVelocityText.text = "<color=#ff8f8f>Escape Velocity:</color> " + _currentPlanetInfo.EscapeVelocity;
            MagneticFieldStrengthText.text = "<color=#ff8f8f>Magnetic Field Strength:</color> " + _currentPlanetInfo.MagneticFieldStrength;
            MeanDensityText.text = "<color=#ff8f8f>Mean Density:</color> " + _currentPlanetInfo.MeanDensity;
            AxialTiltText.text = "<color=#ff8f8f>Axial Tilt:</color> " + _currentPlanetInfo.AxialTilt;
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
            foreach (TextMeshProUGUI textElement in _textElements)
            {
                textElement.gameObject.SetActive(true);
            }
        }

        private void HideUIElements()
        {
            foreach (TextMeshProUGUI textElement in _textElements)
            {
                textElement.gameObject.SetActive(false);
            }
        }

        private bool PlanetInfoChanged()
        {
            if (_currentPlanetInfo is null) return false;

            return NameText.text != "<color=#ff8f8f>Name:</color> " + _currentPlanetInfo.Name ||
                   MassText.text != "<color=#ff8f8f>Mass:</color> " + _currentPlanetInfo.Mass ||
                   RadiusText.text != "<color=#ff8f8f>Radius:</color> " + _currentPlanetInfo.Radius ||
                   RotationalPeriodText.text != "<color=#ff8f8f>Rotational Period:</color> " + _currentPlanetInfo.RotationalPeriod ||
                   OrbitalPeriodText.text != "<color=#ff8f8f>Orbital Period:</color> " + _currentPlanetInfo.OrbitalPeriod ||
                   SurfaceTemperatureText.text != "<color=#ff8f8f>Surface Temperature:</color> " + _currentPlanetInfo.SurfaceTemperature ||
                   SurfacePressureText.text != "<color=#ff8f8f>Surface Pressure:</color> " + _currentPlanetInfo.SurfacePressure ||
                   SurfaceGravityText.text != "<color=#ff8f8f>Surface Gravity:</color> " + _currentPlanetInfo.SurfaceGravity ||
                   EscapeVelocityText.text != "<color=#ff8f8f>Escape Velocity:</color> " + _currentPlanetInfo.EscapeVelocity ||
                   MagneticFieldStrengthText.text != "<color=#ff8f8f>Magnetic Field Strength:</color> " + _currentPlanetInfo.MagneticFieldStrength ||
                   MeanDensityText.text != "<color=#ff8f8f>Mean Density:</color> " + _currentPlanetInfo.MeanDensity ||
                   AxialTiltText.text != "<color=#ff8f8f>Axial Tilt:</color> " + _currentPlanetInfo.AxialTilt ||
                   NumberOfMoonsText.text != "<color=#ff8f8f>Number of Moons:</color> " + _currentPlanetInfo.NumberOfMoons ||
                   HasAtmosphereText.text != "<color=#ff8f8f>Has Atmosphere:</color> " + _currentPlanetInfo.HasAtmosphere ||
                   HasRingsText.text != "<color=#ff8f8f>Has Rings:</color> " + _currentPlanetInfo.HasRings ||
                   IsHabitableText.text != "<color=#ff8f8f>Is Habitable:</color> " + _currentPlanetInfo.IsHabitable;
        }
    }
}
