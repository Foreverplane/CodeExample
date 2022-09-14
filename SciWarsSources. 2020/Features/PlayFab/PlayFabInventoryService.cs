using Assets.Scripts.Core.Services;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;
using Zenject;
public class PlayFabInventoryService : IInitializable {

    [Inject]
    private readonly ClientSessionData _ClientSessionData;
    [Inject]
    private readonly SignalBus _SignalBus;

    public void Initialize() {
        _ClientSessionData.IsLogged.Where(_ => _).Subscribe(_ => {
            RequestInventory();
        });
        _SignalBus.Subscribe<ChangePointsSignal>(OnPointsChanged);
    }
    public void OnPointsChanged(ChangePointsSignal obj) {
        Debug.Log($"Try add {obj.Amount} points");
        var currentVal = _ClientSessionData.XP.Value;
        currentVal += obj.Amount;
        AddUserVirtualCurrencyRequest addVCrequest = new AddUserVirtualCurrencyRequest {
            Amount = obj.Amount,
            VirtualCurrency = "XP"
        };
        PlayFabClientAPI.AddUserVirtualCurrency(addVCrequest, result => {
            _ClientSessionData.XP.SetValueAndForceNotify(currentVal);
            Debug.Log($"Success currency XP now is {currentVal}");
        }, error => {
            Debug.LogError($"Error. Cant add {obj.Amount} currency because of: {error.GenerateErrorReport()}");
        });
    }
    private void RequestInventory() {
        var request = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(request, OnSuccess, OnFailure);
    }
    private void OnFailure(PlayFabError obj) {
        Debug.LogError($"Get user inventory failed: {obj.GenerateErrorReport()}");
    }
    private void OnSuccess(GetUserInventoryResult obj) {
        Debug.LogWarning($"Get user inventory success");
        var currencyQuantity = obj.VirtualCurrency["CR"];
        Debug.Log($"Got virtual currency CR quantity: {currencyQuantity}");
        _ClientSessionData.XP.SetValueAndForceNotify(obj.VirtualCurrency["XP"]);
        _ClientSessionData.Credits.SetValueAndForceNotify(currencyQuantity);
        foreach (var itemInstance in obj.Inventory) {
            Debug.Log($"Got: {itemInstance.ItemId}");
            _ClientSessionData.ServerInventory.Add(itemInstance);
        }
        _ClientSessionData.IsInventoryReceived.SetValueAndForceNotify(true);
    }
}