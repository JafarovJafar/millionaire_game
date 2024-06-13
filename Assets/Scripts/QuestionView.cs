using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using EasyButtons;
using TMPro;

public class QuestionView : MonoBehaviour
{
    public event UnityAction Appeared;
    public event UnityAction DisAppeared;

    [SerializeField] private TextMeshProUGUI _questionText;

    [SerializeField] private float _appearDuration;
    [SerializeField] private Ease _appearEase = Ease.InOutSine;

    [SerializeField] private float _disAppearDuration;
    [SerializeField] private Ease _disAppearEase = Ease.InOutSine;

    private bool _isInAnimation;

    private RectTransform _transform;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    [Button]
    public void Appear()
    {
        DoAnimation
        (
            1f,
            _appearEase,
            _appearDuration,
            () => Appeared?.Invoke()
        );
    }

    [Button]
    public void DisAppear()
    {
        DoAnimation
        (
            0f,
            _disAppearEase,
            _disAppearDuration,
            () => DisAppeared?.Invoke()
        );
    }

    private void DoAnimation
    (
        float size,
        Ease ease,
        float duration,
        UnityAction callback
    )
    {
        if (_isInAnimation) return;
        _isInAnimation = true;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(_transform.DOScale(size, duration)).SetEase(ease);
        sequence.Play();
        sequence.OnComplete(() =>
        {
            _isInAnimation = false;
            callback();
        });
    }

    public void SetQuestion(string question)
    {
        _questionText.text = question;
    }
}