using UnityEngine;

public class EllipseCalculator : MonoBehaviour
{
    public Vector3 fociOne;
    public Vector3 fociTwo;
    public float starMass;

    public float semiMajorAxis;
    public float semiMinorAxis;

    private void Start()
    {
        /*
        fociOne = SaveManager.instance.activeSave.starFociOne;
        fociTwo = SaveManager.instance.activeSave.starFociTwo;
        starMass = (float)SaveManager.instance.activeSave.starMass;
        // Calculate the distance between the foci points
        float distance = Vector3.Distance(fociOne, fociTwo);

        // Calculate the semi-major and semi-minor axes
        semiMajorAxis = distance / 2f;
        semiMinorAxis = Mathf.Sqrt(Mathf.Pow(semiMajorAxis, 2f) - Mathf.Pow(starMass, 2f));
        */
    }

    /*
    public Vector3 GetPositionOnEllipse(float time)
    {
        // Calculate the position on the ellipse at the given time
        float x = semiMajorAxis * Mathf.Cos(time);
        float y = semiMinorAxis * Mathf.Sin(time);

        // Calculate the position of the planet relative to the center of the ellipse
        Vector3 position = new Vector3(x, 0f, y);

        // Offset the position by the average position of the foci points
        position += (fociOne + fociTwo) / 2f;

        return position;

    }
    */
}
