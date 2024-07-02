using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Xml.Serialization;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public SystemSaveData activeSave;
    public Button resetButton = null;
    public bool hasLoaded;
    public bool hasBeenReset;

    // Start is called before the first frame update
    private void Start()
    {
        resetButton.onClick.AddListener(DeleteSaveData);
    }

    public void Awake()
    {
        instance = this;
        Load();
    }

    public void Save()
    {
        // goes to C:/Users/steve/AppData/LocalLow/DefaultCompany/Solar Systems
        // change in the future
        string dataPath = Application.persistentDataPath;

        // type of info we are saving
        XmlSerializer serializer = new (typeof(SystemSaveData));

        // where we are saving it to
        FileStream stream = new (dataPath + "/" + activeSave.saveName + ".xml", FileMode.Create);

        // object to save
        serializer.Serialize(stream, activeSave);

        stream.Close();
        //Debug.Log("Saved.");
        //Debug.Log(dataPath);
    }

    private void Load()
    {
        string dataPath = Application.persistentDataPath;

        if (!File.Exists(dataPath + "/" + activeSave.saveName + ".xml")) return;
        XmlSerializer serializer = new (typeof(SystemSaveData));
        FileStream stream = new (dataPath + "/" + activeSave.saveName + ".xml", FileMode.Open);
        activeSave = serializer.Deserialize(stream) as SystemSaveData;
        stream.Close();
        //Debug.Log("Loaded.");
        hasLoaded = true;
    }

    private void DeleteSaveData()
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + ".xml"))
        {
            File.Delete(dataPath + "/" + activeSave.saveName + ".xml");
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        hasBeenReset = true;
    }
}

[System.Serializable]
public class SystemSaveData
{
    public string saveName;

    // STAR DATA: start
    public string starClassAsString;                                // class
    public string starSystemName;                                   // name
    public double starAge;                                          // age
    public Vector3 starPosition;                                    // position
    public float starMass;                                          // mass
    public float starRadius;                                        // radius
    public float starActualRadius;                                  // actual radius for gae object
    public float starLuminosity;                                    // luminosity
    public float starTemperature;                                   // temperature
    public float starRotation;                                      // rotation
    public float starMagneticField;                                 // magnetic field
    public bool hasIntrinsicVariability;                            // is intrinsic?
    public bool hasExtrinsicVariability;                            // is extrinsic?
    public float starVariability;                                   // variability
    public float habitableRangeInner;                               // inner hab zone boundary
    public float habitableRangeOuter;                               // outer hab zone boundary

    public Vector3 starSize;                                        // scale
    public Color starChromaticity;                                  // primary color (surface)
    public Color starCellColor;                                     // secondary color (solar flare)

    public SerializableDictionary<string, float> starMetallicity;   // metallicity
    // STAR DATA: end

    // PLANET DATA: start
    public List<RockyPlanet> rockyPlanets;
    // PLANET DATA: end

    // PLANET DATA: start
    public List<Moon> moons;
    // PLANET DATA: end
}
