using System;
using UniRx;
using UnityEngine;
using System.Reflection;

namespace Game
{
    [Obfuscation(Exclude = true)]
    public class InputHandler : MonoBehaviour
    {
        public static InputHandler Instance { get; private set; }

        private readonly Subject<Vector2> _onDragDeltaSubject = new Subject<Vector2>();
        private readonly ReactiveProperty<bool> _isTouching = new ReactiveProperty<bool>();

        public IObservable<Vector2> OnDragDelta => _onDragDeltaSubject;
        public IReadOnlyReactiveProperty<bool> IsTouching => _isTouching;

        private Vector2 _lastInputPosition;
        private bool _currentlyDragging;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Update()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                HandleTouch(touch.phase, touch.position);
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartDrag(Input.mousePosition);
                }
                else if (Input.GetMouseButton(0))
                {
                    ContinueDrag(Input.mousePosition);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    EndDrag();
                }
                else
                {
                    _isTouching.Value = false;
                }
            }
        }

        private void HandleTouch(TouchPhase phase, Vector2 position)
        {
            switch (phase)
            {
                case TouchPhase.Began:
                    StartDrag(position);
                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    ContinueDrag(position);
                    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    EndDrag();
                    break;
            }
        }

        private void StartDrag(Vector2 position)
        {
            _currentlyDragging = true;
            _lastInputPosition = position;
            _isTouching.Value = true;
        }

        private void ContinueDrag(Vector2 position)
        {
            if (!_currentlyDragging)
                return;

            Vector2 delta = position - _lastInputPosition;
            _lastInputPosition = position;

            _onDragDeltaSubject.OnNext(delta);
            _isTouching.Value = true;
        }

        private void EndDrag()
        {
            _currentlyDragging = false;
            _isTouching.Value = false;
        }

        public Vector2 GetInputPosition()
        {
            return _lastInputPosition;
        }
    }
}
