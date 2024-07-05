using Saving;
using UnityEngine;
using TMPro;

public class MenuStarData : MonoBehaviour
{
    public TextMeshProUGUI SystemNameText;
    public TextMeshProUGUI SystemAgeText;
    public TextMeshProUGUI StellarClassText;
    public TextMeshProUGUI PlanetsCountText;
    public TextMeshProUGUI MoonsCountText;

    // Start is called before the first frame update
    public void Start()
    {
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
                                ? "Unknown" : SaveManager.Instance.ActiveSave.starSystemName);
        
        string systemAge = FormatAgeText(SaveManager.Instance.ActiveSave.starAge);
        
        string stellarClass = "<color=#ff8f8f>Stellar Class:</color> " 
                              + (string.IsNullOrEmpty(SaveManager.Instance.ActiveSave.starClassAsString) 
                                  ? "Unknown" : SaveManager.Instance.ActiveSave.starClassAsString);
        
        string planetsCount = "<color=#ff8f8f>Number of Planets:</color> " 
                              + ((SaveManager.Instance.ActiveSave.rockyPlanets.Count <= 0) 
                                  ? "Unknown" : SaveManager.Instance.ActiveSave.rockyPlanets.Count.ToString());
        
        string moonsCount = "<color=#ff8f8f>Number of Moons:</color> " 
                              + ((SaveManager.Instance.ActiveSave.moons.Count <= 0) 
                                  ? "Unknown" : SaveManager.Instance.ActiveSave.moons.Count.ToString());

        SystemNameText.text = systemName;
        SystemAgeText.text = systemAge;
        StellarClassText.text = stellarClass;
        PlanetsCountText.text = planetsCount;
        MoonsCountText.text = moonsCount;
    }
    
    private static string FormatAgeText(double age)
    {
        if (age <= 0)
        {
            return "<color=#ff8f8f>Age:</color> Unknown";
        }
        return age >= 1000000000 ? 
            $"<color=#ff8f8f>Age:</color> {(age / 1000000000f):0.00} billion years old" : 
            $"<color=#ff8f8f>Age:</color> {(age / 1000000f):0.00} million years old";
    }
}
