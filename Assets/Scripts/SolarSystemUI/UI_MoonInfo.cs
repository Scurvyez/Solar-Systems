using Language;
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
        
        private GameObject _currentMoon;
        private MoonInfo _currentMoonInfo;
        
        private LanguageManager _languageManager;
        private string _parent_planet_name_label;
        private string _name_label;
        private string _radius_label;
        private string _rot_period_label;
        private string _orb_period_label;
        private string _axial_tilt_label;
        private string _surf_temp_label;
        private string _surf_grav_label;
        private string _mean_density_label;
        private string _esc_vel_label;
        private string _has_atmos_label;
        private string _habitable_label;
        
        private void Start()
        {
            _languageManager = LanguageManager.Instance;
            _parent_planet_name_label = _languageManager.GetValue("obj_moon_parent_name_label");
            _name_label = _languageManager.GetValue("obj_name_label");
            _radius_label = _languageManager.GetValue("obj_radius_label");
            _rot_period_label = _languageManager.GetValue("obj_rot_period_label");
            _orb_period_label = _languageManager.GetValue("obj_orb_period_label");
            _axial_tilt_label = _languageManager.GetValue("obj_axial_tilt_label");
            _surf_temp_label = _languageManager.GetValue("obj_surf_temp_label");
            _surf_grav_label = _languageManager.GetValue("obj_surf_grav_label");
            _mean_density_label = _languageManager.GetValue("obj_mean_density_label");
            _esc_vel_label = _languageManager.GetValue("obj_esc_vel_label");
            _has_atmos_label = _languageManager.GetValue("obj_has_atmos_label");
            _habitable_label = _languageManager.GetValue("obj_habitable_label");
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
                    SetMoonValues();
                }
                else
                {
                    HideUIElements();
                }
            }
        }
        
        private void SetMoonValues()
        {
            ParentPlanetNameText.text = _parent_planet_name_label + _currentMoonInfo.ParentPlanetName;
            NameText.text = _name_label + _currentMoonInfo.Name;
            RadiusText.text = _radius_label + _currentMoonInfo.Radius.ToString("F2") + " kilometers";
            OrbitalPeriodText.text = _orb_period_label + _currentMoonInfo.OrbitalPeriod.ToString("F2") + " days";
            RotationalPeriodText.text = _rot_period_label + _currentMoonInfo.RotationalPeriod.ToString("F2") + " hours";
            AxialTiltText.text = _axial_tilt_label + _currentMoonInfo.AxialTilt.ToString("F2") + " <sup>O</sup>";
            SurfaceTemperatureText.text = _surf_temp_label + _currentMoonInfo.SurfaceTemperature.ToString("F2") + " Kelvin";
            MeanDensityText.text = _mean_density_label + _currentMoonInfo.MeanDensity.ToString("F2") + " kg/m<sup>3</sup>";
            SurfaceGravityText.text = _surf_grav_label + _currentMoonInfo.SurfaceGravity.ToString("F2") + " m/s<sup>2</sup>";
            EscapeVelocityText.text = _esc_vel_label + _currentMoonInfo.EscapeVelocity.ToString("F2") + " km/s";
            HasAtmosphereText.text = _has_atmos_label + (_currentMoonInfo.HasAtmosphere ? "Yes" : "No");
            IsHabitableText.text = _habitable_label + (_currentMoonInfo.IsHabitable ? "Yes" : "No");
        }

        private GameObject GetSelectedStar()
        {
            return SystemCamera?.SelectedObject;
        }

        private void ShowUIElements()
        {
            UI_SelectedMoon.SetActive(true);
        }

        private void HideUIElements()
        {
            UI_SelectedMoon.SetActive(false);
        }
    }
}