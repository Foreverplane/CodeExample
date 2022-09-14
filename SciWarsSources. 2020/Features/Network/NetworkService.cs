using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.Core.Services {
    public class NetworkService : MonoBehaviourPunCallbacks, ISignalListener<ConnectClientSignal>,  IInitializable {


        private string _roomName;
        [Zenject.Inject]
        private GameContextDataService _contextDataService;
        [Zenject.Inject]
        private Zenject.SignalBus _signalBus;

        [Zenject.Inject]
        private void Construct(string name) {
            Debug.Log($"Network service constructed!");
            _roomName = name;
        }

        void IInitializable.Initialize() {
            Debug.Log($"Initialize NetworkService");
            RegisterTypeToPhoton<Entity>(255);

            var types = DataMapper.GetDictionary();

            var method = this.GetType().GetMethod(nameof(RegisterTypeToPhoton));

            foreach (var t in types) {
                var generic = method.MakeGenericMethod(t.Key);
                generic.Invoke(this, new object[] { t.Value });
            }

        }

        public void RegisterTypeToPhoton<TType>(byte b) where TType : class {
            PhotonPeer.RegisterType(typeof(TType), b, Serialize<TType>(), Deserialize<TType>());
        }

        private static DeserializeMethod Deserialize<TType>() {
            return (e) => MessagePack.MessagePackSerializer.Deserialize<TType>(e);
        }

        private static SerializeMethod Serialize<TType>() where TType : class {
            return (e) => MessagePack.MessagePackSerializer.Serialize(e as TType);
        }



        public override void OnDisable() {
            base.OnDisable();
            Debug.Log($"<color=red>Try disconnect! When is connected: {PhotonNetwork.IsConnected}</color>");
            PhotonNetwork.LeaveLobby();
            PhotonNetwork.LeaveRoom();

            PhotonNetwork.Disconnect();
        }

        public override void OnConnected() {
            Debug.LogWarning("Onconnected");
            base.OnConnected();

        }

        public override void OnConnectedToMaster() {
            Debug.LogWarning("OnConnectedToMaster");
            base.OnConnectedToMaster();

            byte maxPlayers = 3;

            var roomProperties = new Hashtable();

            for (byte i = 0; i < maxPlayers; i++) {
                var entity = new Entity();
                var clientId = new ActorData(i);
                var rangeData = new RangeData(i * 1000);
    
                var lastAllocated = new NextAllocatedIdData(rangeData.range);
                var idData = new IdData(i);
                entity.Id = i;
                entity.Add(clientId, rangeData, lastAllocated, idData);
                var e = entity.ConvertToRoomProperties();
                foreach (var kvp in e) {
                    roomProperties.Add(kvp.Key, kvp.Value);
                }
            }

            Debug.Log($"Create <b>{roomProperties.Count}</b> roomProperties");

            var roomOptions = new RoomOptions() {
                MaxPlayers = maxPlayers,
                IsVisible = true,
                PublishUserId = true,
                CustomRoomProperties = roomProperties,
                CleanupCacheOnLeave = false,
                DeleteNullProperties = true,
                  //EmptyRoomTtl = 1000*30

            };
            var enter = new EnterRoomParams()
            {
                CreateIfNotExists = true,
                RoomOptions = roomOptions
            };
        
            PhotonNetwork.NetworkingClient.OpJoinRandomOrCreateRoom(null, enter);
          //  PhotonNetwork.JoinRandomOrCreateRoom(_roomName, roomOptions, TypedLobby.Default);


        }

        public override void OnCreatedRoom() {
            Debug.LogWarning("OnCreatedRoom");
            base.OnCreatedRoom();

        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            Debug.LogWarning("OnCreateRoomFailed");
            base.OnCreateRoomFailed(returnCode, message);

        }

        public override void OnCustomAuthenticationFailed(string debugMessage) {
            Debug.LogWarning("OnCustomAuthenticationFailed");
            base.OnCustomAuthenticationFailed(debugMessage);

        }

        public override void OnCustomAuthenticationResponse(Dictionary<string, object> data) {
            Debug.LogWarning("OnCustomAuthenticationResponse");
            base.OnCustomAuthenticationResponse(data);

        }

        public override void OnDisconnected(DisconnectCause cause) {
            Debug.LogWarning("OnDisconnected");
            base.OnDisconnected(cause);

        }

        public override void OnFriendListUpdate(List<FriendInfo> friendList) {
            Debug.LogWarning("OnFriendListUpdate");
            base.OnFriendListUpdate(friendList);

        }

        public override void OnJoinedLobby() {
            Debug.LogWarning("OnJoinedLobby");
            base.OnJoinedLobby();

        }

        public override void OnJoinedRoom() {
            Debug.LogWarning("OnJoinedRoom");
            base.OnJoinedRoom();
            Debug.Log($"Connect to room with properties: <b>{PhotonNetwork.CurrentRoom.CustomProperties.Count}</b>");
            //  PhotonNetwork.NetworkingClient.ChangeLocalID(GlobalData.IsClone ? 1 : 2);
            _signalBus.Fire(new RoomPropertiesUpdatedSignal(PhotonNetwork.CurrentRoom.CustomProperties));

        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            Debug.LogWarning("OnJoinRandomFailed");
            base.OnJoinRandomFailed(returnCode, message);

        }

        public override void OnJoinRoomFailed(short returnCode, string message) {
            Debug.LogWarning("OnJoinRoomFailed");
            base.OnJoinRoomFailed(returnCode, message);

        }

        public override void OnLeftLobby() {
            Debug.LogWarning("OnLeftLobby");
            base.OnLeftLobby();

        }

        public override void OnLeftRoom() {
            Debug.LogWarning("OnLeftRoom");
            base.OnLeftRoom();

        }

        public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) {
            Debug.LogWarning("OnLobbyStatisticsUpdate");
            base.OnLobbyStatisticsUpdate(lobbyStatistics);

        }

        public override void OnMasterClientSwitched(Player newMasterClient) {
            Debug.LogWarning("OnMasterClientSwitched");
            base.OnMasterClientSwitched(newMasterClient);

        }

        public override void OnPlayerEnteredRoom(Player newPlayer) {
            Debug.LogWarning("OnPlayerEnteredRoom");
            base.OnPlayerEnteredRoom(newPlayer);


        }

        public override void OnPlayerLeftRoom(Player otherPlayer) {
            Debug.LogWarning("OnPlayerLeftRoom");
            base.OnPlayerLeftRoom(otherPlayer);

        }

        public override void OnPlayerPropertiesUpdate(Player target, Hashtable changedProps) {
            Debug.LogWarning("OnPlayerPropertiesUpdate");
            base.OnPlayerPropertiesUpdate(target, changedProps);

        }

        public override void OnRegionListReceived(RegionHandler regionHandler) {
            Debug.LogWarning("OnRegionListReceived");
            base.OnRegionListReceived(regionHandler);

        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList) {
            Debug.LogWarning("OnRoomListUpdate");
            base.OnRoomListUpdate(roomList);

        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged) {
            //Debug.LogWarning($"<b>{propertiesThatChanged.Count}</b> properties updated!");
            var hastable = new Hashtable();
            foreach (var entry in propertiesThatChanged) {
                if (entry.Value == null)
                    continue;
                hastable.Add(entry.Key, entry.Value);
            }
            if (hastable.Count > 0)
                _signalBus.Fire(new RoomPropertiesUpdatedSignal(hastable));

        }

        public override void OnWebRpcResponse(OperationResponse response) {
            Debug.LogWarning("OnWebRpcResponse");
            base.OnWebRpcResponse(response);

        }



        void ISignalListener<ConnectClientSignal>.OnSignal(ConnectClientSignal signal) {
            Debug.Log("<b>Try to connect to photon!</b>");
            //return;
            if (!PhotonNetwork.IsConnected) {
              

                var postfix = GlobalData.IsClone ? "clone" : "";

                var prefKey = $"LocalPlayerUserId{postfix}";

                var cachedUserId = PlayerPrefs.GetString(prefKey);
                if (string.IsNullOrEmpty(cachedUserId)) {
                    cachedUserId = Guid.NewGuid().ToString();
                }

                PlayerPrefs.SetString(prefKey, cachedUserId);
                PhotonNetwork.AuthValues = new AuthenticationValues(cachedUserId);

               // PhotonNetwork.OfflineMode = true;
                PhotonNetwork.ConnectUsingSettings();

            }
            else
            {
                Debug.LogError($"Photon is connected!");
            }
        }


    }
}