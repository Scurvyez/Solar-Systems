using UnityEngine;

namespace Utils
{
    public class TexturesUtil : MonoBehaviour
    {
        public static readonly string[] RockyPlanetTextureFolders = 
        { 
            "barren_1", "barren_2", "barren_3", "crystal_1", 
            "crystal_2", "crystal_4", "crystal_5", "dark_1",
            "desert_1", "desert_2", "desert_3", "forest_1",
            "wetlands_1"
        };

        public static readonly string[] GasGiantTextureFolders =
        {
            "gasgiant_1", "gasgiant_2", "gasgiant_3", "gasgiant_4",
            "gasgiant_5", "gasgiant_6", "gasgiant_8", "gasgiant_11"
        };
        
        public static void GetPlanetTextures(string planetTypeFolder, out Texture2D albedo, out Texture2D ambientOcclusion, out Texture2D normalMap)
        {
            // Initialize the output parameters
            albedo = null;
            ambientOcclusion = null;
            normalMap = null;

            // Define the folder path based on the planet type
            string folderPath = $"Textures/Planets/{planetTypeFolder}";

            // Load all textures from the specified folder
            Texture2D[] textures = Resources.LoadAll<Texture2D>(folderPath);

            // Iterate through the textures and assign them based on their names
            foreach (Texture2D texture in textures)
            {
                if (texture.name.EndsWith("_Albedo"))
                {
                    albedo = texture;
                }
                else if (texture.name.EndsWith("_AmbientOcclusion"))
                {
                    ambientOcclusion = texture;
                }
                else if (texture.name.EndsWith("_Normal"))
                {
                    normalMap = texture;
                }
            }

            // Check if any texture is missing and log a warning
            if (albedo == null)
            {
                Debug.LogWarning($"Albedo texture not found in folder {folderPath}");
            }
            if (ambientOcclusion == null)
            {
                Debug.LogWarning($"Ambient Occlusion texture not found in folder {folderPath}");
            }
            if (normalMap == null)
            {
                Debug.LogWarning($"Normal map texture not found in folder {folderPath}");
            }
        }
    }
}