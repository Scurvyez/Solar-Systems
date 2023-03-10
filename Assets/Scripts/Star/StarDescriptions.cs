using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDescriptions : MonoBehaviour
{
    public Dictionary<StarProperties.SpectralType, string> StarDescription;

    /// <summary>
    /// Initializes scientific descriptions for each specific star class.
    /// These are displayed in the second scene (index 1) when a user mouses over the corresponding spectral type on screen.
    /// </summary>
    public void Start()
    {
        StarDescription = new Dictionary<StarProperties.SpectralType, string>()
        {
            { StarProperties.SpectralType.O, "<color=#f9f5ff>O-type stars are incredibly hot and massive, with temperatures reaching up to 60,000 K. " +
                    "They emit a tremendous amount of ultraviolet radiation and have a very short lifespan, living only a few million years. " +
                    "They are primarily composed of hydrogen and helium.</color>" },
            { StarProperties.SpectralType.B, "<color=#f9f5ff>B-type stars are also hot and luminous, with temperatures ranging from 10,000 to 30,000 K. " +
                    "They have a relatively short lifespan compared to other star types, living up to 100 million years. " +
                    "They are primarily composed of hydrogen and helium, with traces of other elements such as carbon and nitrogen.</color>" },
            { StarProperties.SpectralType.A, "<color=#f9f5ff>A-type stars have temperatures ranging from 7,500 to 10,000 K, and have a moderate lifespan of around 1 billion years. " +
                    "They are primarily composed of hydrogen and helium, with traces of other elements such as carbon, nitrogen, and oxygen.</color>" },
            { StarProperties.SpectralType.F, "<color=#f9f5ff>F-type stars have temperatures ranging from 6,000 to 7,500 K, and have a longer lifespan of around 3 billion years. " +
                    "They are primarily composed of hydrogen and helium, with traces of other elements such as carbon, nitrogen, oxygen, and iron.</color>" },
            { StarProperties.SpectralType.G, "<color=#f9f5ff>G-type stars, like our sun, have temperatures around 5,500 K and have a lifespan of around 10 billion years. " +
                    "They are primarily composed of hydrogen and helium, with traces of other elements such as carbon, nitrogen, oxygen, and iron.</color>" },
            { StarProperties.SpectralType.K, "<color=#f9f5ff>K-type stars have temperatures ranging from 3,500 to 5,000 K and have a longer lifespan of around 30 billion years. " +
                    "They are primarily composed of hydrogen and helium, with traces of other elements such as carbon, nitrogen, oxygen, iron, silicon, and magnesium.</color>" },
            { StarProperties.SpectralType.M, "<color=#f9f5ff>M-type stars are the coolest and have temperatures of less than 3,500 K. " +
                    "They have a longer lifespan of around 100 billion years and are primarily composed of hydrogen and helium, " +
                    "with traces of other elements such as carbon, nitrogen, oxygen, iron, silicon, and magnesium.</color>" },
        };
    }
}
