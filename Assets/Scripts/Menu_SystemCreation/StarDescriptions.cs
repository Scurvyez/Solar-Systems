using System.Collections.Generic;
using Language;
using UnityEngine;

public class StarDescriptions : MonoBehaviour
{
    public Dictionary<Star.SpectralType, string> StarDescription;
    
    private LanguageManager _locMan;
    
    public void Start()
    {
        _locMan = LanguageManager.Instance;
        StarDescription = new Dictionary<Star.SpectralType, string>()
        {
            { Star.SpectralType.O, _locMan.GetValue("desc_o_class") },
            { Star.SpectralType.B, _locMan.GetValue("desc_b_class") },
            { Star.SpectralType.A, _locMan.GetValue("desc_a_class") },
            { Star.SpectralType.F, _locMan.GetValue("desc_f_class") },
            { Star.SpectralType.G, _locMan.GetValue("desc_g_class") },
            { Star.SpectralType.K, _locMan.GetValue("desc_k_class") },
            { Star.SpectralType.M, _locMan.GetValue("desc_m_class") },
        };
    }
}
