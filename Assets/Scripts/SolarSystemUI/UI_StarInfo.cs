using Language;
using SolarSystem;
using TMPro;
using UnityEngine;
using Utils;

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
        public TextMeshProUGUI VariabilityText;
        
        private GameObject _currentStar;
        private StarInfo _currentStarInfo;
        
        private LanguageManager _languageManager;
        private string _unknownValue;
        private string _sys_name_label;
        private string _sys_age_label;
        private string _b_years_old;
        private string _m_years_old;
        private string _sys_class_label;
        private string _star_mass_label;
        private string _star_radius_label;
        private string _star_lumin_label;
        private string _star_surf_temp_label;
        private string _star_rot_period_label;
        private string _star_mag_field_strength_label;
        private string _star_variability_label;
        
        private void Start()
        {
            _languageManager = LanguageManager.Instance;
            _unknownValue = _languageManager.GetValue("unknown");
            _sys_name_label = _languageManager.GetValue("sys_name_label");
            _sys_age_label = _languageManager.GetValue("sys_age_label");
            _b_years_old = _languageManager.GetValue("b_years_old");
            _m_years_old = _languageManager.GetValue("m_years_old");
            _sys_class_label = _languageManager.GetValue("sys_class_label");
            _star_mass_label = _languageManager.GetValue("obj_mass_label");
            _star_radius_label = _languageManager.GetValue("obj_radius_label");
            _star_lumin_label = _languageManager.GetValue("obj_lumin_label");
            _star_surf_temp_label = _languageManager.GetValue("obj_surf_temp_label");
            _star_rot_period_label = _languageManager.GetValue("obj_rot_period_label");
            _star_mag_field_strength_label = _languageManager.GetValue("obj_mag_field_strength_label");
            _star_variability_label = _languageManager.GetValue("obj_variability_label");
        }
        
        private void Update()
        {
            GameObject selectedStar = GetSelectedStar();

            if (selectedStar == _currentStar) return;
            _currentStar = selectedStar;
            _currentStarInfo = _currentStar?.GetComponent<StarInfo>();

            if (_currentStarInfo is not null)
            {
                ShowUIElements();
                SetStarValues();
            }
            else
            {
                HideUIElements();
            }
        }
        
        private void SetStarValues()
        {
            ClassText.text = _sys_class_label + _currentStarInfo.Class;
            AgeText.text = FormatAgeText(_currentStarInfo.Age);
            NameText.text = _sys_name_label + _currentStarInfo.Name;
            MassText.text = _star_mass_label + _currentStarInfo.Mass.ToString("F2") + " M<sub>O</sub>";
            RadiusText.text = _star_radius_label + _currentStarInfo.Radius.ToString("F2") + " R<sub>O</sub>";
            LuminosityText.text = _star_lumin_label + _currentStarInfo.Luminosity.ToString("F2") + " L<sub>O</sub>";
            SurfaceTemperatureText.text = _star_surf_temp_label + _currentStarInfo.SurfaceTemperature.ToString("F2") + " Kelvin";
            RotationalPeriodText.text = _star_rot_period_label + _currentStarInfo.RotationalPeriod.ToString("F2") + " hours";
            MagneticFieldStrengthText.text = _star_mag_field_strength_label + _currentStarInfo.MagneticFieldStrength.ToString("F5") + " Gauss";
            VariabilityText.text = _star_variability_label + _currentStarInfo.Variability.ToString("F2");
        }
        
        private GameObject GetSelectedStar()
        {
            return SystemCamera?.SelectedObject;
        }
        
        private void ShowUIElements()
        {
            UI_SelectedStar.SetActive(true);
        }
        
        private void HideUIElements()
        {
            UI_SelectedStar.SetActive(false);
        }
        
        private string FormatAgeText(double age)
        {
            if (age <= 0)
            {
                return _sys_age_label + _unknownValue;
            }
            return age >= ConstantsUtil.BILLION 
                ? _sys_age_label + (age / ConstantsUtil.BILLION).ToString("F2") + _b_years_old 
                : _sys_age_label + (age / ConstantsUtil.MILLION).ToString("F2") + _m_years_old;
        }
    }
}
