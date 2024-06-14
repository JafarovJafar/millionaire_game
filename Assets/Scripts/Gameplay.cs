using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Gameplay : MonoBehaviour
{
    public event UnityAction Finished;

    [SerializeField] private PlayerData _hero;
    [SerializeField] private PlayerData _enemy;

    private IntroScreen _introScreen;
    private GameScreen _gameScreen;
    private WinScreen _winScreen;

    private BaseEnemy _enemyLogic;

    private LevelData _currentLevel;

    private int[] _questionsIndexesOrder;
    private int _currentQuestionIndex;

    private int _heroPlayerIndex = 0;
    private int _enemyPlayerIndex = 1;

    /// <summary>
    /// Инициализация кор-лупа
    /// </summary>
    /// <param name="introScreen">Начальный экран</param>
    /// <param name="gameScreen">Экран геймплея</param>
    /// <param name="winScreen">Экран конца битвы</param>
    /// <param name="enemy">Логика врага</param>
    public void Init
    (
        IntroScreen introScreen,
        GameScreen gameScreen,
        WinScreen winScreen,
        BaseEnemy enemy
    )
    {
        _introScreen = introScreen;
        _gameScreen = gameScreen;
        _winScreen = winScreen;
        _enemyLogic = enemy;
    }

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

        _gameScreen.HideHeader();

        _winScreen.Hide(true);
        _gameScreen.HideQuestion(true);
        _gameScreen.HideAnswers(true);

        _gameScreen.AnswerClicked += Answer_Clicked;

        yield return ShowIntro();

        _gameScreen.ShowHeader();

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
                _gameScreen.SelectAnswer(answerIndex, _enemyPlayerIndex);
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

        _gameScreen.AnswerClicked -= Answer_Clicked;

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
        _gameScreen.SelectAnswer(answerIndex, _heroPlayerIndex);
    }

    private IEnumerator ShowQuestion(QuestionData currentQuestion)
    {
        bool questionViewAppeared = false;
        UnityAction viewAppearedAction = () => questionViewAppeared = true;
        _gameScreen.QuestionAppeared += viewAppearedAction;
        _gameScreen.SetQuestionText(currentQuestion.Question);
        _gameScreen.ShowQuestion();

        while (!questionViewAppeared) yield return null;
        _gameScreen.QuestionAppeared -= viewAppearedAction;
    }

    private IEnumerator ShowAnswers(QuestionData currentQuestion)
    {
        bool answersViewAppeared = false;
        UnityAction viewAppearedAction = () => answersViewAppeared = true;
        _gameScreen.AnswersAppeared += viewAppearedAction;
        _gameScreen.SetAnswers(currentQuestion.Answers);
        _gameScreen.ShowAnswers();

        while (!answersViewAppeared) yield return null;
        _gameScreen.AnswersAppeared -= viewAppearedAction;
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
        _gameScreen.SetHeroScore(_hero.Score);
        _gameScreen.SetEnemyScore(_enemy.Score);
    }

    private IEnumerator HideAnswers()
    {
        bool answersViewDisAppeared = false;
        UnityAction viewDisAppearedAction = () => answersViewDisAppeared = true;
        _gameScreen.AnswersDisAppeared += viewDisAppearedAction;
        _gameScreen.HideAnswers();

        while (!answersViewDisAppeared) yield return null;
        _gameScreen.AnswersDisAppeared -= viewDisAppearedAction;
    }

    private IEnumerator HideQuestion()
    {
        bool questionViewDisAppeared = false;
        UnityAction viewDisAppearedAction = () => questionViewDisAppeared = true;
        _gameScreen.QuestionDisAppeared += viewDisAppearedAction;
        _gameScreen.HideQuestion();

        while (!questionViewDisAppeared) yield return null;
        _gameScreen.QuestionDisAppeared -= viewDisAppearedAction;
    }

    private void ClearSelections()
    {
        _gameScreen.DeSelectAnswers();
    }
}