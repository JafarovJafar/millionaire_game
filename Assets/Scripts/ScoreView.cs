using UnityEngine;
using TMPro;

public class ScoreView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public void SetScore(int score)
    {
        _text.text = score.ToString();
    }
}