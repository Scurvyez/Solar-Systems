using UnityEngine;

[System.Serializable]
public class GasGiant : Planet
{
    private const float MASS_AVERAGE = 318f; // Earth masses, average mass of gas giants
    
    /*
    public override float GenerateMass(float starMass, Vector3 planetFocusPoint, float planetOrbitalPeriod)
    {
        Mass = MASS_AVERAGE * Mathf.Pow(Radius * 6371f, 3f) / Mathf.Pow(SemiMajorAxis, 1.5f);
        return Mass;
    }
    */

    public override float GenerateGORadius()
    {
        Radius = Random.Range(3.5f, 12.0f); // in AU
        return Radius;
    }

    public override SerializableDictionary<string, float> GenerateComposition()
    {
        Composition = new SerializableDictionary<string, float>();
        float total = 100f;

        float hydrogen = Random.Range(0f, total);
        Composition.Add("H", hydrogen);

        total -= hydrogen;
        float helium = Random.Range(0f, total);
        Composition.Add("He", helium);

        total -= helium;
        float neon = Random.Range(0f, total);
        Composition.Add("Ne", neon);

        total -= neon;
        float nitrogen = total;
        Composition.Add("N", nitrogen);

        return Composition;
    }
}
