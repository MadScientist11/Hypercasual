using System.Collections;
using Hypercasual.Food;
using Hypercasual.Services;
using UnityEngine;
using VContainer;

namespace Hypercasual.Player
{
    public class PlayerGrabFood : MonoBehaviour
    {
        [SerializeField] private PlayerReachZone playerReachZone;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private Transform[] _handIKTargets;
        [SerializeField] private Transform _hand;
        [SerializeField] private Transform _fallInBasketPoint;

        private FoodView _currentFood;
        private ILevelManager _levelManager;
        private IInputService _inputService;

        private readonly float _inHandScaleFactor = 0.6f;

        [Inject]
        public void Construct(ILevelManager levelManager, IInputService inputService)
        {
            _inputService = inputService;
            _levelManager = levelManager;
        }

        private void Start()
        {
            _inputService.OnLeftMouseButtonClicked += TryGrabFood;
        }

        private void OnDestroy()
        {
            _inputService.OnLeftMouseButtonClicked -= TryGrabFood;
        }

        private void TryGrabFood()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(_inputService.MousePosition);

            if (Physics.Raycast(ray, out hit, 100))
            {
                Transform objectHit = hit.transform;
                if (objectHit.TryGetComponent<FoodView>(out var food))
                {
                    if (playerReachZone.Contains(food.transform.position))
                        GrabFood(food);
                }
            }
        }

        private void GrabFood(FoodView food)
        {
            if (food.IsProcessed) return;
            food.IsProcessed = true;
            SetIK(food);

            _currentFood = food;
            _playerAnimator.PlayGrabFoodAnimation(() => PlaceInTheHand(food));
        }

        private void PlaceInTheHand(FoodView food)
        {
            food.CachedTransform.SetParent(_hand);
            food.CachedTransform.localPosition = Vector3.zero;
            StartCoroutine(PlaceItemInBasket());
        }


        private IEnumerator PlaceItemInBasket()
        {
            yield return new WaitForSeconds(1.5f);
            
            _currentFood.ResetScaleAndRotation();
            _currentFood.CachedTransform.localScale *= _inHandScaleFactor;
            _currentFood.CachedTransform.position = _fallInBasketPoint.position;
            _currentFood.GetComponent<Rigidbody>().isKinematic = false;
            _currentFood.CachedTransform.SetParent(null);
            
            _levelManager.CheckFood(_currentFood);
            
            _currentFood = null;
        }

        private void SetIK(FoodView food)
        {
            foreach (Transform handIKTarget in _handIKTargets)
            {
                handIKTarget.position = food.transform.position;
            }
        }
    }
}