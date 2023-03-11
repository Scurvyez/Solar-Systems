using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabitableZoneDisplay : MonoBehaviour
{
    public GameObject starPrefab;
    public GameObject habitableZoneOuterPrefab;
    public GameObject habitableZoneInnerPrefab;

    private float starLuminosity;
    private float starTemperature;

    private const float solLuminosity = 3.828f;
    private const float solEffTemperature = 5780.0f;

    private float HabitableRangeInner { get; set; }
    private float HabitableRangeOuter { get; set; }

    public void Start()
    {
        // Get a reference to the parent stars' class
        string spectralClass = SaveManager.instance.activeSave.starClassAsString;
        HabitableRangeInner = HabitableZoneInner(spectralClass);
        HabitableRangeOuter = HabitableZoneOuter(spectralClass);

        starLuminosity = (float)SaveManager.instance.activeSave.starLuminosity;
        starTemperature = (float)SaveManager.instance.activeSave.starTemperature;

        // Calculate the inner habitable zone boundary
        float zoneInner = Mathf.Sqrt(starLuminosity / solLuminosity) * Mathf.Sqrt(solEffTemperature / starTemperature) * (HabitableRangeInner * 1000.0f);

        // Calculate the outer habitable zone boundary
        float zoneOuter = Mathf.Sqrt(starLuminosity / solLuminosity) * Mathf.Sqrt(solEffTemperature / starTemperature) * (HabitableRangeOuter * 1000.0f);

        // Instantiate the habitable zone outer sprite as a child of the current GameObject
        GameObject habitableZoneOuterMarker = Instantiate(habitableZoneOuterPrefab, transform);
        // Set the scale of the habitable zone outer sprite to match the size of the habitable zone
        habitableZoneOuterMarker.transform.localScale = new Vector3(zoneOuter, zoneOuter, 1f);

        // Instantiate the habitable zone inner sprite as a child of the current GameObject
        GameObject habitableZoneInnerMarker = Instantiate(habitableZoneInnerPrefab, transform);
        // Set the scale of the habitable zone inner sprite to match the size of the habitable zone
        habitableZoneInnerMarker.transform.localScale = new Vector3(zoneInner, zoneInner, 1f);

        // Rotate the habitable zone outer sprite to cover the x plane
        habitableZoneOuterMarker.transform.Rotate(new Vector3(90f, 0f, 0f));
        // Rotate the habitable zone inner sprite to cover the x plane
        habitableZoneInnerMarker.transform.Rotate(new Vector3(90f, 0f, 0f));
    }

    public float HabitableZoneInner(string spectralClass)
    {
        HabitableRangeInner = spectralClass switch
        {
            "O" => 0.0f,
            "B" => 2.2f,
            "A" => 1.6f,
            "F" => 1.1f,
            "G" => 0.95f,
            "K" => 0.37f,
            "M" => 0.08f,
            _ => HabitableRangeInner,
        };
        return HabitableRangeInner;
    }

    public float HabitableZoneOuter(string spectralClass)
    {
        HabitableRangeOuter = spectralClass switch
        {
            "O" => 0.0f,
            "B" => 15.0f,
            "A" => 3.0f,
            "F" => 1.5f,
            "G" => 1.4f,
            "K" => 0.73f,
            "M" => 0.24f,
            _ => HabitableRangeOuter,
        };
        return HabitableRangeOuter;
    }
}
