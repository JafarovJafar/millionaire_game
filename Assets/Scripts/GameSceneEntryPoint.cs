using UnityEngine;

public class GameSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private IntroScreen _introScreen;
    [SerializeField] private GameScreen _gameScreen;
    [SerializeField] private WinScreen _winscreen;
    [SerializeField] private BaseEnemy _enemy;
    [SerializeField] private Gameplay _gameplay;

    [SerializeField] private LevelData[] _levels;

    private int _currentLevelIndex;

    private void Start()
    {
        _gameScreen.Init();
        _winscreen.Init();

        _gameplay.Init
        (
            _introScreen,
            _gameScreen,
            _winscreen,
            _enemy
        );

        _currentLevelIndex = 0;

        _gameplay.Finished += Gameplay_Finished;
        StartLevel(_currentLevelIndex);
    }

    private void Gameplay_Finished()
    {
        _currentLevelIndex++;
        if (_currentLevelIndex >= _levels.Length) _currentLevelIndex = 0;

        StartLevel(_currentLevelIndex);
    }

    private void StartLevel(int levelIndex)
    {
        var level = _levels[levelIndex]; ;
        _gameplay.StartGameLoop(level);
    }
}