using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HabitableZoneDisplay : MonoBehaviour
{
    public GameObject starPrefab;
    public GameObject habitableZoneOuterPrefab;
    public GameObject habitableZoneInnerPrefab;
    public float habitableRangeInner;
    public float habitableRangeOuter;
    
    public void Start()
    {
        habitableRangeInner = SaveManager.instance.activeSave.habitableRangeInner;
        habitableRangeOuter = SaveManager.instance.activeSave.habitableRangeOuter;

        // Instantiate the habitable zone outer sprite as a child of the current GameObject
        GameObject habitableZoneOuterMarker = Instantiate(habitableZoneOuterPrefab, transform);
        // Set the scale of the habitable zone outer sprite to match the size of the habitable zone
        habitableZoneOuterMarker.transform.localScale = new Vector3(habitableRangeOuter, habitableRangeOuter, 1f);

        // Instantiate the habitable zone inner sprite as a child of the current GameObject
        GameObject habitableZoneInnerMarker = Instantiate(habitableZoneInnerPrefab, transform);
        // Set the scale of the habitable zone inner sprite to match the size of the habitable zone
        habitableZoneInnerMarker.transform.localScale = new Vector3(habitableRangeInner, habitableRangeInner, 1f);

        // Rotate the habitable zone outer sprite to cover the x plane
        habitableZoneOuterMarker.transform.Rotate(new Vector3(90f, 0f, 0f));
        // Rotate the habitable zone inner sprite to cover the x plane
        habitableZoneInnerMarker.transform.Rotate(new Vector3(90f, 0f, 0f));
    }
}
