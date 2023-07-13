using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace ScpQuizUltimate
{
    public class StartUpButtonsAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform[] _buttons;
        [SerializeField] private Ease _easeType;

        private IEnumerator Start()
        {
            foreach (var button in _buttons)
            {
                button.localScale = Vector3.zero;
            }
            foreach (var button in _buttons)
            {
                button.DOScale(Vector3.one,.35f).SetEase(_easeType);
                yield return new WaitForSeconds(.1f);
            }
        }
    }
}
