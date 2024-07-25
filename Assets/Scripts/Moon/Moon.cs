using System.Collections.Generic;
using UnityEngine;
using Utils;

[System.Serializable]
public class Moon
{
    public string Info_ParentPlanetName { get; set; }
    public string Info_Name { get; set; }
    public float Info_Mass { get; set; }
    public float GO_Radius { get; set; }
    public float OrbitalDistanceFrom { get; set; }
    public float Info_Radius { get; set; }
    public float Info_OrbitalPeriod { get; set; }
    public float Info_RotationPeriod { get; set; }
    public float Info_AxialTilt { get; set; }
    public float Info_SurfaceTemperature { get; set; }
    public bool Info_HasAtmosphere { get; set; }
    public bool Info_Habitable { get; set; }
    public float Info_MeanDensity { get; set; }
    public float Info_MagneticFieldStrength { get; set; }
    public float Info_SurfacePressure { get; set; }
    public float Info_SurfaceGravity { get; set; }
    public float Info_EscapeVelocity { get; set; }
    public SerializableDictionary<string, float> Info_Composition { get; set; }
    public SerializableDictionary<string, float> Info_AtmosphereComposition { get; set; }

    public virtual float GenerateMass(float moonRadius, float moonMeanDensity)
    {
        float radiusInMeters = moonRadius * 1000f;
        float volInCubicM = (4f / 3f) * Mathf.PI * Mathf.Pow(radiusInMeters, 3f);
        float moonMassInKG = moonMeanDensity * volInCubicM;

        Info_Mass = moonMassInKG;
        return Info_Mass;
    }

    public virtual float GenerateGORadius()
    {
        GO_Radius = 0.25f;
        return GO_Radius;
    }
    
    public virtual float GenerateInfoRadius()
    {
        Info_Radius = Random.Range(1000f, 5300f);
        return Info_Radius;
    }

    public virtual float GenerateOrbitalDistanceFrom()
    {
        OrbitalDistanceFrom = 1f;
        return OrbitalDistanceFrom;
    }

    public virtual float GenerateOrbitalPeriod()
    {
        Info_OrbitalPeriod = Random.Range(3.5f, 200f);
        return Info_OrbitalPeriod;
    }

    public virtual float GenerateRotationPeriod()
    {
        Info_RotationPeriod = Random.Range(5.75f, 75f);
        return Info_RotationPeriod;
    }

    public virtual float GenerateAxialTilt()
    {
        Info_AxialTilt = Random.Range(0f, 90f);
        return Info_AxialTilt;
    }

    public virtual float GenerateSurfaceTemperature()
    {
        Info_SurfaceTemperature = Random.Range(-500f, 500f);
        return Info_SurfaceTemperature;
    }

    public virtual bool HasRandomAtmosphere()
    {
        Info_HasAtmosphere = Random.value < 0.35f;
        return Info_HasAtmosphere;
    }

    public virtual bool IsRandomlyHabitable()
    {
        Info_Habitable = Random.value < 0.2f;
        return Info_Habitable;
    }

    public virtual float GenerateMeanDensity(SerializableDictionary<string, float> moonComposition)
    {
        Info_MeanDensity = 0;
        float totalProportion = 0;

        foreach (KeyValuePair<string, float> element in moonComposition)
        {
            float atomicMass = AtomicMass.GetAtomicMass(element.Key);
            Info_MeanDensity += atomicMass * element.Value;
            totalProportion += element.Value;
        }
        return Info_MeanDensity / totalProportion;
    }
    
    public virtual float GenerateMagneticFieldStrength()
    {
        Info_MagneticFieldStrength = Random.Range(0f, 100f);
        return Info_MagneticFieldStrength;
    }
    
    public virtual float GenerateSurfacePressure(SerializableDictionary<string, float> moonAtmosphereComposition, float moonSurfaceGravity, float moonTemperature)
    {
        // Dictionary to store molar masses of common atmospheric gases (in kg/mol)
        Dictionary<string, float> molarMasses = new Dictionary<string, float>
        {
            { "O", 32f }, { "N", 28f }, { "CO2", 44f }, { "CH4", 16f },
            { "H", 2f }, { "He", 4f }, { "Ne", 20f }, { "Ar", 40f },
            { "Kr", 84f }, { "Xe", 131f }, { "SO2", 64f }, { "NO", 30f },
            { "O3", 48f }, { "H2O", 18f }
        };
        
        float totalPressure = 0f;

        foreach (var gas in moonAtmosphereComposition)
        {
            string gasName = gas.Key;
            float gasProportion = gas.Value;

            if (!molarMasses.TryGetValue(gasName, out float molarMass)) continue;
            // Convert molar mass from g/mol to kg/mol (1 g/mol = 0.001 kg/mol)
            molarMass *= 10f;
            
            // Calculate partial pressure using the ideal gas law: P = (n/V)RT, where n is the number of moles, V is volume,
            // R is the gas constant, and T is temperature. Here, we'll use a proportional approach.
            float partialPressure = (gasProportion * moonSurfaceGravity * molarMass * moonTemperature) / ConstantsUtil.GAS_CONSTANT;
            totalPressure += partialPressure;
        }

        float surfacePressureInAtm = totalPressure / ConstantsUtil.EARTH_ATMOSPHERIC_PRESSURE;
        Info_SurfacePressure = surfacePressureInAtm;
        return Info_SurfacePressure;
    }
    
    public virtual bool HasLiquidWater(float moonSurfaceTemp, float moonSurfacePressure, bool hasAtmosphere)
    {
        if (moonSurfaceTemp is < 273 or > 373) return false;
        if (!hasAtmosphere) return false;
        return moonSurfacePressure is >= 0.1f and <= 100f;
    }

    public virtual float GenerateSurfaceGravity(float moonMass, float moonRadius)
    {
        float moonRadInM = moonRadius * 1000f;
        float surfaceGravity = ConstantsUtil.GRAVITY * moonMass / (moonRadInM * moonRadInM);

        Info_SurfaceGravity = surfaceGravity;
        return Info_SurfaceGravity;
    }
    
    public virtual float GenerateEscapeVelocity(float moonMass, float moonRadius)
    {
        float moonRadInM = moonRadius * 1000f;
        float escapeVelocityInMetersPerSecond = Mathf.Sqrt((2 * ConstantsUtil.GRAVITY * moonMass) / moonRadInM); // m/s

        Info_EscapeVelocity = escapeVelocityInMetersPerSecond / 1000f; // km/s
        return Info_EscapeVelocity;
    }

    public virtual SerializableDictionary<string, float> GenerateComposition()
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
    
    public virtual SerializableDictionary<string, float> GenerateAtmosphereComposition(bool hasAtmosphere)
    {
        Info_AtmosphereComposition = new SerializableDictionary<string, float>();
        if (!hasAtmosphere)
        {
            return Info_AtmosphereComposition;
        }

        float total = 100f;

            float oxygen = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("O", oxygen);

            total -= oxygen;
            float nitrogen = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("N", nitrogen);

            total -= nitrogen;
            float carbonDioxide = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("CO2", carbonDioxide);

            total -= carbonDioxide;
            float methane = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("CH4", methane);

            total -= methane;
            float hydrogen = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("H", hydrogen);

            total -= hydrogen;
            float helium = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("He", helium);

            total -= helium;
            float neon = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("Ne", neon);

            total -= neon;
            float argon = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("Ar", argon);

            total -= argon;
            float krypton = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("Kr", krypton);

            total -= krypton;
            float xenon = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("Xe", xenon);

            total -= xenon;
            float sulfurDioxide = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("SO2", sulfurDioxide);

            total -= sulfurDioxide;
            float nitrogenOxides = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("NO", nitrogenOxides);

            total -= nitrogenOxides;
            float ozone = Random.Range(0f, total);
            Info_AtmosphereComposition.Add("O3", ozone);

            total -= ozone;
            float waterVapor = total;
            Info_AtmosphereComposition.Add("H2O", waterVapor);

            return Info_AtmosphereComposition;
    }
}
