using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class QuestionView : MonoBehaviour
{
    public event UnityAction Appeared;
    public event UnityAction DisAppeared;

    [SerializeField] private TextMeshProUGUI _questionText;

    public void Appear()
    {
        Appeared?.Invoke();
    }

    public void DisAppear()
    {
        DisAppeared?.Invoke();
    }

    public void SetQuestion(string question)
    {
        _questionText.text = question;
    }
}