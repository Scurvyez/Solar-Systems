using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class RomanNumConverter
{
    public string ToRomanNumeral(int number)
    {
        // Define a dictionary of Roman numeral values
        var numeralMap = new Dictionary<int, string>
        {
            {1000, "M"},
            {900, "CM"},
            {500, "D"},
            {400, "CD"},
            {100, "C"},
            {90, "XC"},
            {50, "L"},
            {40, "XL"},
            {10, "X"},
            {9, "IX"},
            {5, "V"},
            {4, "IV"},
            {1, "I"}
        };

        // Convert the number to a Roman numeral string
        var result = new StringBuilder();
        foreach (var pair in numeralMap)
        {
            while (number >= pair.Key)
            {
                result.Append(pair.Value);
                number -= pair.Key;
            }
        }

        return result.ToString();
    }
}
