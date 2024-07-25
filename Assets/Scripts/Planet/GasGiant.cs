using UnityEngine;
using Utils;

[System.Serializable]
public class GasGiant : Planet
{
    public override float GenerateGORadius()
    {
        GO_Radius = 3.25f;
        return GO_Radius;
    }
    
    public override int GenerateNumMoons()
    {
        int numMoons = Mathf.RoundToInt(Random.Range(0, 9));
        numMoons = Mathf.Clamp(numMoons, 0, ConstantsUtil.NUM_MOONS_PER_PLANET);
        
        Info_Moons = numMoons;
        return Info_Moons;
    }

    public override SerializableDictionary<string, float> GenerateComposition()
    {
        Info_Composition = new SerializableDictionary<string, float>();
        float total = 100f;

        float hydrogen = Random.Range(0f, total);
        Info_Composition.Add("H", hydrogen);

        total -= hydrogen;
        float helium = Random.Range(0f, total);
        Info_Composition.Add("He", helium);

        total -= helium;
        float neon = Random.Range(0f, total);
        Info_Composition.Add("Ne", neon);

        total -= neon;
        float nitrogen = total;
        Info_Composition.Add("N", nitrogen);

        return Info_Composition;
    }

    public override bool IsRandomlyHabitable(float habRangeInner, float habRangeOuter, Vector3 planetFocusPoint)
    {
        return false;
    }
}
