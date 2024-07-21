using UnityEngine;

namespace Utils
{
    public static class TexturesUtil
    {
        public static readonly string[] RockyPlanetTextureFolders = 
        { 
            "barren_1", "barren_2", "barren_3", "barren_4", "barren_5",
            "barren_6", "barren_7", "barren_8", "barren_10", "crystal_1", 
            "crystal_2", "crystal_3", "crystal_4", "crystal_5", "crystal_6",
            "crystal_7", "dark_1", "dark_2", "dark_3", "dark_4", 
            "desert_1", "desert_2", "desert_3", "desert_4", "desert_6", 
            "desert_7", "desert_8", "desolated_1", "forest_1", "frozen_1",
            "frozen_3", "frozen_4", "frozen_6", "liquid_1", "liquid_3", 
            "liquid_4", "liquid_5", "tundra_1", "tundra_2", "tundra_3", 
            "wasteland_1", "wasteland_3", "wasteland_4", "wasteland_5", "wasteland_7", 
            "wasteland_8", "wetlands_1", "wetlands_2", "white_desert_1"
        };

        public static readonly string[] GasGiantTextureFolders =
        {
            "gasgiant_1", "gasgiant_2", "gasgiant_3", "gasgiant_4",
            "gasgiant_5", "gasgiant_6", "gasgiant_8", "gasgiant_11"
        };
        
        public static void GetPlanetTextures(string planetTypeFolder, out Texture2D albedo, out Texture2D ambientOcclusion, out Texture2D normalMap, out Texture2D heightMap)
        {
            // Initialize the output parameters
            albedo = null;
            ambientOcclusion = null;
            normalMap = null;
            heightMap = null;

            string folderPath = $"Textures/Planets/{planetTypeFolder}";
            Texture2D[] textures = Resources.LoadAll<Texture2D>(folderPath);

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
                else if (texture.name.EndsWith("_Height"))
                {
                    heightMap = texture;
                }
            }

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
            if (heightMap == null)
            {
                Debug.LogWarning($"Height map texture not found in folder {folderPath}");
            }
        }
    }
}