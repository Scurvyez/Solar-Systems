using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class RockyPlanet : Planet
{
    public override float GenerateMass()
    {
        float starMass = (float)SaveManager.instance.activeSave.starMass; // solar masses

        // Convert FocusPoint to AU
        Vector3 focusPointInAU = FocusPoint / 149597870700f;

        // Convert OrbitalPeriod to Earth years
        float periodInYears = OrbitalPeriod / 365.25f;


        // Calculate the mass using the third law of Kepler
        Mass = 4f * Mathf.Pow(Mathf.PI, 2f) * Mathf.Pow(focusPointInAU.x, 3f) / (GravConstant * Mathf.Pow(periodInYears, 2f) * starMass);

        Debug.Log("focusPointInAU: " + "..." + focusPointInAU);
        Debug.Log("GravConstant: " + "..." + GravConstant);
        Debug.Log("periodInYears: " + "..." + periodInYears);
        Debug.Log("starMass: " + "..." + starMass);
        Debug.Log("Mass: " + "..." + Mass);

        return Mass;
    }

    public override float GenerateMeanDensity()
    {
        // Calculate the volume of the planet
        float radiusInKm = Radius * 6370.0f; // Earth radii to km

        float volume = (4f / 3f) * Mathf.PI * Mathf.Pow(radiusInKm, 3f);

        // Calculate and set the mean density of the planet
        MeanDensity = Mass / volume;

        //Debug.Log("radiusInKm: " + "..." + radiusInKm);
        //Debug.Log("volume: " + "..." + volume);
        //Debug.Log("Mass: " + "..." + Mass);
        //Debug.Log("MeanDensity: " + "..." + MeanDensity);

        return MeanDensity;
    }

    public override float GenerateRadius()
    {
        Radius = Random.Range(0.8f, 6.25f);  // in Earth radii
        return Radius;
    }

    public override SerializableDictionary<string, float> GenerateComposition()
    {
        Composition = new SerializableDictionary<string, float>();
        float total = 100f;

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

        return Composition;
    }
}
