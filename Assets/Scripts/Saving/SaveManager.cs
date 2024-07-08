using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Xml.Serialization;
using Utils;

namespace Saving
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance;
        public SystemSaveData ActiveSave;
        public Button ResetButton = null;
        public bool HasLoaded;
        public bool HasBeenReset;
    
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
    
            XmlSerializer serializer = new (typeof(SystemSaveData));
            FileStream stream = new (dataPath + "/" + ActiveSave + ".xml", FileMode.Create);
            serializer.Serialize(stream, ActiveSave);
    
            stream.Close();
        }
    
        private void Load()
        {
            string dataPath = Application.persistentDataPath;
    
            if (!File.Exists(dataPath + "/" + ActiveSave + ".xml")) return;
            XmlSerializer serializer = new (typeof(SystemSaveData));
            FileStream stream = new (dataPath + "/" + ActiveSave + ".xml", FileMode.Open);
            ActiveSave = serializer.Deserialize(stream) as SystemSaveData;
            stream.Close();
            HasLoaded = true;
        }
    
        private void DeleteSaveData()
        {
            string dataPath = Application.persistentDataPath;
    
            if (File.Exists(dataPath + "/" + ActiveSave + ".xml"))
            {
                File.Delete(dataPath + "/" + ActiveSave + ".xml");
            }
    
            StartCoroutine(DelayedSceneSwitch(ConstantsUtil.BUTTON_ACTION_DELAY_SOUND));
            HasBeenReset = true;
        }
        
        private IEnumerator DelayedSceneSwitch(float delay)
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }    
}
