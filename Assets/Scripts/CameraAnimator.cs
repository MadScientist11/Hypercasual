using UnityEngine;

namespace Hypercasual
{
    public class CameraAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _cameraAnimator;

        public void SwitchToDefaultCamera()
        {
            _cameraAnimator.Play("WinCamera");
        }

        public void SwitchToWinCamera()
        {
            _cameraAnimator.Play("WinCamera");
        }
    }
}