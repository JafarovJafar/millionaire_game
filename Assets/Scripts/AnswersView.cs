using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using EasyButtons;
using System.Collections.Generic;

public class AnswersView : MonoBehaviour
{
    public event UnityAction<int> AnswerClicked;

    public event UnityAction Appeared;
    public event UnityAction DisAppeared;

    [SerializeField] private CanvasGroup _canvasGroup;

    [SerializeField] private List<AnswerView> _views;

    [SerializeField] private float _appearDuration;
    [SerializeField] private Ease _appearEase;

    [SerializeField] private float _disAppearDuration;
    [SerializeField] private Ease _disAppearEase;

    [SerializeField] private Vector3[] _disAppearDirections;
    [SerializeField] private float _disAppearStrength;

    private bool _isInAnimation;

    private Vector3[] _onAwakePoses;

    private Dictionary<int, AnswerView> _playersSelections;

    public void Init()
    {
        foreach (var view in _views) view.Init();

        _onAwakePoses = new Vector3[4];
        for (int i = 0; i < _views.Count; i++)
        {
            _onAwakePoses[i] = _views[i].Position;
            _views[i].Clicked += AnswerView_Clicked;
        }

        _playersSelections = new Dictionary<int, AnswerView>();
    }

    private void AnswerView_Clicked(AnswerView answerView)
    {
        int index = _views.IndexOf(answerView);
        AnswerClicked?.Invoke(index);
    }

    public void SetAnswers(string[] answers)
    {
        for (int i = 0; i < _views.Count; i++)
        {
            _views[i].SetText(answers[i]);
        }
    }

    [Button]
    public void Appear()
    {
        DoAnimation
        (
            _onAwakePoses,
            _appearDuration,
            _appearEase,
            () =>
            {
                foreach (var view in _views) view.ShowText();
                Appeared?.Invoke();
            }
        );
    }

    [Button]
    public void DisAppear(bool immediate = false)
    {
        Vector3[] poses = new Vector3[4];
        for (int i = 0; i < poses.Length; i++)
        {
            poses[i] = _onAwakePoses[i] + _disAppearDirections[i] * _disAppearStrength;
        }

        float duration = immediate ? 0f : _disAppearDuration;

        DoAnimation
        (
            poses,
            duration,
            _disAppearEase,
            () =>
            {
                foreach (var view in _views) view.HideText();
                DisAppeared?.Invoke();
            }
        );
    }

    private void DoAnimation
    (
        Vector3[] goalPoses,
        float duration,
        Ease ease,
        UnityAction callback
    )
    {
        if (_isInAnimation) return;
        _isInAnimation = true;

        _canvasGroup.blocksRaycasts = false;

        foreach (var view in _views) view.HideText();

        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < goalPoses.Length; i++)
        {
            var pose = goalPoses[i];
            var viewTransform = _views[i].Transform;

            sequence.Insert(0, viewTransform.DOLocalMove(pose, duration));
        }

        sequence.SetEase(ease);
        sequence.OnComplete(() =>
        {
            _isInAnimation = false;
            _canvasGroup.blocksRaycasts = true;

            callback();
        });
    }

    public void Select(int answerIndex, int playerIndex)
    {
        if (_playersSelections.ContainsKey(playerIndex))
        {
            _playersSelections[playerIndex].DeSelect();
            _playersSelections[playerIndex] = _views[answerIndex];
        }
        else
        {
            _playersSelections.Add(playerIndex, _views[answerIndex]);
        }

        _views[answerIndex].Select(playerIndex);
    }

    public void DeSelect()
    {
        foreach (var view in _views)
        {
            view.DeSelect();
        }
    }
}