using System.Collections;
using DG.Tweening;
using Hypercasual.Level;
using Hypercasual.Services;
using TMPro;
using UnityEngine;
using VContainer;

namespace Hypercasual.Player
{
    public class PlayerUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _foodCounterText;
        private Vector3 _foodCounterInitialPosition;
        private ILevelService _levelService;

        [Inject]
        public void Construct(ILevelService levelService)
        {
            _levelService = levelService;
        }

        private void Start()
        {
            _foodCounterInitialPosition = _foodCounterText.transform.position;
            _levelService.LevelStatusChanged += AnimateFoodCounterText;
        }

        private void OnDestroy()
        {
            _levelService.LevelStatusChanged -= AnimateFoodCounterText;
        }

        private void AnimateFoodCounterText(LevelInfo info)
        {
            StartCoroutine(FoodCounterAnimation());
        }

        private IEnumerator FoodCounterAnimation()
        {
            ResetFoodCounterText();
            Sequence sequence = DOTween.Sequence();
            sequence.Append(_foodCounterText.transform.DOMoveY(1.35f, 1).SetEase(Ease.OutQuint));
            sequence.Append(_foodCounterText.DOFade(0, 1));
            yield return new WaitForSeconds(1);
            
        }

        private void ResetFoodCounterText()
        {
            _foodCounterText.ChangeAlpha(1);
            _foodCounterText.transform.position = _foodCounterInitialPosition;
        }
    }
}