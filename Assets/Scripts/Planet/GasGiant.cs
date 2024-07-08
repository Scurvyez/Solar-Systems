using UnityEngine;
using Utils;

[System.Serializable]
public class GasGiant : Planet
{
    /*
    public override float GenerateMass(float starMass, Vector3 planetFocusPoint, float planetOrbitalPeriod)
    {
        Mass = ConstantsUtil.GAS_GIANT_MASS_AVERAGE * Mathf.Pow(Radius * 6371f, 3f) / Mathf.Pow(SemiMajorAxis, 1.5f);
        return Mass;
    }
    */

    public override float GenerateGORadius()
    {
        Radius = 3.25f;
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

    public override bool IsRandomlyHabitable(float habRangeInner, float habRangeOuter, Vector3 planetFocusPoint)
    {
        return false;
    }
}
