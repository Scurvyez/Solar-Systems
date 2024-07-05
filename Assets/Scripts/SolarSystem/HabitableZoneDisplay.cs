using Saving;
using UnityEngine;

public class HabitableZoneDisplay : MonoBehaviour
{
    public GameObject StarPrefab;
    public GameObject HabitableZoneOuterPrefab;
    public GameObject HabitableZoneInnerPrefab;
    public float HabitableRangeInner;
    public float HabitableRangeOuter;

    public void Start()
    {
        HabitableRangeInner = SaveManager.Instance.ActiveSave.habitableRangeInner;
        HabitableRangeOuter = SaveManager.Instance.ActiveSave.habitableRangeOuter;

        // Instantiate the habitable zone outer sprite as a child of the current GameObject
        GameObject habitableZoneOuterMarker = Instantiate(HabitableZoneOuterPrefab, transform);
        // Set the scale of the habitable zone outer sprite to match the size of the habitable zone
        habitableZoneOuterMarker.transform.localScale = new Vector3(HabitableRangeOuter, HabitableRangeOuter, 1f);

        // Instantiate the habitable zone inner sprite as a child of the current GameObject
        GameObject habitableZoneInnerMarker = Instantiate(HabitableZoneInnerPrefab, transform);
        // Set the scale of the habitable zone inner sprite to match the size of the habitable zone
        habitableZoneInnerMarker.transform.localScale = new Vector3(HabitableRangeInner, HabitableRangeInner, 1f);

        // Rotate the habitable zone outer sprite to cover the x plane
        habitableZoneOuterMarker.transform.Rotate(new Vector3(90f, 0f, 0f));
        // Rotate the habitable zone inner sprite to cover the x plane
        habitableZoneInnerMarker.transform.Rotate(new Vector3(90f, 0f, 0f));
    }
}
