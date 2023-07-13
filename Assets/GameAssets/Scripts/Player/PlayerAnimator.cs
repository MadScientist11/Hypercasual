using UnityEngine;

namespace Hypercasual.Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private const string WinState = "Win";
        private const string IdleState = "Idle";
        private static readonly int GrabItem = Animator.StringToHash("GrabItem");
        private static readonly int GrabItemBack = Animator.StringToHash("GrabItemBack");

        public void PlayGrabItemAnimation() =>
            _animator.SetTrigger(GrabItem);

        public void PlayGrabItemBackAnimation() =>
            _animator.SetTrigger(GrabItemBack);

        public void PlayWinAnimation()
        {
            _animator.Play(WinState);
        }

        public void ResetAnimatorState()
        {
            _animator.Play(IdleState);
        }

     
    }
}