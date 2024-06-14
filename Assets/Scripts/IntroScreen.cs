using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using EasyButtons;

public class IntroScreen : MonoBehaviour
{
    public event UnityAction ShowFinished;

    [SerializeField] private AnimationCurve _showCurve;
    [SerializeField] private float _showDuration;
    [SerializeField] private RectTransform _heroTransform;
    [SerializeField] private RectTransform _enemyTransform;
    [SerializeField] private Vector3 _heroStartPos;
    [SerializeField] private Vector3 _heroEndPos;
    [SerializeField] private Vector3 _enemyStartPos;
    [SerializeField] private Vector3 _enemyEndPos;

    private bool _isInAnimation;

    [Button]
    public void Show()
    {
        if (_isInAnimation) return;
        _isInAnimation = true;

        _heroTransform.localPosition = _heroStartPos;
        _enemyTransform.localPosition = _enemyStartPos;

        Sequence sequence = DOTween.Sequence();
        sequence.Insert(0f, _heroTransform.DOLocalMove(_heroEndPos, _showDuration).SetEase(_showCurve));
        sequence.Insert(0f, _enemyTransform.DOLocalMove(_enemyEndPos, _showDuration).SetEase(_showCurve));
        sequence.OnComplete(() =>
        {
            _isInAnimation = false;
            ShowFinished?.Invoke();
        });
    }
}