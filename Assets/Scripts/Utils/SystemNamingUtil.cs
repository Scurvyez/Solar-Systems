using UnityEngine;

namespace Utils
{
    public static class SystemNamingUtil
    {
        public static string Info_SystemName { get; set; }

        private const string LETTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string NUMBERS = "0123456789";

        private static readonly char[] Consonants = { 'b', 'c', 'd', 'f', 'g', 'h', 'j', 'k', 'l', 'm', 'n', 'p', 'q', 'r', 's', 't', 'v', 'w', 'x', 'y', 'z' };
        private static readonly char[] Vowels = { 'a', 'e', 'i', 'o', 'u' };
        private static readonly string[] RomanNumerals = { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X" };

        private static readonly System.Random _nameGenRandom = new ();
        
        /// <summary>
        /// Chooses between 2 available naming methods.
        /// Randomly picks one and returns the generated star system name.
        /// </summary>
        public static string PickNamingMethodAndGenerate()
        {
            Info_SystemName = _nameGenRandom.Next(100) < 8 ? GenerateStarSystemNameSemiUnique() : GenerateStarSystemNameGeneric();
            return Info_SystemName;
        }
        
        /// <summary>
        /// Generates a semi-unique star system name using the English alphabet.
        /// Alternating patterns of consonants and vowels.
        /// (Lazy naming method.)
        /// </summary>
        private static string GenerateStarSystemNameSemiUnique()
        {
            // Set the name length to a random number between 3 and 9
            int nameLength = _nameGenRandom.Next(3, 10);

            // Start with an empty string
            string tempSystemName = "";

            // Loop for the specified number of characters in the name
            for (int i = 0; i < nameLength; i++)
            {
                // Choose a random character from either the consonants or vowels
                char letter = i % 2 == 0 ? Consonants[_nameGenRandom.Next(Consonants.Length)] : Vowels[_nameGenRandom.Next(Vowels.Length)];

                // Add the letter to the name
                tempSystemName += letter;
            }

            // Capitalize the first letter of the name
            tempSystemName = char.ToUpper(tempSystemName[0]) + tempSystemName.Substring(1);

            // Check if the name should be hyphenated
            if (_nameGenRandom.Next(100) >= 50) return tempSystemName;
            string numeral = RomanNumerals[_nameGenRandom.Next(RomanNumerals.Length)];
            tempSystemName = tempSystemName + "-" + numeral;

            return tempSystemName;
        }

        /// <summary>
        /// Generates a standard star system name.
        /// Based on how the International Astronomical Union (IAU) names stars.
        /// A simple mix of "x" letters, a "-", followed by a mix of "y" numbers.
        /// </summary>
        private static string GenerateStarSystemNameGeneric()
        {
            string tempSystemName = "";
            int nameLength = Random.Range(5, 14);
            bool addingLetters = true;
            for (int i = 0; i < nameLength; i++)
            {
                if (addingLetters)
                {
                    tempSystemName += LETTERS[Random.Range(0, LETTERS.Length)];
                }
                else
                {
                    tempSystemName += NUMBERS[Random.Range(0, NUMBERS.Length)];
                }

                if (i != (nameLength / 2) - 1) continue;
                tempSystemName += "-";
                addingLetters = false;
            }
            return tempSystemName;
        }
    }
}