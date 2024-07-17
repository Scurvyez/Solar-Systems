using SolarSystem;
using TMPro;
using UnityEngine;

namespace SolarSystemUI
{
    public class UI_StarInfo : MonoBehaviour
    {
        public StaticAngledCamera SystemCamera;
        public GameObject UI_SelectedStar;
        
        [Header("Star Info UI Elements")]
        public TextMeshProUGUI ClassText;
        public TextMeshProUGUI NameText;
        public TextMeshProUGUI AgeText;
        public TextMeshProUGUI MassText;
        public TextMeshProUGUI RadiusText;
        public TextMeshProUGUI LuminosityText;
        public TextMeshProUGUI SurfaceTemperatureText;
        public TextMeshProUGUI RotationalPeriodText;
        public TextMeshProUGUI MagneticFieldStrengthText;
        public TextMeshProUGUI HasIntrinsicVariabilityText;
        public TextMeshProUGUI HasExtrinsicVariabilityText;
        public TextMeshProUGUI VariabilityText;
        
        private TextMeshProUGUI[] _textElements;
        private GameObject _currentStar;
        private StarInfo _currentStarInfo;

        private void Start()
        {
            PopulateTextElementsArray();
        }
        
        private void Update()
        {
            GameObject selectedStar = GetSelectedStar();

            if (selectedStar != _currentStar)
            {
                _currentStar = selectedStar;
                _currentStarInfo = _currentStar?.GetComponent<StarInfo>();
                
                if (_currentStarInfo is not null)
                {
                    ShowUIElements();
                    SetSavedStarValues();
                }
                else
                {
                    HideUIElements();
                }
            }
            else if (_currentStarInfo is not null && StarInfoChanged())
            {
                SetSavedStarValues();
            }
        }
        
        private void SetSavedStarValues()
        {
            ClassText.text = "<color=#ff8f8f>Class:</color> " + _currentStarInfo.Class;
            AgeText.text = "<color=#ff8f8f>Age:</color> " + _currentStarInfo.Age + " years";
            NameText.text = "<color=#ff8f8f>Name:</color> " + _currentStarInfo.Name;
            MassText.text = "<color=#ff8f8f>Mass:</color> " + _currentStarInfo.Mass.ToString("F2");
            RadiusText.text = "<color=#ff8f8f>Radius:</color> " + _currentStarInfo.Radius.ToString("F2");
            LuminosityText.text = "<color=#ff8f8f>Luminosity:</color> " + _currentStarInfo.Luminosity.ToString("F2");
            SurfaceTemperatureText.text = "<color=#ff8f8f>Surface Temperature:</color> " + _currentStarInfo.SurfaceTemperature.ToString("F2");
            RotationalPeriodText.text = "<color=#ff8f8f>Rotational Period:</color> " + _currentStarInfo.RotationalPeriod.ToString("F2");
            MagneticFieldStrengthText.text = "<color=#ff8f8f>Magnetic Field Strength:</color> " + _currentStarInfo.MagneticFieldStrength.ToString("F5");
            HasIntrinsicVariabilityText.text = "<color=#ff8f8f>Has Intrinsic Variability:</color> " + _currentStarInfo.HasIntrinsicVariability;
            HasExtrinsicVariabilityText.text = "<color=#ff8f8f>Has Extrinsic Variability:</color> " + _currentStarInfo.HasExtrinsicVariability;
            VariabilityText.text = "<color=#ff8f8f>Variability:</color> " + _currentStarInfo.Variability.ToString("F2");
        }

        private GameObject GetSelectedStar()
        {
            return SystemCamera?.SelectedObject;
        }

        private void PopulateTextElementsArray()
        {
            _textElements = new [] 
            {
                ClassText,
                AgeText,
                NameText,
                MassText,
                RadiusText,
                LuminosityText,
                SurfaceTemperatureText,
                RotationalPeriodText,
                MagneticFieldStrengthText,
                HasIntrinsicVariabilityText,
                HasExtrinsicVariabilityText,
                VariabilityText
            };
        }
        
        private void ShowUIElements()
        {
            UI_SelectedStar.SetActive(true);
        }

        private void HideUIElements()
        {
            UI_SelectedStar.SetActive(false);
        }

        private bool StarInfoChanged()
        {
            if (_currentStarInfo is null) return false;

            return ClassText.text != "<color=#ff8f8f>Class:</color> " + _currentStarInfo.Class ||
                   AgeText.text != "<color=#ff8f8f>Age:</color> " + _currentStarInfo.Age ||
                   NameText.text != "<color=#ff8f8f>Name:</color> " + _currentStarInfo.Name ||
                   MassText.text != "<color=#ff8f8f>Mass:</color> " + _currentStarInfo.Mass.ToString("F2") ||
                   RadiusText.text != "<color=#ff8f8f>Radius:</color> " + _currentStarInfo.Radius.ToString("F2") ||
                   LuminosityText.text != "<color=#ff8f8f>Luminosity:</color> " + _currentStarInfo.Luminosity.ToString("F2") ||
                   SurfaceTemperatureText.text != "<color=#ff8f8f>Surface Temperature:</color> " + _currentStarInfo.SurfaceTemperature.ToString("F2") ||
                   RotationalPeriodText.text != "<color=#ff8f8f>Rotational Period:</color> " + _currentStarInfo.RotationalPeriod.ToString("F2") ||
                   MagneticFieldStrengthText.text != "<color=#ff8f8f>Magnetic Field Strength:</color> " + _currentStarInfo.MagneticFieldStrength.ToString("F5") ||
                   HasIntrinsicVariabilityText.text != "<color=#ff8f8f>Has Intrinsic Variability:</color> " + _currentStarInfo.HasIntrinsicVariability ||
                   HasExtrinsicVariabilityText.text != "<color=#ff8f8f>Has Extrinsic Variability:</color> " + _currentStarInfo.HasExtrinsicVariability ||
                   VariabilityText.text != "<color=#ff8f8f>Variability:</color> " + _currentStarInfo.Variability;
        }
    }
}
