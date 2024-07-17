using SolarSystem;
using TMPro;
using UnityEngine;

namespace SolarSystemUI
{
    public class UI_MoonInfo : MonoBehaviour
    {
        public StaticAngledCamera SystemCamera;
        public GameObject UI_SelectedMoon;
        
        [Header("Moon Info UI Elements")]
        public TextMeshProUGUI ParentPlanetNameText;
        public TextMeshProUGUI NameText;
        public TextMeshProUGUI RadiusText;
        public TextMeshProUGUI OrbitalPeriodText;
        public TextMeshProUGUI RotationalPeriodText;
        public TextMeshProUGUI AxialTiltText;
        public TextMeshProUGUI SurfaceTemperatureText;
        public TextMeshProUGUI MeanDensityText;
        public TextMeshProUGUI SurfaceGravityText;
        public TextMeshProUGUI EscapeVelocityText;
        public TextMeshProUGUI HasAtmosphereText;
        public TextMeshProUGUI IsHabitableText;
        
        private TextMeshProUGUI[] _textElements;
        private GameObject _currentMoon;
        private MoonInfo _currentMoonInfo;

        private void Start()
        {
            PopulateTextElementsArray();
        }
        
        private void Update()
        {
            GameObject selectedMoon = GetSelectedStar();

            if (selectedMoon != _currentMoon)
            {
                _currentMoon = selectedMoon;
                _currentMoonInfo = _currentMoon?.GetComponent<MoonInfo>();
                
                if (_currentMoonInfo is not null)
                {
                    ShowUIElements();
                    SetSavedStarValues();
                }
                else
                {
                    HideUIElements();
                }
            }
            else if (_currentMoonInfo is not null && StarInfoChanged())
            {
                SetSavedStarValues();
            }
        }
        
        private void SetSavedStarValues()
        {
            ParentPlanetNameText.text = "<color=#ff8f8f>Parent Planet Name:</color> " + _currentMoonInfo.ParentPlanetName;
            NameText.text = "<color=#ff8f8f>Name:</color> " + _currentMoonInfo.Name;
            RadiusText.text = "<color=#ff8f8f>Radius:</color> " + _currentMoonInfo.Radius.ToString("F2");
            OrbitalPeriodText.text = "<color=#ff8f8f>Orbital Period:</color> " + _currentMoonInfo.OrbitalPeriod.ToString("F2");
            RotationalPeriodText.text = "<color=#ff8f8f>Rotational Period:</color> " + _currentMoonInfo.RotationalPeriod.ToString("F2");
            AxialTiltText.text = "<color=#ff8f8f>Axial Tilt:</color> " + _currentMoonInfo.AxialTilt.ToString("F2");
            SurfaceTemperatureText.text = "<color=#ff8f8f>Surface Temperature:</color> " + _currentMoonInfo.SurfaceTemperature.ToString("F2");
            MeanDensityText.text = "<color=#ff8f8f>Mean Density:</color> " + _currentMoonInfo.MeanDensity.ToString("F2");
            SurfaceGravityText.text = "<color=#ff8f8f>Surface Gravity:</color> " + _currentMoonInfo.SurfaceGravity.ToString("F2");
            EscapeVelocityText.text = "<color=#ff8f8f>Escape Velocity:</color> " + _currentMoonInfo.EscapeVelocity.ToString("F2");
            HasAtmosphereText.text = "<color=#ff8f8f>Has Atmosphere:</color> " + _currentMoonInfo.HasAtmosphere;
            IsHabitableText.text = "<color=#ff8f8f>Is Habitable:</color> " + _currentMoonInfo.IsHabitable;
        }

        private GameObject GetSelectedStar()
        {
            return SystemCamera?.SelectedObject;
        }

        private void PopulateTextElementsArray()
        {
            _textElements = new [] 
            {
                ParentPlanetNameText,
                NameText,
                RadiusText,
                OrbitalPeriodText,
                RotationalPeriodText,
                AxialTiltText,
                SurfaceTemperatureText,
                MeanDensityText,
                SurfaceGravityText,
                EscapeVelocityText,
                HasAtmosphereText,
                IsHabitableText
            };
        }
        
        private void ShowUIElements()
        {
            UI_SelectedMoon.SetActive(true);
        }

        private void HideUIElements()
        {
            UI_SelectedMoon.SetActive(false);
        }

        private bool StarInfoChanged()
        {
            if (_currentMoonInfo is null) return false;

            return ParentPlanetNameText.text != "<color=#ff8f8f>Parent Planet Name:</color> " + _currentMoonInfo.ParentPlanetName ||
                   NameText.text != "<color=#ff8f8f>Name:</color> " + _currentMoonInfo.Name ||
                   RadiusText.text != "<color=#ff8f8f>Radius:</color> " + _currentMoonInfo.Radius.ToString("F2") ||
                   OrbitalPeriodText.text != "<color=#ff8f8f>Orbital Period:</color> " + _currentMoonInfo.OrbitalPeriod.ToString("F2") ||
                   RotationalPeriodText.text != "<color=#ff8f8f>Rotational Period:</color> " + _currentMoonInfo.RotationalPeriod.ToString("F2") ||
                   AxialTiltText.text != "<color=#ff8f8f>Axial Tilt:</color> " + _currentMoonInfo.AxialTilt.ToString("F2") ||
                   SurfaceTemperatureText.text != "<color=#ff8f8f>Surface Temperature:</color> " + _currentMoonInfo.SurfaceTemperature.ToString("F2") ||
                   MeanDensityText.text != "<color=#ff8f8f>Mean Density:</color> " + _currentMoonInfo.MeanDensity.ToString("F2") ||
                   SurfaceGravityText.text != "<color=#ff8f8f>Surface Gravity:</color> " + _currentMoonInfo.SurfaceGravity.ToString("F2") ||
                   EscapeVelocityText.text != "<color=#ff8f8f>Escape Velocity:</color> " + _currentMoonInfo.EscapeVelocity.ToString("F2") ||
                   HasAtmosphereText.text != "<color=#ff8f8f>Has Atmosphere:</color> " + _currentMoonInfo.HasAtmosphere ||
                   IsHabitableText.text != "<color=#ff8f8f>Is Habitable:</color> " + _currentMoonInfo.IsHabitable;
        }
    }
}