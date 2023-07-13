using UnityEngine;

namespace Hypercasual
{
    public class CameraAnimator : MonoBehaviour
    {
        [SerializeField] private Animator _cameraAnimator;
        private const string DefaultCameraState = "DefaultCamera";
        private const string WinCameraState = "WinCamera";

        public void SwitchToDefaultCamera()
        {
            _cameraAnimator.Play(DefaultCameraState);
        }

        public void SwitchToWinCamera()
        {
            _cameraAnimator.Play(WinCameraState);
        }
    }
}