using SolarSystem;
using TMPro;
using UnityEngine;

namespace SolarSystemUI
{
    public class UI_StarInfo : MonoBehaviour
    {
        public StaticAngledCamera SystemCamera;
        
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
            AgeText.text = "<color=#ff8f8f>Age:</color> " + _currentStarInfo.Age;
            NameText.text = "<color=#ff8f8f>Name:</color> " + _currentStarInfo.Name;
            MassText.text = "<color=#ff8f8f>Mass:</color> " + _currentStarInfo.Mass;
            RadiusText.text = "<color=#ff8f8f>Radius:</color> " + _currentStarInfo.Radius;
            LuminosityText.text = "<color=#ff8f8f>Luminosity:</color> " + _currentStarInfo.Luminosity;
            SurfaceTemperatureText.text = "<color=#ff8f8f>Surface Temperature:</color> " + _currentStarInfo.SurfaceTemperature;
            RotationalPeriodText.text = "<color=#ff8f8f>Rotational Period:</color> " + _currentStarInfo.RotationalPeriod;
            HasIntrinsicVariabilityText.text = "<color=#ff8f8f>Has Intrinsic Variability:</color> " + _currentStarInfo.HasIntrinsicVariability;
            HasExtrinsicVariabilityText.text = "<color=#ff8f8f>Has Extrinsic Variability:</color> " + _currentStarInfo.HasExtrinsicVariability;
            MagneticFieldStrengthText.text = "<color=#ff8f8f>Magnetic Field Strength:</color> " + _currentStarInfo.MagneticFieldStrength;
            VariabilityText.text = "<color=#ff8f8f>Variability:</color> " + _currentStarInfo.Variability;
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

        private bool StarInfoChanged()
        {
            if (_currentStarInfo is null) return false;

            return ClassText.text != "<color=#ff8f8f>Class:</color> " + _currentStarInfo.Class ||
                   AgeText.text != "<color=#ff8f8f>Age:</color> " + _currentStarInfo.Age ||
                   NameText.text != "<color=#ff8f8f>Name:</color> " + _currentStarInfo.Name ||
                   MassText.text != "<color=#ff8f8f>Mass:</color> " + _currentStarInfo.Mass ||
                   RadiusText.text != "<color=#ff8f8f>Radius:</color> " + _currentStarInfo.Radius ||
                   LuminosityText.text != "<color=#ff8f8f>Luminosity:</color> " + _currentStarInfo.Luminosity ||
                   SurfaceTemperatureText.text != "<color=#ff8f8f>Surface Temperature:</color> " + _currentStarInfo.SurfaceTemperature ||
                   RotationalPeriodText.text != "<color=#ff8f8f>Rotational Period:</color> " + _currentStarInfo.RotationalPeriod ||
                   MagneticFieldStrengthText.text != "<color=#ff8f8f>Magnetic Field Strength:</color> " + _currentStarInfo.MagneticFieldStrength ||
                   HasIntrinsicVariabilityText.text != "<color=#ff8f8f>Has Intrinsic Variability:</color> " + _currentStarInfo.HasIntrinsicVariability ||
                   HasExtrinsicVariabilityText.text != "<color=#ff8f8f>Has Extrinsic Variability:</color> " + _currentStarInfo.HasExtrinsicVariability ||
                   VariabilityText.text != "<color=#ff8f8f>Variability:</color> " + _currentStarInfo.Variability;
        }
    }
}
