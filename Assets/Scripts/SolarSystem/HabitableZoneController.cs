using Saving;
using UnityEngine;

public class HabitableZoneController : MonoBehaviour
{
    public GameObject StarPrefab;
    public PolygonGenerator PolygonGenerator;
    public Material RingMaterial;
    public int RingSides;
    public float InnerRadius;
    public float OuterRadius;

    private void Start()
    {
        // Assign the material to the PolygonGenerator
        PolygonGenerator.GetComponent<MeshRenderer>().material = RingMaterial;

        InnerRadius = SaveManager.Instance.ActiveSave.habitableRangeInner;
        OuterRadius = SaveManager.Instance.ActiveSave.habitableRangeOuter;

        // Set the PolygonGenerator's properties
        PolygonGenerator.PolygonSides = RingSides;
        PolygonGenerator.CenterRadius = InnerRadius / 1.5f; // 1.5f? Make a constant world factor at some point for scaling everything?
        PolygonGenerator.PolygonRadius = OuterRadius / 1.5f;

        // Call the appropriate method in PolygonGenerator based on your needs
        PolygonGenerator.IsFilled = false; // Set to false for a hollow ring
        PolygonGenerator.DrawHollow(RingSides, OuterRadius, InnerRadius);

        PolygonGenerator.transform.Rotate(new Vector3(90f, 0f, 0f));
        PolygonGenerator.transform.localPosition = StarPrefab.transform.position;
    }
}
