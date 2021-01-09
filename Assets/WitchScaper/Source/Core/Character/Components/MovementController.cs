using UnityEngine;
using WitchScaper.Core.State;

namespace WitchScaper.Core.Character
{
    public class MovementController
    {
        private readonly Rigidbody2D _rigidbody2D;
        private readonly CharacterControllerData _controllerData;
        private readonly GameState _state;

        private Vector2 _movement;

        public MovementController(Rigidbody2D rigidbody2D, CharacterControllerData controllerData, GameState state)
        {
            _rigidbody2D = rigidbody2D;
            _controllerData = controllerData;
            _state = state;
        }

        public void Update()
        {
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");
        }

        public void ApplyMovement()
        {
            if (_state.CurrentState != GameState.State.Battle)
            {
                return;
            }
            
            _rigidbody2D.MovePosition(_rigidbody2D.position + _movement * (_controllerData.Speed * Time.fixedDeltaTime));
        }
    }
}