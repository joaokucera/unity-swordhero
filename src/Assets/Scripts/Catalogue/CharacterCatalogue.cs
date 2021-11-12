using System;
using UnityEngine;

namespace Catalogue
{
    [CreateAssetMenu(fileName = "New CharacterCatalogue", menuName = "Catalogues/CharacterCatalogue")]
    public class CharacterCatalogue : ScriptableObject
    {
        [SerializeField] private CharacterSettings characterSettings;

        public CharacterSettings GetCharacterSettings()
        {
            return characterSettings;
        }
    }

    [Serializable]
    public class CharacterSettings
    {
        public int CharacterMaxHealth;
    }
}