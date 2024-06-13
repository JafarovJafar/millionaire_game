using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class AnswerView : MonoBehaviour
{
    public event UnityAction<AnswerView> Clicked;

    public Vector3 Position => _transform.anchoredPosition3D;
    public RectTransform Transform => _transform;

    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _answerText;
    [SerializeField] private Image[] _playerSelectionImages;

    private RectTransform _transform;

    public void Init()
    {
        _transform = GetComponent<RectTransform>();
        _button.onClick.AddListener(() => Clicked?.Invoke(this));
    }

    public void SetText(string text)
    {
        _answerText.text = text;
    }

    public void ShowText() => _answerText.gameObject.SetActive(true);
    public void HideText() => _answerText.gameObject.SetActive(false);

    public void Select(int playerIndex)
    {
        _playerSelectionImages[playerIndex].gameObject.SetActive(true);
    }

    public void DeSelect()
    {
        foreach (var image in _playerSelectionImages)
        {
            image.gameObject.SetActive(false);
        }
    }
}