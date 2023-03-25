using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class RockyPlanet : Planet
{
    public override float GenerateMass()
    {
        float radiusToKm = Radius * 6371f;
        float averageDensity = MeanDensity; // kg/m^3, average density of rocky planets

        // Calculate the volume of the planet
        float volume = (4f / 3f) * Mathf.PI * Mathf.Pow(radiusToKm, 3f);
        Mass = averageDensity * volume;

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
            //Debug.Log(element.Key.ToString() + "..." + element.Value);
        }

        //Debug.Log(totalMass.ToString() + "%");

        // Calculate the volume of the planet
        float radiusToKm = Radius * 6371f;
        float volume = (4f / 3f) * Mathf.PI * Mathf.Pow(radiusToKm, 3f);

        // Calculate and set the mean density of the planet
        MeanDensity = totalMass / volume;

        return MeanDensity;
    }

    public override float GenerateRadius()
    {
        Radius = Random.Range(0.8f, 6.25f);  // in AU
        return Radius;
    }

    public override SerializableDictionary<string, float> GenerateComposition()
    {
        Composition = new SerializableDictionary<string, float>();
        float total = 100f;

        // Define the probabilities of each element
        float iron;
        float silicon;
        float magnesium;
        float oxygen;
        float carbon;
        float nitrogen;
        float sulfur;
        float nickel;
        float calcium;
        float aluminum;
        float sodium;
        float potassium;
        float chlorine;
        float phosphorus;

        iron = Random.Range(0f, total);
        Composition.Add("Fe", iron);

        total -= iron;
        silicon = Random.Range(0f, total);
        Composition.Add("Si", silicon);

        total -= silicon;
        magnesium = Random.Range(0f, total);
        Composition.Add("Mg", magnesium);

        total -= magnesium;
        oxygen = Random.Range(0f, total);
        Composition.Add("O", oxygen);

        total -= oxygen;
        carbon = Random.Range(0f, total);
        Composition.Add("C", carbon);

        total -= carbon;
        nitrogen = Random.Range(0f, total);
        Composition.Add("N", nitrogen);

        total -= nitrogen;
        sulfur = Random.Range(0f, total);
        Composition.Add("S", sulfur);

        total -= sulfur;
        nickel = Random.Range(0f, total);
        Composition.Add("Ni", nickel);

        total -= nickel;
        calcium = Random.Range(0f, total);
        Composition.Add("Ca", calcium);

        total -= calcium;
        aluminum = Random.Range(0f, total);
        Composition.Add("Al", aluminum);

        total -= aluminum;
        sodium = Random.Range(0f, total);
        Composition.Add("Na", sodium);

        total -= sodium;
        potassium = Random.Range(0f, total);
        Composition.Add("K", potassium);

        total -= potassium;
        chlorine = Random.Range(0f, total);
        Composition.Add("Cl", chlorine);

        total -= chlorine;
        phosphorus = total;
        Composition.Add("P", phosphorus);

        // Calculate total probability
        var sum = iron + silicon + magnesium + oxygen + carbon + nitrogen + sulfur + nickel +
                calcium + aluminum + sodium + potassium + chlorine + phosphorus;

        Debug.Log("Fe: " + iron);
        Debug.Log("Si: " + silicon);
        Debug.Log("Mg: " + magnesium);
        Debug.Log("O: " + oxygen);
        Debug.Log("C: " + carbon);
        Debug.Log("N: " + nitrogen);
        Debug.Log("S: " + sulfur);
        Debug.Log("Ni: " + nickel);
        Debug.Log("Ca: " + calcium);
        Debug.Log("Al: " + aluminum);
        Debug.Log("Na: " + sodium);
        Debug.Log("K: " + potassium);
        Debug.Log("Cl: " + chlorine);
        Debug.Log("P: " + phosphorus);
        Debug.Log("Total %: " + sum);

        return Composition;
    }
}
