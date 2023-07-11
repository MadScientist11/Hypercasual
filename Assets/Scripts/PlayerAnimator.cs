using UnityEngine;

namespace Hypercasual
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private static readonly int GrabItem = Animator.StringToHash("GrabItem");
        private static readonly int GrabItemBack = Animator.StringToHash("GrabItemBack");

        public void PlayGrabItemAnimation() => 
            _animator.SetTrigger(GrabItem);

        public void PlayGrabItemBackAnimation() =>
            _animator.SetTrigger(GrabItemBack);
    }
}
