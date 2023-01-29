using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;
    public StarSaveData activeSave;
    public bool hasLoaded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Awake()
    {
        instance = this;
        Load();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Load();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            DeleteSaveData();
        }
    }

    public void Save()
    {
        // goes to C:/Users/steve/AppData/LocalLow/DefaultCompany/Solar Systems
        // change in the future
        string dataPath = Application.persistentDataPath;

        // type of info we are saving
        var serializer = new XmlSerializer(typeof(StarSaveData));

        // where we are saving it to
        var stream = new FileStream(dataPath + "/" + activeSave.saveName + "currentSolarSystem.xml", FileMode.Create);

        // object to save
        serializer.Serialize(stream, activeSave);

        stream.Close();
        Debug.Log("Saved.");
        //Debug.Log(dataPath);
    }

    public void Load()
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + "currentSolarSystem.xml"))
        {
            var serializer = new XmlSerializer(typeof(StarSaveData));
            var stream = new FileStream(dataPath + "/" + activeSave.saveName + "currentSolarSystem.xml", FileMode.Open);
            activeSave = serializer.Deserialize(stream) as StarSaveData;

            stream.Close();
            Debug.Log("Loaded.");

            hasLoaded = true;
        }
    }

    public void DeleteSaveData()
    {
        string dataPath = Application.persistentDataPath;

        if (System.IO.File.Exists(dataPath + "/" + activeSave.saveName + "currentSolarSystem.xml"))
        {
            File.Delete(dataPath + "/" + activeSave.saveName + "currentSolarSystem.xml");
        }
    }
}

[System.Serializable]
public class StarSaveData
{
    public string saveName;

    public double savedStarAge;
    public double savedStarMass;
    public double savedStarRadius;
    public double savedStarLuminosity;
    public double savedStarTemperature;
    public double savedStarRotation;
    public double savedStarMagneticField;
    public bool savedStarVariability;
    public bool savedStarBinary;
    public double savedStarDistance;
    public Vector3 savedStarSize;
}
