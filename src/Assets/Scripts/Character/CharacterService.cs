using Catalogue;
using UnityEngine;
using Widget;

namespace Character
{
    public interface ICharacterService : IGameService
    {
        void RegisterCharacter(CharacterController characterController);
        bool TryGetCharacterController(out CharacterController characterController);
        public CharacterSettings GetCharacterSettings();
    }

    public class CharacterService : ICharacterService
    {
        private readonly CharacterCatalogue _characterCatalogue;
        
        private CharacterController _characterController;

        public CharacterService()
        {
            // TODO: It would be removed by installing scriptable objects with zenject's scene context
            _characterCatalogue = Resources.Load<CharacterCatalogue>("CharacterCatalogue");
        }

        public void RegisterCharacter(CharacterController characterController)
        {
            _characterController = characterController;
        }

        public bool TryGetCharacterController(out CharacterController characterController)
        {
            characterController = _characterController;
            return characterController != null;
        }

        public CharacterSettings GetCharacterSettings()
        {
            return _characterCatalogue.GetCharacterSettings();
        }
    }
}