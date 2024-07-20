using Language;
using Saving;
using UnityEngine;
using TMPro;
using Utils;

public class MenuSystemCreation : MonoBehaviour
{
    public TextMeshProUGUI SystemNameText;
    public TextMeshProUGUI SystemAgeText;
    public TextMeshProUGUI StellarClassText;
    public TextMeshProUGUI PlanetsCountText;
    public TextMeshProUGUI MoonsCountText;

    private LanguageManager _languageManager;
    private string _unknownValue;
    private string _sys_name_label;
    private string _sys_age_label;
    private string _b_years_old;
    private string _m_years_old;
    private string _sys_class_label;
    private string _msc_num_planets;
    private string _msc_num_moons;

    public void Start()
    {
        _languageManager = LanguageManager.Instance;
        _unknownValue = _languageManager.GetValue("unknown");
        _sys_name_label = _languageManager.GetValue("sys_name_label");
        _sys_age_label = _languageManager.GetValue("sys_age_label");
        _b_years_old = _languageManager.GetValue("b_years_old");
        _m_years_old = _languageManager.GetValue("m_years_old");
        _sys_class_label = _languageManager.GetValue("sys_class_label");
        _msc_num_planets = _languageManager.GetValue("msc_num_planets");
        _msc_num_moons = _languageManager.GetValue("msc_num_moons");
        UpdateStarData();
    }

    private void Update()
    {
        UpdateStarData();
    }
    
    private void UpdateStarData()
    {
        string systemName = _sys_name_label + (string.IsNullOrEmpty(SaveManager.Instance.ActiveSave.starSystemName) 
            ? _unknownValue : SaveManager.Instance.ActiveSave.starSystemName);
        
        string systemAge = FormatAgeText(SaveManager.Instance.ActiveSave.starAge);
        
        string stellarClass = _sys_class_label + (string.IsNullOrEmpty(SaveManager.Instance.ActiveSave.starClassAsString) 
            ? _unknownValue : SaveManager.Instance.ActiveSave.starClassAsString);
        
        string planetsCount = _msc_num_planets + ((SaveManager.Instance.ActiveSave.planetCount <= 0) 
            ? _unknownValue : SaveManager.Instance.ActiveSave.planetCount.ToString());
        
        string moonsCount = _msc_num_moons + ((SaveManager.Instance.ActiveSave.moons.Count <= 0) 
            ? _unknownValue : SaveManager.Instance.ActiveSave.moons.Count.ToString());

        SystemNameText.text = systemName;
        SystemAgeText.text = systemAge;
        StellarClassText.text = stellarClass;
        PlanetsCountText.text = planetsCount;
        MoonsCountText.text = moonsCount;
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
