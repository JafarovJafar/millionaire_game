using UnityEngine;
using UnityEngine.Events;

public class GameScreen : MonoBehaviour
{
    public event UnityAction QuestionAppeared;
    public event UnityAction QuestionDisAppeared;

    public event UnityAction AnswersAppeared;
    public event UnityAction AnswersDisAppeared;
    public event UnityAction<int> AnswerClicked;

    [SerializeField] private GameScreenHeader _header;
    [SerializeField] private QuestionView _question;
    [SerializeField] private AnswersView _answers;

    public void Init()
    {
        _question.Init();
        _question.Appeared += () => QuestionAppeared?.Invoke();
        _question.DisAppeared += () => QuestionDisAppeared?.Invoke();

        _answers.Init();
        _answers.Appeared += () => AnswersAppeared?.Invoke();
        _answers.DisAppeared += () => AnswersDisAppeared?.Invoke();
        _answers.AnswerClicked += (answer) => AnswerClicked?.Invoke(answer);
    }

    public void ShowHeader() => _header.Show();
    public void HideHeader() => _header.Hide();
    public void SetHeroScore(int score) => _header.SetHeroScore(score);
    public void SetEnemyScore(int score) => _header.SetEnemyScore(score);

    public void ShowQuestion() => _question.Appear();
    public void HideQuestion(bool immediate = false) => _question.DisAppear(immediate);
    public void SetQuestionText(string questionText) => _question.SetQuestion(questionText);

    public void ShowAnswers() => _answers.Appear();
    public void HideAnswers(bool immediate = false) => _answers.DisAppear(immediate);
    public void SetAnswers(string[] answers) => _answers.SetAnswers(answers);
    public void SelectAnswer(int answerIndex, int playerIndex) => _answers.Select(answerIndex, playerIndex);
    public void DeSelectAnswers() => _answers.DeSelect();
}