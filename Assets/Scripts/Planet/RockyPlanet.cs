using UnityEngine;

[System.Serializable]
public class RockyPlanet : Planet
{
    public override float GenerateMass(float starMass, Vector3 planetFocusPoint, float planetOrbitalPeriod)
    {
        Vector3 focusPointInAU = planetFocusPoint / 149597870700f; // Convert FocusPoint to AU
        float periodInYears = planetOrbitalPeriod / 365.25f; // Convert OrbitalPeriod to Earth years
        
        // Calculate the mass using the third law of Kepler
        Mass = 4f * Mathf.Pow(Mathf.PI, 2f) * Mathf.Pow(focusPointInAU.x, 3f) / (GravConstant * Mathf.Pow(periodInYears, 2f) * starMass);

        return Mass;
    }

    public override float GenerateGORadius()
    {
        //Radius = Random.Range(0.8f, 6.25f);  // in Earth radii
        Radius = 2.0f;
        return Radius;
    }

    public override SerializableDictionary<string, float> GenerateComposition()
    {
        Composition = new SerializableDictionary<string, float>();
        float total = 100f;

        float iron = Random.Range(0f, total);
        Composition.Add("Fe", iron);

        total -= iron;
        float silicon = Random.Range(0f, total);
        Composition.Add("Si", silicon);

        total -= silicon;
        float magnesium = Random.Range(0f, total);
        Composition.Add("Mg", magnesium);

        total -= magnesium;
        float oxygen = Random.Range(0f, total);
        Composition.Add("O", oxygen);

        total -= oxygen;
        float carbon = Random.Range(0f, total);
        Composition.Add("C", carbon);

        total -= carbon;
        float nitrogen = Random.Range(0f, total);
        Composition.Add("N", nitrogen);

        total -= nitrogen;
        float sulfur = Random.Range(0f, total);
        Composition.Add("S", sulfur);

        total -= sulfur;
        float nickel = Random.Range(0f, total);
        Composition.Add("Ni", nickel);

        total -= nickel;
        float calcium = Random.Range(0f, total);
        Composition.Add("Ca", calcium);

        total -= calcium;
        float aluminum = Random.Range(0f, total);
        Composition.Add("Al", aluminum);

        total -= aluminum;
        float sodium = Random.Range(0f, total);
        Composition.Add("Na", sodium);

        total -= sodium;
        float potassium = Random.Range(0f, total);
        Composition.Add("K", potassium);

        total -= potassium;
        float chlorine = Random.Range(0f, total);
        Composition.Add("Cl", chlorine);

        total -= chlorine;
        float phosphorus = total;
        Composition.Add("P", phosphorus);

        return Composition;
    }
}
