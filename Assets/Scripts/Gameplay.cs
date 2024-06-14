using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gameplay : MonoBehaviour
{
    public event UnityAction Finished;

    [SerializeField] private IntroScreen _introScreen;
    [SerializeField] private GameScreenHeader _gameScreenHeader;
    [SerializeField] private ScoreView _heroGameScore;
    [SerializeField] private ScoreView _enemyGameScore;
    [SerializeField] private QuestionView _questionView;
    [SerializeField] private AnswersView _answersView;
    [SerializeField] private WinScreen _winScreen;

    [SerializeField] private PlayerData _hero;
    [SerializeField] private PlayerData _enemy;

    [SerializeField] private BaseEnemy _enemyLogic;

    private LevelData _currentLevel;

    private int[] _questionsIndexesOrder;
    private int _currentQuestionIndex;

    private int _heroPlayerIndex = 0;
    private int _enemyPlayerIndex = 1;

    /// <summary>
    /// Начать core-loop
    /// </summary>
    /// <param name="level">Уровень, который необходимо проиграть</param>
    public void StartGameLoop(LevelData level)
    {
        _currentLevel = level;
        _currentQuestionIndex = 0;

        _hero.Score = 0;
        _enemy.Score = 0;

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
        UpdateScoreView();

        _gameScreenHeader.Hide();

        _winScreen.Hide(true);
        _questionView.DisAppear(true);
        _answersView.DisAppear(true);

        _answersView.AnswerClicked += Answer_Clicked;

        yield return ShowIntro();

        _gameScreenHeader.Show();

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

            _enemyLogic.Answer(currentQuestion, (answerIndex) =>
            {
                _enemy.SelectedAnswer = true;
                _enemy.AnswerIndex = answerIndex;
                _answersView.Select(answerIndex, _enemyPlayerIndex);
            });

            // ждем, пока игроки ответят
            yield return WaitPlayersAnswers();

            CheckAnswer(_hero, currentQuestion);
            CheckAnswer(_enemy, currentQuestion);

            ResetAnswer(_hero);
            ResetAnswer(_enemy);

            UpdateScoreView();

            // скрываем ответы
            yield return HideAnswers();
            // скрываем вопросы
            yield return HideQuestion();

            // очищаем выбор игроков с ответов
            ClearSelections();

            _currentQuestionIndex++;
        }

        _answersView.AnswerClicked -= Answer_Clicked;

        _winScreen.Show(_hero, _enemy, false);
        _winScreen.ContinueClicked += WinScreen_ContinueClicked;
    }

    private IEnumerator ShowIntro()
    {
        bool showFinished = false;
        UnityAction action = () => showFinished = true;
        _introScreen.ShowFinished += action;
        _introScreen.Show();

        while (!showFinished) yield return null;
        _introScreen.ShowFinished -= action;
    }

    private void WinScreen_ContinueClicked()
    {
        _winScreen.ContinueClicked -= WinScreen_ContinueClicked;
        Finished?.Invoke();
    }

    private void Answer_Clicked(int answerIndex)
    {
        _hero.SelectedAnswer = true;
        _hero.AnswerIndex = answerIndex;
        _answersView.Select(answerIndex, _heroPlayerIndex);
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
        while (!_hero.SelectedAnswer) yield return null;
        while (!_enemy.SelectedAnswer) yield return null;
    }

    private void CheckAnswer(PlayerData playerData, QuestionData questionData)
    {
        if (playerData.AnswerIndex == questionData.CorrectAnswerIndex)
        {
            playerData.Score++;
        }
    }

    private void ResetAnswer(PlayerData playerData)
    {
        playerData.AnswerIndex = -1;
        playerData.SelectedAnswer = false;
    }

    private void UpdateScoreView()
    {
        _heroGameScore.SetScore(_hero.Score);
        _enemyGameScore.SetScore(_enemy.Score);
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

    private void ClearSelections()
    {
        _answersView.DeSelect();
    }
}