using UnityEngine;

public class GameSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private Gameplay _gameplay;
    [SerializeField] private LevelData[] _levels;

    private int _currentLevelIndex;

    private void Start()
    {
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