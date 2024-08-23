using System.Collections.Generic;
using Language;
using UnityEngine;

public class StarDescriptions : MonoBehaviour
{
    public Dictionary<Star.SpectralType, string> StarDescription;
    
    private LanguageManager _langMan;
    
    public void Start()
    {
        _langMan = LanguageManager.Instance;
        StarDescription = new Dictionary<Star.SpectralType, string>()
        {
            { Star.SpectralType.O, _langMan.GetValue("desc_o_class") },
            { Star.SpectralType.B, _langMan.GetValue("desc_b_class") },
            { Star.SpectralType.A, _langMan.GetValue("desc_a_class") },
            { Star.SpectralType.F, _langMan.GetValue("desc_f_class") },
            { Star.SpectralType.G, _langMan.GetValue("desc_g_class") },
            { Star.SpectralType.K, _langMan.GetValue("desc_k_class") },
            { Star.SpectralType.M, _langMan.GetValue("desc_m_class") },
        };
    }
}
