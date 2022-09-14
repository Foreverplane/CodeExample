using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;
using Zenject;
public class PlayFabPlayerStatisticsService : IInitializable {
    [Inject]
    private readonly ClientSessionData _ClientSessionData;
    public void Initialize() {
        _ClientSessionData.IsInventoryReceived.Where(_ => _).Subscribe(_ => {
            RequestPlayerStats();
        });
    }
    private void RequestPlayerStats() {
        Debug.Log("Request playerstats");
        GetStatistics();
    }
    void GetStatistics() {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    void OnGetStatistics(GetPlayerStatisticsResult result) {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
        _ClientSessionData.Statistics.SetValueAndForceNotify(result.Statistics);
    }
}
