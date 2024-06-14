using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Бот, который случайным образом выбирает ответ
/// </summary>
public class RandomEnemy : BaseEnemy
{
    // тут можно вынести всякие настройки (пока что их нет)

    [SerializeField] private float _minPrepareDurationInSeconds = 0.1f;
    [SerializeField] private float _maxPrepareDurationInSeconds = 2;

    public override void Answer(QuestionData question, UnityAction<int> callback)
    {
        StartCoroutine(AnswerCoroutine(question, callback));
    }

    private IEnumerator AnswerCoroutine
    (
        QuestionData question,
        UnityAction<int> callback
    )
    {
        float interval = Random.Range(_minPrepareDurationInSeconds, _maxPrepareDurationInSeconds);

        yield return new WaitForSeconds(interval);

        int result = Random.Range(0, question.Answers.Length);
        callback?.Invoke(result);
    }
}