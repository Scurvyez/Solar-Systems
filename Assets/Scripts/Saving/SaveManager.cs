using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Xml.Serialization;

namespace Saving
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance;
        public SystemSaveData ActiveSave;
        public Button ResetButton = null;
        public bool HasLoaded;
        public bool HasBeenReset;
    
        // Start is called before the first frame update
        private void Start()
        {
            if (ResetButton == null) return;
            ResetButton.onClick.AddListener(DeleteSaveData);
        }
    
        public void Awake()
        {
            Instance = this;
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
            FileStream stream = new (dataPath + "/" + ActiveSave + ".xml", FileMode.Create);
    
            // object to save
            serializer.Serialize(stream, ActiveSave);
    
            stream.Close();
            //Debug.Log("Saved.");
            //Debug.Log(dataPath);
        }
    
        private void Load()
        {
            string dataPath = Application.persistentDataPath;
    
            if (!File.Exists(dataPath + "/" + ActiveSave + ".xml")) return;
            XmlSerializer serializer = new (typeof(SystemSaveData));
            FileStream stream = new (dataPath + "/" + ActiveSave + ".xml", FileMode.Open);
            ActiveSave = serializer.Deserialize(stream) as SystemSaveData;
            stream.Close();
            //Debug.Log("Loaded.");
            HasLoaded = true;
        }
    
        private void DeleteSaveData()
        {
            string dataPath = Application.persistentDataPath;
    
            if (File.Exists(dataPath + "/" + ActiveSave + ".xml"))
            {
                File.Delete(dataPath + "/" + ActiveSave + ".xml");
            }
    
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            HasBeenReset = true;
        }
    }    
}
