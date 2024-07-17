using System;
using System.Collections.Generic;

namespace Utils
{
    /// <summary>
    /// Measured in AMU (atomic mass unit).
    /// </summary>
    public static class AtomicMass
    {
        public static float GetAtomicMass(string elementSymbol)
        {
            Dictionary<string, float> atomicMasses = new ()
            {
                {"H", 1.008f }/*Hydrogen*/, {"He", 4.0026f }/*Helium*/, { "Li", 6.94f }/*Lithium*/, { "Be", 9.0122f }/*Berylium*/,
                { "B", 10.81f }/*Boron*/, { "C", 12.011f }/*Carbon*/, { "N", 14.007f }/*Nitrogen*/, { "O", 15.999f }/*Oxygen*/,
                { "F", 18.998f }/*Fluorine*/, { "Ne", 20.180f }/*Neon*/, { "Na", 22.990f }/*Sodium*/, { "Mg", 24.305f }/*Magnesium*/,
                { "Al", 26.982f }/*Aluminium*/, { "Si", 28.085f }/*Silicon*/, { "P", 30.974f }/*Phosphorus*/, { "S", 32.06f }/*Sulfur*/,
                { "Cl", 35.45f }/*Chlorine*/, { "Ar", 39.948f }/*Argon*/, { "K", 39.098f }/*Potassium*/, { "Ca", 40.078f }/*Calcium*/,
                { "Sc", 44.956f }/*Scandium*/, { "Ti", 47.956f }/*Titanium*/, { "V", 50.942f }/*Vanadium*/, { "Cr", 51.996f }/*Chromium*/,
                { "Mn", 54.938f }/*Manganese*/, { "Fe", 55.845f }/*Iron*/, { "Co", 58.933f }/*Cobalt*/, { "Ni", 58.693f }/*Nickel*/,
                { "Cu", 63.546f }/*Copper*/, { "Zn", 65.38f }/*Zinc*/, { "Ga", 69.723f }/*Gallium*/, { "Ge", 72.630f }/*Germanium*/,
                { "As", 74.922f }/*Arsenic*/, { "Se", 78.971f }/*Selenium*/, { "Br", 79.904f }/*Bromine*/, { "Kr", 83.798f }/*Krypton*/,
                { "Rb", 85.468f }/*Rubidium*/, { "Sr", 87.62f }/*Strontium*/, { "Y", 88.906f }/*Yttrium*/, { "Zr", 91.224f }/*Zirconium*/,
                { "Nb", 92.906f }/*Niobium*/, { "Mo", 95.95f }/*Molybdenum*/, { "Tc", 98.0f }/*Technetium*/, { "Ru", 101.07f }/*Ruthenium*/,
                { "Rh", 102.91f }/*Rhodium*/, { "Pd", 106.42f }/*Palladium*/, { "Ag", 107.87f }/*Silver*/, { "Cd", 112.41f }/*Cadmium*/,
                { "In", 114.82f }/*Indium*/, { "Sn", 118.71f }/*Tin*/, { "Sb", 121.76f }/*Antimony*/, { "Te", 127.60f }/*Tellurium*/,
                { "I", 126.90f }/*Iodine*/, { "Xe", 131.29f }/*Xenon*/, { "Cs", 132.91f }/*Caesium*/, { "Ba", 137.33f }/*Barium*/,
                { "La", 138.91f }/*Lanthanum*/, { "Ce", 140.12f }/*Cerium*/, { "Pr", 140.91f }/*Praseodymium*/, { "Nd", 144.24f }/*Neodymium*/,
                { "Pm", 145.0f }/*Promethium*/, { "Sm", 150.36f }/*Samarium*/, { "Eu", 151.96f }/*Europium*/, { "Gd", 157.25f }/*Gadolinium*/,
                { "Tb", 158.93f }/*Terbium*/, { "Dy", 162.50f }/*Dysprosium*/, { "Ho", 164.93f }/*Holmium*/, { "Er", 167.26f }/*Erbium*/,
                { "Tm", 168.93f }/*Thulium*/, { "Yb", 173.05f }/*Ytterbium*/, { "Lu", 174.97f }/*Lutetium*/, { "Hf", 178.49f }/*Hafnium*/,
                { "Ta", 180.95f }/*Tantalum*/, { "W", 183.84f }/*Tungsten*/, { "Re", 186.21f }/*Rhenium*/, { "Os", 190.23f }/*Osmium*/,
                { "Ir", 192.22f }/*Iridium*/, { "Pt", 195.08f }/*Platinum*/, { "Au", 196.97f }/*Gold*/, { "Hg", 200.59f }/*Mercury*/,
                { "Tl", 204.38f }/*Thallium*/, { "Pb", 207.2f }/*Lead*/, { "Bi", 208.98f }/*Bismuth*/, { "Po", 209.0f }/*Polonium*/,
                { "At", 210.0f }/*Astatine*/, { "Rn", 222.0f }/*Radon*/, { "Fr", 223.0f }/*Francium*/, { "Ra", 226.0f }/*Radium*/,
                { "Ac", 227.0f }/*Actinium*/, { "Th", 232.04f }/*Thorium*/, { "Pa", 231.04f }/*Protactinium*/, { "U", 238.03f }/*Uranium*/,
                { "Np", 237.0f }/*Neptunium*/, { "Pu", 244.0f }/*Plutonium*/, { "Am", 243.0f }/*Americium*/, { "Cm", 247.0f }/*Curium*/,
                { "Bk", 247.0f }/*Berkelium*/, { "Cf", 251.0f }/*Californium*/, { "Es", 252.0f }/*Einsteinium*/, { "Fm", 257.0f }/*Fermium*/,
                { "Md", 258.0f }/*Mendelevium*/, { "No", 259.0f }/*Nobelium*/, { "Lr", 266.0f }/*Lawrencium*/, { "Rf", 267.0f }/*Rutherfordium*/,
                { "Db", 268.0f }/*Dubnium*/, { "Sg", 269.0f }/*Seaborgium*/, { "Bh", 270.0f }/*Bohrium*/, { "Hs", 277.0f }/*Hassium*/,
            };

            if (atomicMasses.TryGetValue(elementSymbol, out float mass))
            {
                return mass;
            }
            else
            {
                // handle case where the element symbol is not in the dictionary
                throw new ArgumentException("Element symbol not found: " + elementSymbol);
            }
        }
    }    
}
