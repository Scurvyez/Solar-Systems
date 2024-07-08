using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace Language
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance { get; private set; }

        private Dictionary<string, string> _localizedStrings;
        private const string LanguageFolderPath = "Assets/Resources/Language/";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadText("strings_en.xml"); // Default to English
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void LoadText(string fileName)
        {
            string filePath = Path.Combine(LanguageFolderPath, fileName);
            if (File.Exists(filePath))
            {
                XmlSerializer serializer = new (typeof(LanguageData));
                using (StreamReader reader = new (filePath))
                {
                    LanguageData data = (LanguageData)serializer.Deserialize(reader);
                    _localizedStrings = new Dictionary<string, string>();
                    foreach (LocalizedString localizedString in data.Strings)
                    {
                        _localizedStrings[localizedString.Key] = localizedString.Value;
                    }
                }
            }
            else
            {
                Debug.LogError("Localization file not found: " + filePath);
            }
        }

        public string GetValue(string key)
        {
            if (_localizedStrings.TryGetValue(key, out string value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning("Localization key not found: " + key);
                return key;
            }
        }
    }
}