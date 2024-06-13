using UnityEngine;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    public event UnityAction ContinueClicked;

    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private TextMeshProUGUI _heroScore;
    [SerializeField] private TextMeshProUGUI _enemyScore;
    [SerializeField] private Button _continueButton;

    [SerializeField] private float _animationDuration;

    private void Awake()
    {
        _continueButton.onClick.AddListener(ContinueButton_Clicked);
    }

    private void ContinueButton_Clicked()
    {
        ContinueClicked?.Invoke();
    }

    public void Show
    (
        PlayerData heroData,
        PlayerData enemyData,
        bool immediate
    )
    {
        if (heroData.Score > enemyData.Score)
        {
            _header.text = "Победа!!!";
        }
        else if (heroData.Score < enemyData.Score)
        {
            _header.text = "Поражение...";
        }
        else
        {
            _header.text = "Ничья";
        }

        _heroScore.text = heroData.Score.ToString();
        _enemyScore.text = enemyData.Score.ToString();

        float duration = immediate ? 0f : _animationDuration;

        DoAnimation
        (
            1f,
            duration,
            () =>
            {
                _canvasGroup.blocksRaycasts = true;
            }
        );
    }

    public void Hide(bool immediate = false)
    {
        float duration = immediate ? 0f : _animationDuration;
        DoAnimation
        (
            0f,
            duration,
            () =>
            {
                _canvasGroup.blocksRaycasts = false;
            }
        );
    }

    private void DoAnimation
    (
        float goalAlpha,
        float duration,
        UnityAction callback
    )
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(_canvasGroup.DOFade(goalAlpha, duration));
        sequence.OnComplete(() => callback?.Invoke());
    }
}