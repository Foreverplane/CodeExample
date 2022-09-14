using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using RandomNameGen;
using UniRx;
using UnityEngine;
using Zenject;

public struct OnLoginSuccessSignal {

}


public class PlayFabLoginService : IInitializable {

    [Inject]
    private readonly SignalBus _SignalBus;
    [Inject]
    private readonly ClientSessionData _ClientSessionData;

    public void Initialize() {
        Debug.Log("BackendService initialized");
        if (string.IsNullOrEmpty(PlayFabSettings.TitleId)) {
            Debug.LogError($"There is no Titleid");
            return;
        }
        var request = new LoginWithAndroidDeviceIDRequest {
            AndroidDeviceId = DeviceId,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnSuccess, OnFailure);
        _SignalBus.Subscribe<UserChangeNickNameSignal>(OnUserChangeNickName);
    }


    public void OnGooglePlayServicesInitialized() {
        var linkGoogleAccount = new LinkGoogleAccountRequest() {
            ForceLink = true
        };
        PlayFabClientAPI.LinkGoogleAccount(linkGoogleAccount, OnLinkSuccess, OnLinkFailure);
    }
    private void OnLinkFailure(PlayFabError obj) {
        Debug.LogError($"Link goolge account failed because of: {obj.GenerateErrorReport()}");
    }
    private void OnLinkSuccess(LinkGoogleAccountResult obj) {
        Debug.LogWarning("Google Account successfully linked!");
    }

    private string DeviceId => SystemInfo.deviceUniqueIdentifier;
    private void OnFailure(PlayFabError obj) {
        Debug.LogError($"On playfab event error because of: {obj.GenerateErrorReport()}");
    }
    private void OnSuccess(LoginResult obj) {
        Debug.LogWarning("On anonymous login successful");
        _ClientSessionData.IsLogged.SetValueAndForceNotify(true);
        _SignalBus.Fire(new OnLoginSuccessSignal());
        _ClientSessionData.StartBattleWithShip.Subscribe(_ => {
            PlayFabClientAPI.WritePlayerEvent(new WriteClientPlayerEventRequest() {
                Body = new Dictionary<string, object>() {
                    {
                        "Ship", _
                    }
                },
                EventName = "to_battle"
            }, OnEventWriteSuccess, OnFailure);
        });
        PlayFabClientAPI.GetPlayerProfile( new GetPlayerProfileRequest() {
                PlayFabId = obj.PlayFabId,
                ProfileConstraints = new PlayerProfileViewConstraints() {
                    ShowDisplayName = true
                }
            },
            result => Debug.Log($"The player's DisplayName profile data is: { result.PlayerProfile.DisplayName}"),
            error => Debug.LogError(error.GenerateErrorReport()));
        // var addUserName = new AddUsernamePasswordRequest() {
        //     Username = RndName.RandomNickName
        // };
        // PlayFabClientAPI.AddUsernamePassword(addUserName,OnAddUserNameSuccess,(e)=>Debug.LogError($"Got: {e.GenerateErrorReport()}"));
    }
    private void OnEventWriteSuccess(WriteEventResponse obj) {
        Debug.Log("Write success");
    }
    private void OnAddUserNameSuccess(AddUsernamePasswordResult obj) {
        _ClientSessionData.UserName.SetValueAndForceNotify(obj.Username);
    }
    private void OnUserChangeNickName(UserChangeNickNameSignal obj) {
        _ClientSessionData.UserName.SetValueAndForceNotify(obj.Name);
        // var addUserName = new AddUsernamePasswordRequest() {
        //     Username = obj.Name
        // };
        // PlayFabClientAPI.AddUsernamePassword(addUserName,OnAddUserNameSuccess,(e)=>Debug.LogError($"Got: {e.GenerateErrorReport()}"));
    }
}