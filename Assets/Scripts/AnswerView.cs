using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnswerView : MonoBehaviour
{
    [SerializeField] private Image _background;
    [SerializeField] private TextMeshProUGUI _answerText;

    public void SetText(string text)
    {
        _answerText.text = text;
    }

    public void Highlight(Color highlightColor)
    {

    }
}