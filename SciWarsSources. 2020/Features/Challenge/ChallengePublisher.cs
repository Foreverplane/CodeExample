using System.Collections.Generic;
using Assets.Scripts.Core.Services;
using UnityEngine;
public class ChallengePublisher {
    private readonly ProfileChangesHandler _ProfileChangesHandler;
    private readonly ChallengeRootView _ChallengesRootView;
    private readonly ChallengeNameView _ChallengeNameView;
    private readonly ChallengeScoresView _ChallengeScoreView;
    private readonly UserNameView _UserNameView;

    private readonly Animator _Animator;
    private readonly Queue<ChallengeData> _Queue = new Queue<ChallengeData>();
    private readonly IdleBehaviour _IdleBehaviour;


    public ChallengePublisher(ChallengePanel challengesPanel, ProfileChangesHandler profileChangesHandler) {
        _ProfileChangesHandler = profileChangesHandler;
        _ChallengesRootView = challengesPanel.GetView<ChallengeRootView>();
        _ChallengeNameView = _ChallengesRootView.GetView<ChallengeNameView>();
        _ChallengeScoreView = _ChallengesRootView.GetView<ChallengeScoresView>();
        _UserNameView = _ChallengesRootView.GetView<UserNameView>();
        _Animator = challengesPanel.GetComponent<Animator>();
        _IdleBehaviour = _Animator.GetBehaviour<IdleBehaviour>();
    }

    public void Publish(ChallengeData data) {
        _Queue.Enqueue(data);
        _ProfileChangesHandler.HandleExp(data.Guid,data.ChallengeScore);
        _ProfileChangesHandler.HandlePoints(data.Guid,data.ChallengeScore);
    }

    public void Update() {
        if (!_IdleBehaviour.IsBehaviourActive)
            return;
        var count = _Queue.Count;
        if (count == 0)
            return;
        var data = _Queue.Dequeue();
        _ChallengeNameView.Value = data.ChallengeName;
        _ChallengeScoreView.Value = $"XP+{data.ChallengeScore.ToString()}";
        _UserNameView.Value = data.UserName;
        _Animator.SetTrigger("Post");
    }
}
