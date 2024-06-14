using UnityEngine;

public class GameScreenHeader : MonoBehaviour
{
    [SerializeField] private ScoreView _heroScore;
    [SerializeField] private ScoreView _enemyScore;

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

    public void SetHeroScore(int score) => _heroScore.SetScore(score);
    public void SetEnemyScore(int score) => _enemyScore.SetScore(score);
}