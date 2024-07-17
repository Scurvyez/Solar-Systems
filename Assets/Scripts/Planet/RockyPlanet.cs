using UnityEngine;
using Utils;

[System.Serializable]
public class RockyPlanet : Planet
{
    public override float GenerateGORadius()
    {
        GO_Radius = 2f;
        return GO_Radius;
    }

    public override int GenerateNumMoons()
    {
        int numMoons = Mathf.RoundToInt(Random.Range(0, 3));
        numMoons = Mathf.Clamp(numMoons, 0, ConstantsUtil.NUM_MOONS_PER_PLANET);
        
        Info_Moons = numMoons;
        return Info_Moons;
    }

    public override SerializableDictionary<string, float> GenerateComposition()
    {
        Info_Composition = new SerializableDictionary<string, float>();
        float total = 100f;

        float iron = Random.Range(0f, total);
        Info_Composition.Add("Fe", iron);

        total -= iron;
        float silicon = Random.Range(0f, total);
        Info_Composition.Add("Si", silicon);

        total -= silicon;
        float magnesium = Random.Range(0f, total);
        Info_Composition.Add("Mg", magnesium);

        total -= magnesium;
        float oxygen = Random.Range(0f, total);
        Info_Composition.Add("O", oxygen);

        total -= oxygen;
        float carbon = Random.Range(0f, total);
        Info_Composition.Add("C", carbon);

        total -= carbon;
        float nitrogen = Random.Range(0f, total);
        Info_Composition.Add("N", nitrogen);

        total -= nitrogen;
        float sulfur = Random.Range(0f, total);
        Info_Composition.Add("S", sulfur);

        total -= sulfur;
        float nickel = Random.Range(0f, total);
        Info_Composition.Add("Ni", nickel);

        total -= nickel;
        float calcium = Random.Range(0f, total);
        Info_Composition.Add("Ca", calcium);

        total -= calcium;
        float aluminum = Random.Range(0f, total);
        Info_Composition.Add("Al", aluminum);

        total -= aluminum;
        float sodium = Random.Range(0f, total);
        Info_Composition.Add("Na", sodium);

        total -= sodium;
        float potassium = Random.Range(0f, total);
        Info_Composition.Add("K", potassium);

        total -= potassium;
        float chlorine = Random.Range(0f, total);
        Info_Composition.Add("Cl", chlorine);

        total -= chlorine;
        float phosphorus = total;
        Info_Composition.Add("P", phosphorus);

        return Info_Composition;
    }
}
