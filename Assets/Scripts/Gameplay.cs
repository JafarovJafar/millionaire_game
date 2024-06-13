using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gameplay : MonoBehaviour
{
    public event UnityAction Finished;

    [SerializeField] private QuestionView _questionView;
    [SerializeField] private AnswersView _answersView;

    [SerializeField] private bool _playerAnswered;
    [SerializeField] private bool _enemyAnswered;

    private LevelData _currentLevel;

    private int[] _questionsIndexesOrder;
    private int _currentQuestionIndex;

    /// <summary>
    /// Начать core-loop
    /// </summary>
    /// <param name="level">Уровень, который необходимо проиграть</param>
    public void StartGameLoop(LevelData level)
    {
        _currentLevel = level;

        GetRandomQuestionsIndexes(_currentLevel);

        StartCoroutine(MainCoroutine());
    }

    private void GetRandomQuestionsIndexes(LevelData level)
    {
        var indexes = new List<int>();
        for (int i = 0; i < level.Questions.Count; i++)
        {
            indexes.Add(i);
        }

        _questionsIndexesOrder = new int[indexes.Count];
        for (int i = 0; i < _questionsIndexesOrder.Length; i++)
        {
            int randomIndex = Random.Range(0, indexes.Count - 1);

            _questionsIndexesOrder[i] = indexes[randomIndex];

            indexes.RemoveAt(randomIndex);
        }
    }

    // core-loop
    private IEnumerator MainCoroutine()
    {
        while (_currentQuestionIndex < _questionsIndexesOrder.Length)
        {
            // устанавливаем вопрос и вызываем анимацию появления
            // ждем, пока оба игрока ответят
            // вызываем анимацию исчезания

            // Выбор вопроса
            int currentQuestionIndex = _questionsIndexesOrder[_currentQuestionIndex];
            QuestionData currentQuestion = _currentLevel.Questions[currentQuestionIndex];

            // Показываем вопрос
            yield return ShowQuestion(currentQuestion);
            // Показываем ответы
            yield return ShowAnswers(currentQuestion);

            // ждем, пока игроки ответят
            yield return WaitPlayersAnswers();

            // скрываем ответы
            yield return HideAnswers();
            // скрываем вопросы
            yield return HideQuestion();

            _currentQuestionIndex++;
        }

        Finished?.Invoke();
    }

    private IEnumerator ShowQuestion(QuestionData currentQuestion)
    {
        bool questionViewAppeared = false;
        UnityAction viewAppearedAction = () => questionViewAppeared = true;
        _questionView.Appeared += viewAppearedAction;
        _questionView.SetQuestion(currentQuestion.Question);
        _questionView.Appear();

        while (!questionViewAppeared) yield return null;
        _questionView.Appeared -= viewAppearedAction;
    }

    private IEnumerator ShowAnswers(QuestionData currentQuestion)
    {
        bool answersViewAppeared = false;
        UnityAction viewAppearedAction = () => answersViewAppeared = true;
        _answersView.Appeared += viewAppearedAction;
        _answersView.SetAnswers(currentQuestion.Answers);
        _answersView.Appear();

        while (!answersViewAppeared) yield return null;
        _answersView.Appeared -= viewAppearedAction;
    }

    private IEnumerator WaitPlayersAnswers()
    {
        while (!_playerAnswered || !_enemyAnswered)
        {
            yield return null;
        }

        _playerAnswered = false;
        _enemyAnswered = false;
    }

    private IEnumerator HideAnswers()
    {
        bool answersViewDisAppeared = false;
        UnityAction viewDisAppearedAction = () => answersViewDisAppeared = true;
        _answersView.DisAppeared += viewDisAppearedAction;
        _answersView.DisAppear();

        while (!answersViewDisAppeared) yield return null;
        _answersView.DisAppeared -= viewDisAppearedAction;
    }

    private IEnumerator HideQuestion()
    {
        bool questionViewDisAppeared = false;
        UnityAction viewDisAppearedAction = () => questionViewDisAppeared = true;
        _questionView.DisAppeared += viewDisAppearedAction;
        _questionView.DisAppear();

        while (!questionViewDisAppeared) yield return null;
        _questionView.Appeared -= viewDisAppearedAction;
    }
}