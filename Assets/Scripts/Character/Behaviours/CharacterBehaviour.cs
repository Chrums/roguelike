using System;
using Fizz6.Actor;

namespace Fizz6.Roguelike.Character.Behaviours
{
    [Serializable]
    public abstract class CharacterBehaviour : Behaviour
    {
        protected Character Character { get; private set; }
            
        public void Initialize(Character character) => Character = character;
    }
}