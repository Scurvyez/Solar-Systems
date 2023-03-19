using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GasGiant : Planet
{
    public override float GenerateMass()
    {
        float averageMass = 318f; // Earth masses, average mass of gas giants
        Mass = averageMass * Mathf.Pow(Radius, 3f) / Mathf.Pow(SemiMajorAxis, 1.5f);
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
        float volume = (4f / 3f) * Mathf.PI * Mathf.Pow(Radius, 3f);

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
        Composition = new();
        float total = 0f;

        // Define the probabilities of each element
        float hydrogenProb = Random.value;
        float heliumProb = Random.value;
        float carbonProb = Random.value;
        float nitrogenProb = Random.value;
        float neonProb = Random.value;

        // Calculate total probability
        total = hydrogenProb + heliumProb + carbonProb + nitrogenProb + neonProb;

        // Add element probabilities to the dictionary
        Composition.Add("Fe", hydrogenProb / total);
        Composition.Add("Si", heliumProb / total);
        Composition.Add("C", carbonProb / total);
        Composition.Add("N", nitrogenProb / total);
        Composition.Add("Ne", neonProb / total);

        return Composition;
    }
}
