using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RockyPlanet : Planet
{
    public override float GenerateMass()
    {
        float averageDensity = MeanDensity; // kg/m^3, average density of rocky planets

        // Calculate the volume of the planet
        float volume = (4f / 3f) * Mathf.PI * Mathf.Pow(Radius, 3f);

        Mass = averageDensity * volume;

        //Debug.Log("averageDensity: " + averageDensity);
        //Debug.Log("volume: " + volume);
        //Debug.Log("Radius: " + Radius);
        //Debug.Log("Mass: " + Mass);

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

        //Debug.Log("Radius: " + Radius);
        //Debug.Log("totalMass: " + totalMass);
        //Debug.Log("volume: " + volume);
        //Debug.Log("MeanDensity: " + MeanDensity);

        return MeanDensity;
    }

    public override float GenerateRadius()
    {
        Radius = Random.Range(0.8f, 6.25f);  // in AU
        return Radius;
    }

    public override SerializableDictionary<string, float> GenerateComposition()
    {
        Composition = new();
        float total = 0f;

        // Define the probabilities of each element
        float ironProb = Random.value;
        float siliconProb = Random.value;
        float magnesiumProb = Random.value;
        float oxygenProb = Random.value;
        float carbonProb = Random.value;
        float nitrogenProb = Random.value;
        float sulfurProb = Random.value;
        float nickelProb = Random.value;
        float calciumProb = Random.value;
        float aluminumProb = Random.value;
        float sodiumProb = Random.value;
        float potassiumProb = Random.value;
        float chlorineProb = Random.value;
        float phosphorusProb = Random.value;

        // Calculate total probability
        total = ironProb + siliconProb + magnesiumProb + oxygenProb + carbonProb + nitrogenProb + sulfurProb + nickelProb +
                calciumProb + aluminumProb + sodiumProb + potassiumProb + chlorineProb + phosphorusProb;

        // Add element probabilities to the dictionary
        Composition.Add("Fe", ironProb / total);
        Composition.Add("Si", siliconProb / total);
        Composition.Add("Mg", magnesiumProb / total);
        Composition.Add("O", oxygenProb / total);
        Composition.Add("C", carbonProb / total);
        Composition.Add("N", nitrogenProb / total);
        Composition.Add("S", sulfurProb / total);
        Composition.Add("Ni", nickelProb / total);
        Composition.Add("Ca", calciumProb / total);
        Composition.Add("Al", aluminumProb / total);
        Composition.Add("Na", sodiumProb / total);
        Composition.Add("K", potassiumProb / total);
        Composition.Add("Cl", chlorineProb / total);
        Composition.Add("P", phosphorusProb / total);

        return Composition;
    }
}
