using UnityEngine;

public class GameSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private Gameplay _gameplay;
    [SerializeField] private LevelData _level;

    private void Start()
    {
        //_gameplay.Init();

        _gameplay.StartGameLoop(_level);
    }
}