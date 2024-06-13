using UnityEngine;
using UnityEngine.Events;

public class AnswersView : MonoBehaviour
{
    public event UnityAction Appeared;
    public event UnityAction DisAppeared;

    [SerializeField] private AnswerView[] _views;

    public void SetAnswers(string[] answers)
    {
        for (int i = 0; i < _views.Length; i++)
        {
            _views[i].SetText(answers[i]);
        }
    }

    public void Appear()
    {
        Appeared?.Invoke();
    }

    public void DisAppear()
    {
        DisAppeared?.Invoke();
    }
}