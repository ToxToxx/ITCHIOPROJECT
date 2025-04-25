using System;
using UniRx;
using UnityEngine;
using System.Linq;
using System.Reflection;

namespace Game
{
    [ObfuscationAttribute(Exclude = true)]
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler Instance { get; private set; }

        [Header("Настройки")]
        [SerializeField] LayerMask _targetLayer;
        [SerializeField] private float _rayDistance = 100f;

        private Camera _mainCamera;
        private readonly Subject<GameObject> _onClickSubject = new Subject<GameObject>();
        public IObservable<GameObject> OnObjectClicked => _onClickSubject;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    ProcessInputAtPosition(touch.position);
                }
            }
            else if (Input.GetMouseButtonDown(0))
            {
                ProcessInputAtPosition(Input.mousePosition);
            }
        }

        public void OnClick()
        {
            Vector2 inputPosition = GetInputPosition();
            if (inputPosition != Vector2.zero)
            {
                ProcessInputAtPosition(inputPosition);
            }
        }

        private Vector2 GetInputPosition()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                return touch.position;
            }
            return Input.mousePosition;
        }

        private void ProcessInputAtPosition(Vector2 inputPosition)
        {
            if (inputPosition == Vector2.zero)
                return;

            Ray ray = _mainCamera.ScreenPointToRay(inputPosition);
            CheckObjectUnderPointer(ray);
        }

        private void CheckObjectUnderPointer(Ray ray)
        {
            Vector2 rayOrigin = ray.origin;
            Vector2 rayDirection = ray.direction;

            // Получаем все объекты под лучом
            RaycastHit2D[] hits = Physics2D.RaycastAll(rayOrigin, rayDirection, _rayDistance, _targetLayer);

            if (hits.Length > 0)
            {
                // Сортируем по убыванию Sorting Order и, при равенстве, по Z-позиции (меньше Z — выше)
                var sortedHits = hits
                    .Where(h => h.collider != null)
                    .OrderByDescending(h => h.collider.GetComponent<SpriteRenderer>()?.sortingOrder ?? 0)
                    .ThenBy(h => h.point.y); // Предполагаем 2D, где меньший Y (или Z в 3D) — ближе к камере

                foreach (var hit in sortedHits)
                {
                    GameObject clickedObject = hit.collider.gameObject;
                    if (clickedObject.TryGetComponent<IClickable>(out var clickable))
                    {
                        clickable.CheckClick();
                        return; // Обрабатываем только первый IClickable
                    }
                }

                // Если ни один IClickable не найден, отправляем первый объект
                _onClickSubject.OnNext(sortedHits.First().collider.gameObject);
            }
        }

        public void SetInputSettings(LayerMask targetLayer, float rayDistance)
        {
            _targetLayer = targetLayer;
            _rayDistance = rayDistance;
        }
    }
}