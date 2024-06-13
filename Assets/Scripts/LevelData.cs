using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    public IReadOnlyList<QuestionData> Questions => _questions;

    [SerializeField] private QuestionData[] _questions;
}