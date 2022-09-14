using Assets.Scripts.Core.Services;
public class FirstBloodController : ChallengeController {
    private ChallengeData _ChallengeData;

    public FirstBloodController(ChallengePublisher challengePublisher, GameContextDataService contextDataService) : base(challengePublisher, contextDataService) {
    }

    public override void Update() {
        if (_ChallengeData != null)
            return;
        var groups = ContextDataService.GetDataGroups<ChallangeDataGroup>();
        for (var i = 0; i < groups.Length; i++) {
            var g = groups[i];
            if (g.killsData.Kills.Count > 0) {
                _ChallengeData = new ChallengeData() {
                    Guid = g.profileData.guid,
                    UserName = g.profileData.nickName,
                    ChallengeName = "FirstBlood",
                    ChallengeScore = 10
                };
                break;
            }
        }
        if (_ChallengeData == null)
            return;
        ChallengePublisher.Publish(_ChallengeData);
    }
}
