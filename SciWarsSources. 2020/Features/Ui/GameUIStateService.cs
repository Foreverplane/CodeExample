using Assets.Scripts.Core.Services;
using Photon.Pun;
using UnityEngine;
using Zenject;

public class UIStateDataGroup:IEntityDataGroup {
    public OwnerData ownerData;
    public DriverData driverData;

    public DeathData deathData;
}

public class GameUIStateService : DataGroupService, IInitializable, 
    ISignalListener<DeathSignal>,
    ISignalListener<SpawnClickSignal>, ITickable
{
    [Zenject.Inject]
    private readonly GameUiService _gameUiService;

    public void Initialize()
    {
        _gameUiService.SetState<UiState.DeathUiState>();
    }

    public void OnSignal(DeathSignal signal)
    {
        //Debug.Log($"Get deathsignal for <b>{signal.id.Id}</b>");
        var dataGroup = base.ContextDataService.GetDataGroupById<UIStateDataGroup>(signal.deadReceiver.Id);

    }

    public void OnSignal(SpawnClickSignal signal)
    {     
        //_gameUiService.SetState<UiState.SpawnRequestedState>();
    }

    public void Tick()
    {
       var groups = base.ContextDataService.GetDataGroups<UIStateDataGroup>();
        for (int i = 0; i < groups.Length; i++)
        {
            var dataGroup = groups[i];
            if(dataGroup.ownerData.ownerId!=PhotonNetwork.LocalPlayer.UserId)
                continue;
            if (dataGroup.driverData.driverType == DriverData.DriverType.User &&
                dataGroup.ownerData.ownerId == PhotonNetwork.LocalPlayer.UserId) {
                if (dataGroup.deathData.IsDead)
                    _gameUiService.SetState<UiState.DeathUiState>();
                else
                    _gameUiService.SetState<UiState.AliveUiState>();
            }
        }
    }
}