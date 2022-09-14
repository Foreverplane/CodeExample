using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Core.Services;
public class NKillsController : ChallengeController {
    
    private readonly Dictionary<IdData, int> _UserMultipliersDictionary = new Dictionary<IdData, int>();
    
    public override void Update() {
        var groups = ContextDataService.GetDataGroups<ChallangeDataGroup>();
        for (var i = 0; i < groups.Length; i++) {
            var g = groups[i];
            var kills = g.killsData.Kills.Values.Sum();
            int currentMul = 1;
            if (_UserMultipliersDictionary.TryGetValue(g.idData, out var mul)) {
                currentMul = mul;
            }
            else {
                _UserMultipliersDictionary[g.idData] = currentMul;
            }

            var targetKills = 10 * currentMul;
            var mod = kills == (targetKills);
            if (kills > 0 && mod) {
                _UserMultipliersDictionary[g.idData]++;
                ChallengePublisher.Publish(new ChallengeData {
                    Guid = g.profileData.guid,
                    UserName = g.profileData.nickName,
                    ChallengeName = $"<color=red>{targetKills}</color> Kills",
                    ChallengeScore = targetKills * 10
                });
            }
        }
    }
    public NKillsController(ChallengePublisher challengePublisher, GameContextDataService contextDataService) : base(challengePublisher, contextDataService) {
    }
}
