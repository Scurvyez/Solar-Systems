using Language;
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
        
        private GameObject _currentPlanet;
        private PlanetInfo _currentPlanetInfo;
        
        private LanguageManager _languageManager;
        private string _name_label;
        private string _mass_label;
        private string _radius_label;
        private string _rot_period_label;
        private string _orb_period_label;
        private string _surf_temp_label;
        private string _surf_pressure_label;
        private string _surf_grav_label;
        private string _esc_vel_label;
        private string _mag_field_strength_label;
        private string _mean_density_label;
        private string _axial_tilt_label;
        private string _num_satellites_label;
        private string _has_atmos_label;
        private string _has_rings_label;
        private string _habitable_label;
        
        private void Start()
        {
            _languageManager = LanguageManager.Instance;
            _name_label = _languageManager.GetValue("obj_name_label");
            _mass_label = _languageManager.GetValue("obj_mass_label");
            _radius_label = _languageManager.GetValue("obj_radius_label");
            _rot_period_label = _languageManager.GetValue("obj_rot_period_label");
            _orb_period_label = _languageManager.GetValue("obj_orb_period_label");
            _surf_temp_label = _languageManager.GetValue("obj_surf_temp_label");
            _surf_pressure_label = _languageManager.GetValue("obj_surf_pressure_label");
            _surf_grav_label = _languageManager.GetValue("obj_surf_grav_label");
            _esc_vel_label = _languageManager.GetValue("obj_esc_vel_label");
            _mag_field_strength_label = _languageManager.GetValue("obj_mag_field_strength_label");
            _mean_density_label = _languageManager.GetValue("obj_mean_density_label");
            _axial_tilt_label = _languageManager.GetValue("obj_axial_tilt_label");
            _num_satellites_label = _languageManager.GetValue("obj_num_satellites_label");
            _has_atmos_label = _languageManager.GetValue("obj_has_atmos_label");
            _has_rings_label = _languageManager.GetValue("obj_has_rings_label");
            _habitable_label = _languageManager.GetValue("obj_habitable_label");
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
                    SetPlanetValues();
                }
                else
                {
                    HideUIElements();
                }
            }
        }
        
        private void SetPlanetValues()
        {
            float massInEarthMasses = _currentPlanetInfo.Mass / ConstantsUtil.EARTH_MASS_KG;

            NameText.text = _name_label + _currentPlanetInfo.Name;
            MassText.text = _mass_label + massInEarthMasses.ToString("F2") + " Earth masses";
            RadiusText.text = _radius_label + _currentPlanetInfo.Info_Radius.ToString("F2") + " kilometers";
            RotationalPeriodText.text = _rot_period_label + _currentPlanetInfo.RotationalPeriod.ToString("F2") + " hours";
            OrbitalPeriodText.text = _orb_period_label + _currentPlanetInfo.OrbitalPeriod.ToString("F2") + " days";
            SurfaceTemperatureText.text = _surf_temp_label + _currentPlanetInfo.SurfaceTemperature.ToString("F2") + " Kelvin";
            SurfacePressureText.text = _surf_pressure_label + _currentPlanetInfo.SurfacePressure.ToString("F2") + " Earth atmospheres";
            SurfaceGravityText.text = _surf_grav_label + _currentPlanetInfo.SurfaceGravity.ToString("F2") + " m/s<sup>2</sup>";
            EscapeVelocityText.text = _esc_vel_label + _currentPlanetInfo.EscapeVelocity.ToString("F2") + " km/s";
            MagneticFieldStrengthText.text = _mag_field_strength_label + _currentPlanetInfo.MagneticFieldStrength.ToString("F2") + " Gauss";
            MeanDensityText.text = _mean_density_label + _currentPlanetInfo.MeanDensity.ToString("F2") + " kg/m<sup>3</sup>";
            AxialTiltText.text = _axial_tilt_label + _currentPlanetInfo.AxialTilt.ToString("F2") + " <sup>O</sup>";
            NumberOfMoonsText.text = _num_satellites_label + _currentPlanetInfo.NumberOfMoons;
            HasAtmosphereText.text = _has_atmos_label + (_currentPlanetInfo.HasAtmosphere ? "Yes" : "No");
            HasRingsText.text = _has_rings_label + (_currentPlanetInfo.HasRings ? "Yes" : "No");
            IsHabitableText.text = _habitable_label + (_currentPlanetInfo.IsHabitable ? "Yes" : "No");
        }
        
        private GameObject GetSelectedPlanet()
        {
            return SystemCamera?.SelectedObject;
        }
        
        private void ShowUIElements()
        {
            UI_SelectedPlanet.SetActive(true);
        }
        
        private void HideUIElements()
        {
            UI_SelectedPlanet.SetActive(false);
        }
    }
}
