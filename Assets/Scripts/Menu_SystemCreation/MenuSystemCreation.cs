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

    private LocalizationManager _locMan;
    private string _unknownValue;
    
    public void Start()
    {
        _locMan = LocalizationManager.Instance;
        _unknownValue = _locMan.GetValue("unknown");
        UpdateStarData();
    }

    private void Update()
    {
        UpdateStarData();
    }
    
    private void UpdateStarData()
    {
        string systemName = "<color=#ff8f8f>System Name:</color> " 
                            + (string.IsNullOrEmpty(SaveManager.Instance.ActiveSave.starSystemName) 
                                ? _unknownValue : SaveManager.Instance.ActiveSave.starSystemName);
        
        string systemAge = FormatAgeText(SaveManager.Instance.ActiveSave.starAge);
        
        string stellarClass = "<color=#ff8f8f>Stellar Class:</color> " 
                              + (string.IsNullOrEmpty(SaveManager.Instance.ActiveSave.starClassAsString) 
                                  ? _unknownValue : SaveManager.Instance.ActiveSave.starClassAsString);
        
        string planetsCount = "<color=#ff8f8f>Number of Planets:</color> " 
                              + ((SaveManager.Instance.ActiveSave.planetCount <= 0) 
                                  ? _unknownValue : SaveManager.Instance.ActiveSave.planetCount.ToString());
        
        string moonsCount = "<color=#ff8f8f>Number of Moons:</color> " 
                              + ((SaveManager.Instance.ActiveSave.moons.Count <= 0) 
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
            return "<color=#ff8f8f>Age: </color>" + _unknownValue;
        }
        return age >= ConstantsUtil.BILLION ? 
            $"<color=#ff8f8f>Age:</color> {(age / ConstantsUtil.BILLION):0.00} " + _locMan.GetValue("b_years_old") : 
            $"<color=#ff8f8f>Age:</color> {(age / ConstantsUtil.MILLION):0.00} " + _locMan.GetValue("m_years_old");
    }
}
