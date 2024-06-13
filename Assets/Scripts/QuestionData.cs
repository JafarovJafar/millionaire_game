using UnityEngine;

[CreateAssetMenu(menuName = "QuestionData")]
public class QuestionData : ScriptableObject
{
    public string Question => _question;
    public string[] Answers => _answers;
    public int CorrectAnswerIndex => _correctAnswerIndex;

    [SerializeField] private string _question;
    [SerializeField] private string[] _answers;
    [SerializeField] private int _correctAnswerIndex;
}