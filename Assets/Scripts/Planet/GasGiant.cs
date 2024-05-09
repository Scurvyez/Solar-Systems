using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GasGiant : Planet
{
    public override float GenerateMass()
    {
        float averageMass = 318f; // Earth masses, average mass of gas giants
        Mass = averageMass * Mathf.Pow(Radius * 6371f, 3f) / Mathf.Pow(SemiMajorAxis, 1.5f);
        return Mass;
    }

    public override float GenerateMeanDensity()
    {
        // Get the total mass of all the elements in the planet's composition
        float totalMass = 0f;
        foreach (KeyValuePair<string, float> element in Composition)
        {
            var atomicMassHelper = new AtomicMass();
            float atomicMass = atomicMassHelper.GetAtomicMass(element.Key);
            totalMass += atomicMass * element.Value;
        }

        // Calculate the volume of the planet
        float volume = (4f / 3f) * Mathf.PI * Mathf.Pow(Radius * 6371f, 3f);

        // Calculate and set the mean density of the planet
        MeanDensity = totalMass / volume;
        MeanDensity *= 1000f; // convert from g/cm³ to kg/m³

        return MeanDensity;
    }

    public override float GenerateRadius()
    {
        Radius = Random.Range(3.5f, 12.0f); // in AU
        return Radius;
    }

    public override SerializableDictionary<string, float> GenerateComposition()
    {
        Composition = new SerializableDictionary<string, float>();
        float total = 100f;

        float hydrogen;
        float helium;
        float neon;
        float nitrogen;

        hydrogen = Random.Range(0f, total);
        Composition.Add("H", hydrogen);

        total -= hydrogen;
        helium = Random.Range(0f, total);
        Composition.Add("He", helium);

        total -= helium;
        neon = Random.Range(0f, total);
        Composition.Add("Ne", neon);

        total -= neon;
        nitrogen = total;
        Composition.Add("N", nitrogen);

        return Composition;
    }
}
