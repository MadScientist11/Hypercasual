using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Hypercasual.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        
        private const string WinState = "Win";
        private const string IdleState = "Idle";
        private static readonly int GrabFood = Animator.StringToHash("GrabItem");
        
        private Action _onGrabFoodCompleted;

        public void PlayGrabFoodAnimation(Action onCompleted = null)
        {
            _onGrabFoodCompleted = onCompleted;
            _animator.SetTrigger(GrabFood);
        }

        public void PlayWinAnimation()
        {
            _animator.Play(WinState);
        }

        public void ResetAnimatorState()
        {
            _animator.Play(IdleState);
        }

        [UsedImplicitly]
        private void AnimationEventCallback_OnGrabItemCompleted()
        {
            _onGrabFoodCompleted?.Invoke();
        }
    }
}