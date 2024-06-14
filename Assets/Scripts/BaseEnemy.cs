using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Базовый класс для всех типов врагов (бот, реальный игрок по сети и т.д.)
/// </summary>
public abstract class BaseEnemy : MonoBehaviour
{
    public abstract void Answer(QuestionData question, UnityAction<int> callback);
}