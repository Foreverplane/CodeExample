using System;
using System.Collections.Generic;
using Assets.Scripts.Core.Services;
public class LifeTimeController : ChallengeController
{
    public LifeTimeController(ChallengePublisher challengePublisher, GameContextDataService contextDataService) : base(challengePublisher, contextDataService)
    {
    }
    private readonly Dictionary<IdData, int> _UserMultipliersDictionary = new Dictionary<IdData, int>();


    public override void Update()
    {
        var groups = ContextDataService.GetDataGroups<ChallangeDataGroup>();
        for (var i = 0; i < groups.Length; i++)
        {
            var g = groups[i];
            var lastAliveTime = g.deathData.LastAliveTime;
            if (lastAliveTime == 0)
            {
                continue;
            }
            var lifeTime = StaticNetworkTime.NetworkTimeCached() - new DateTime(lastAliveTime);
            //Debug.Log($"min {lifeTime.Minutes} totalMin {lifeTime.TotalMinutes}");
            int currentMul = 1;
            if (_UserMultipliersDictionary.TryGetValue(g.idData, out var mul)) {
                currentMul = mul;
            }
            else {
                _UserMultipliersDictionary[g.idData] = currentMul;
            }
            var targetMin = 5 * currentMul;
            var mod = (lifeTime.Minutes) == targetMin;
            if (targetMin>0&&mod)
            {
                _UserMultipliersDictionary[g.idData]++;
                var data = new ChallengeData {
                    Guid = g.profileData.guid,
                    UserName = g.profileData.nickName,
                    ChallengeName = $"<color=red>{targetMin}</color>min live left",
                    ChallengeScore = targetMin * 10
                };
                ChallengePublisher.Publish(data);
            }
        }
    }
}
