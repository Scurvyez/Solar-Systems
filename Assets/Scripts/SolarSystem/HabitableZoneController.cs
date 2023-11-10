using UnityEngine;

public class HabitableZoneController : MonoBehaviour
{
    public GameObject starPrefab;
    public PolygonGenerator polygonGenerator;
    public Material ringMaterial;
    public int ringSides;
    public float innerRadius;
    public float outerRadius;

    void Start()
    {
        // Assign the material to the PolygonGenerator
        polygonGenerator.GetComponent<MeshRenderer>().material = ringMaterial;

        innerRadius = SaveManager.instance.activeSave.habitableRangeInner;
        outerRadius = SaveManager.instance.activeSave.habitableRangeOuter;

        // Set the PolygonGenerator's properties
        polygonGenerator.polygonSides = ringSides;
        polygonGenerator.centerRadius = innerRadius;
        polygonGenerator.polygonRadius = outerRadius;

        // Call the appropriate method in PolygonGenerator based on your needs
        polygonGenerator.isFilled = false; // Set to false for a hollow ring
        polygonGenerator.DrawHollow(ringSides, outerRadius, innerRadius);

        polygonGenerator.transform.Rotate(new Vector3(90f, 0f, 0f));
        polygonGenerator.transform.localPosition = starPrefab.transform.position;
    }
}
