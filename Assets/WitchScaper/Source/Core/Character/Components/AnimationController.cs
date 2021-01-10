using System;
using UnityEngine;

namespace WitchScaper.Core.Character
{
    public class AnimationController
    {
        private readonly Animator _animator;

        public enum State
        {
            Moving,
            Idle
        }
        
        public AnimationController(Animator animator)
        {
            _animator = animator;
        }

        public void SetState(State state)
        {
            switch (state)
            {
                case State.Moving:
                    _animator.SetFloat("Speed", 1f);
                    break;
                case State.Idle:
                    _animator.SetFloat("Speed", 0f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}