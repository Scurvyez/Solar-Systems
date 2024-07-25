using UnityEngine;

namespace Utils
{
    public static class ShaderPropertyIDs
    {
        // star properties
        public static readonly int Chromaticity = Shader.PropertyToID("_Chromaticity");
        public static readonly int CellColor = Shader.PropertyToID("_CellColor");
        public static readonly int SolarFlare = Shader.PropertyToID("_SolarFlare");
        
        // planet + moon properties
        public static readonly int MainTex = Shader.PropertyToID("_MainTex");
        public static readonly int AOTex = Shader.PropertyToID("_AOTex");
        public static readonly int NormalMap = Shader.PropertyToID("_NormalMap");
        public static readonly int HeightMap = Shader.PropertyToID("_HeightMap");
        public static readonly int CloudTex = Shader.PropertyToID("_CloudTex");
        public static readonly int AtmosphereColor = Shader.PropertyToID("_AtmosphereColor");
        public static readonly int CloudColor = Shader.PropertyToID("_CloudColor");
        public static readonly int HeightMapStep = Shader.PropertyToID("_HeightMapStep");
        public static readonly int AmbientLightDirection = Shader.PropertyToID("_AmbientLightDirection");
        public static readonly int LightDirectionOffset = Shader.PropertyToID("_LightDirectionOffset");
        public static readonly int ScrollSpeed = Shader.PropertyToID("_ScrollSpeed");
        public static readonly int ScrollDirection = Shader.PropertyToID("_ScrollDirection");
        public static readonly int AtmosphereSize = Shader.PropertyToID("_AtmosphereSize");
        public static readonly int AtmosphereHalfMin = Shader.PropertyToID("_AtmosphereHalfMin");
        public static readonly int AtmosphereHalfMax = Shader.PropertyToID("_AtmosphereHalfMax");
        public static readonly int LightDirectionDefault = Shader.PropertyToID("_SLightDirectionDefault");
        public static readonly int AOStrength = Shader.PropertyToID("_AOStrength");
        public static readonly int NormalStrength = Shader.PropertyToID("_NormalStrength");
        public static readonly int HeightStrength = Shader.PropertyToID("_HeightStrength");
        public static readonly int CloudScrollSpeed = Shader.PropertyToID("_CloudScrollSpeed");
        public static readonly int CloudBlendStrength = Shader.PropertyToID("_CloudBlendStrength");
        public static readonly int CloudThreshold = Shader.PropertyToID("_CloudThreshold");
        public static readonly int CloudNoiseScrollSpeed = Shader.PropertyToID("_CloudNoiseScrollSpeed");
        public static readonly int CloudNoiseBlendFactor = Shader.PropertyToID("_CloudNoiseBlendFactor");
        public static readonly int CloudNoiseCellSize = Shader.PropertyToID("_CloudNoiseCellSize");
    }
}