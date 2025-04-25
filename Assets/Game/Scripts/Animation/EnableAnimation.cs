using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class EnableAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform[] _uiElements;

        [Header("Entry Animation")]
        [SerializeField] private float _entryDelay = 0.1f;
        [SerializeField] private float _moveDuration = 0.8f;
        [SerializeField] private float _popDuration = 0.6f;
        [SerializeField] private float _popOvershoot = 1.3f;
        [SerializeField] private Ease _popEase = Ease.OutBack;
        [SerializeField] private float _startOffsetX = -800f;

        [Header("Additional Effects")]
        [SerializeField] private bool _usePulseEffect = true;
        [SerializeField] private float _pulseDuration = 0.8f;
        [SerializeField] private float _pulseScale = 1.1f;
        [SerializeField] private int _pulseLoops = 2;
        [SerializeField] private bool _useColorEffect = true;
        [SerializeField] private Color _targetColor = new(0f, 1f, 1f, 1f);
        [SerializeField] private float _colorAnimDuration = 0.6f;

        private Vector2[] _originalPositions;
        private Color[] _originalColors;

        private void OnEnable()
        {
            if (_uiElements == null || _uiElements.Length == 0)
            {
                Debug.LogWarning("No UI elements assigned for EnableAnimation!", this);
                return;
            }

            CacheOriginalValues();
            ResetElements();
            PlayEntryAnimation();
        }

        private void OnDisable()
        {
            DOTween.Kill(this);
        }

        private void CacheOriginalValues()
        {
            _originalPositions = new Vector2[_uiElements.Length];
            _originalColors = new Color[_uiElements.Length];

            for (int i = 0; i < _uiElements.Length; i++)
            {
                if (_uiElements[i] == null) continue;

                _originalPositions[i] = _uiElements[i].anchoredPosition;

                if (_uiElements[i].TryGetComponent<Image>(out Image image))
                {
                    _originalColors[i] = image.color;
                }
            }
        }

        private void ResetElements()
        {
            foreach (RectTransform element in _uiElements)
            {
                if (element == null) continue;

                element.anchoredPosition = _originalPositions[System.Array.IndexOf(_uiElements, element)]
                    + new Vector2(_startOffsetX, 0);
                element.localScale = Vector3.zero;

                if (element.TryGetComponent<Image>(out Image image))
                {
                    image.color = _originalColors[System.Array.IndexOf(_uiElements, element)];
                }
            }
        }

        private void PlayEntryAnimation()
        {
            Sequence mainSequence = DOTween.Sequence().SetId(this);

            for (int i = 0; i < _uiElements.Length; i++)
            {
                if (_uiElements[i] == null) continue;

                int index = i;
                float delay = index * _entryDelay;

                Vector2 targetPosition = _originalPositions[index];
                mainSequence.Insert(delay,
                    _uiElements[index].DOAnchorPos(targetPosition, _moveDuration)
                        .SetEase(Ease.OutCubic));

                mainSequence.Insert(delay,
                    _uiElements[index].DOScale(_popOvershoot, _popDuration * 0.5f)
                        .SetEase(_popEase));
                mainSequence.Insert(delay + _popDuration * 0.5f,
                    _uiElements[index].DOScale(1f, _popDuration * 0.5f)
                        .SetEase(Ease.InBack));

                if (_usePulseEffect)
                {
                    mainSequence.InsertCallback(delay + _moveDuration, () =>
                    {
                        _uiElements[index].DOPunchScale(
                            Vector3.one * (_pulseScale - 1f),
                            _pulseDuration,
                            _pulseLoops,
                            1f
                        ).SetId(this);
                    });
                }

                if (_useColorEffect && _uiElements[index].TryGetComponent<Image>(out Image image))
                {
                    Sequence colorSequence = DOTween.Sequence()
                        .Append(image.DOColor(_targetColor, _colorAnimDuration * 0.5f))
                        .Append(image.DOColor(_originalColors[index], _colorAnimDuration * 0.5f))
                        .SetId(this);

                    mainSequence.Insert(delay + _popDuration, colorSequence);
                }
            }
        }
    }
}