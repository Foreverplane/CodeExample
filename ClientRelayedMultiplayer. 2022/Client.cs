using System;
using System.Collections;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Nakama;
using UnityEngine;

namespace ClientRelayedMultiplayer {

	public class Client : MonoBehaviour {
		private Nakama.Client _Client;
		private ISocket _Socket;
		private ISession _Session;
		[SerializeField]
		private string _CurrentMatchId;

		private CancellationTokenSource _CancellationTokenSource;
		[SerializeField]
		private int _TickRate = 10;

		[SerializeField]
		private int _GlobalStateTickRate = 1;

		private ISerializer _Serializer;

		private IMatch _Match;

		[SerializeField][UnityEngine.UI.Extensions.ReadOnly]
		private IsMasterProvider _IsMasterProvider;
		[SerializeField][UnityEngine.UI.Extensions.ReadOnly]
		private ClientsSnapshotsContainer _ClientsSnapshotsContainer;
		[SerializeField][UnityEngine.UI.Extensions.ReadOnly]
		private ClientDataProvider _ClientDataProvider;
		[SerializeField][UnityEngine.UI.Extensions.ReadOnly]
		private GlobalDataProvider _GlobalDataProvider;
		[SerializeField][UnityEngine.UI.Extensions.ReadOnly]
		private GlobalSnapshotData _GlobalSnapshotData;

		[SerializeField][UnityEngine.UI.Extensions.ReadOnly]
		private ClientMonoProvider _ClientMonoProvider;
		[SerializeField][UnityEngine.UI.Extensions.ReadOnly]
		private RoomSelector _RoomSelector;
		
		private void OnValidate() {
			_IsMasterProvider = GetComponentInChildren<IsMasterProvider>();
			_ClientsSnapshotsContainer = GetComponentInChildren<ClientsSnapshotsContainer>();
			_ClientDataProvider = GetComponentInChildren<ClientDataProvider>();
			_GlobalDataProvider = GetComponentInChildren<GlobalDataProvider>();
			_ClientMonoProvider = GetComponentInChildren<ClientMonoProvider>();
			_RoomSelector = GetComponentInChildren<RoomSelector>();
		}

		[EasyButtons.Button]
		public async void Create(ClientData clientData, GlobalData globalData) {
			var connectionData = new NakamaConnectionData(Guid.NewGuid().ToString());
			_Client = new Nakama.Client(connectionData.Scheme, connectionData.Host, connectionData.Port, connectionData.ServerKey);
			_Socket = _Client.NewSocket(false);
			_Socket.ReceivedMatchState += OnMatchStateReceived;
			var authToken = PlayerPrefs.GetString(connectionData.PrefKeyName + connectionData.Prefix);
			if (string.IsNullOrEmpty(authToken) || (_Session = Session.Restore(authToken)).IsExpired) {
				// Debug.Log("Session has expired. Must reauthenticate!");
				var deviceId = SystemInfo.deviceUniqueIdentifier + connectionData.Prefix;
				_Session = await _Client.AuthenticateDeviceAsync(deviceId + connectionData.Prefix);
			}
			await _Socket.ConnectAsync(_Session);

			_RoomSelector.RequestRoom(_Client, _Session,_Socket,(data) => {
				_IsMasterProvider.IsMaster = data.IsMaster;
				_Match = data.Match;
				_CurrentMatchId = _Match.Id;
				_ClientDataProvider.Data = clientData;
				_GlobalDataProvider.Data = globalData;
				_CancellationTokenSource = new CancellationTokenSource();
				_Serializer = new UnityJsonSerializer();
				StartClientDataSynchronizationProcess();
				StartGlobalStateSynchronizationProcess();
			});

		}

		private void StartGlobalStateSynchronizationProcess() {
			Task.Run(async () => {
				try {
					while (!_CancellationTokenSource.IsCancellationRequested) {
						await Task.Delay(1000 / _GlobalStateTickRate);
						if (_CancellationTokenSource.IsCancellationRequested)
							return;
						await GlobalStateSynchronizationTick();
					}
				}
				catch (Exception e) {
					Debug.LogError(e);
				}
			});
		}

		private void StartClientDataSynchronizationProcess() {
			Task.Run(async () => {
				try {
					while (!_CancellationTokenSource.IsCancellationRequested) {
						await Task.Delay(1000 / _TickRate);
						if (_CancellationTokenSource.IsCancellationRequested)
							return;
						await ClientDataSynchronizationTick();
					}
				}
				catch (Exception e) {
					Debug.LogError(e);
				}
			});
		}

		private async Task GlobalStateSynchronizationTick() {
			if (!_IsMasterProvider.IsMaster)
				return;
			var currentData = _GlobalDataProvider.Data;
			var currentSnapshot = new Snapshot(_Serializer.Serialize(currentData));
			if (!string.IsNullOrEmpty(_GlobalSnapshotData.Snapshot.CurrentStateHash)) {
				if (_GlobalSnapshotData.Snapshot.CurrentStateHash == currentSnapshot.CurrentStateHash) {
					return;
				}
				var sourceSnapshot = _GlobalSnapshotData;
				var delta = Fossil.Delta.Create(sourceSnapshot.Snapshot.State, currentSnapshot.State);
				var snapshot = new Snapshot(currentSnapshot.CurrentStateHash, delta);
				var deltaSnapshot = new DeltaSnapshot(snapshot, sourceSnapshot.Snapshot.CurrentStateHash);
				// Debug.Log($"Send delta {delta.Length}");
				await _Socket.SendMatchStateAsync(_CurrentMatchId, OpCodes.GlobalSnapshotDelta, _Serializer.Serialize(deltaSnapshot));
			}
			_GlobalSnapshotData = new GlobalSnapshotData() {
				Snapshot = currentSnapshot,
				GlobalData = currentData
			};
		}
		private async Task ClientDataSynchronizationTick() {
			var currentData = _ClientDataProvider.Data;
			var currentSnapshot = new Snapshot(_Serializer.Serialize(currentData));
			var userId = _Match.Self.UserId;
			if (_ClientsSnapshotsContainer.ClientsSnapshots.TryGetValue(userId, out var sourceSnapshot)) {
				if (sourceSnapshot.Snapshot.CurrentStateHash == currentSnapshot.CurrentStateHash) {
					// Debug.LogError("Nothing changed");
					return;
				}
				else {
					var delta = Fossil.Delta.Create(sourceSnapshot.Snapshot.State, currentSnapshot.State);
					var snapshot = new Snapshot(currentSnapshot.CurrentStateHash, delta);
					var deltaSnapshot = new DeltaSnapshot(snapshot, sourceSnapshot.Snapshot.CurrentStateHash);
					// Debug.Log($"Send delta {delta.Length}");
					await _Socket.SendMatchStateAsync(_CurrentMatchId, OpCodes.ClientSnapshotDelta, _Serializer.Serialize(deltaSnapshot));
				}
			}
			_ClientsSnapshotsContainer.ClientsSnapshots[userId] = new ClientSnapshotData() {
				Snapshot = currentSnapshot,
				ClientData = currentData
			};
		}

		private void OnMatchStateReceived(IMatchState obj) {
			// Debug.Log($"Received match state: {obj}");
			if(obj.State==null)
				return;
			var userId = obj.UserPresence.UserId;
			switch (obj.OpCode) {
				case OpCodes.ClientSnapshotDelta:
					bool isClientSnapshotMismatch = false;
					var receivedSnapshot = _Serializer.Deserialize<DeltaSnapshot>(obj.State);
					if (_ClientsSnapshotsContainer.ClientsSnapshots.TryGetValue(userId, out var clientSnapshot)) {
						if (clientSnapshot.Snapshot.CurrentStateHash == receivedSnapshot.SourceSnapshotHash) {
							SaveClientSnapshotData(clientSnapshot, receivedSnapshot, userId);
							// Debug.Log($"Received snapshot restored");
						}
						else {
							Debug.LogWarning($"Receive mismatched snapshot");
							isClientSnapshotMismatch = true;
						}
					}
					else {
						isClientSnapshotMismatch = true;
					}
					if (isClientSnapshotMismatch) {
						Debug.LogWarning("Mismatched client. Need to request snapshot");
						_Socket.SendMatchStateAsync(_CurrentMatchId, OpCodes.RequestClientCurrentSnapshot, "RequestFullSnapshot", new IUserPresence[] {
							obj.UserPresence
						});
					}
					break;
				case OpCodes.RequestClientCurrentSnapshot:
					// Debug.Log("RequestFullSnapshot received");
					if (_ClientsSnapshotsContainer.ClientsSnapshots.TryGetValue(_Match.Self.UserId, out var data)) {
						_Socket.SendMatchStateAsync(_CurrentMatchId, OpCodes.ResponseClientCurrentSnapshot, data.Snapshot.State, new IUserPresence[] {
							obj.UserPresence
						});
					}
					else {
						Debug.LogError("There is no current snapshot");
					}
					break;
				case OpCodes.ResponseClientCurrentSnapshot:
					// Debug.Log("ResponseFullSnapshot received");
					var snapshot = new Snapshot(obj.State.GetMd5Hex(), obj.State);
					var unityClientData = _Serializer.Deserialize(_ClientDataProvider.Data.GetType(),snapshot.State);
					var clientData = new ClientSnapshotData() {
						Snapshot = snapshot,
						ClientData = (ClientData)unityClientData,
					};
					_ClientsSnapshotsContainer.ClientsSnapshots[userId] = clientData;
					break;
				case OpCodes.GlobalSnapshotDelta:
					var receivedGlobalSnapshot = _Serializer.Deserialize<DeltaSnapshot>(obj.State);
					if (_GlobalSnapshotData.Snapshot.CurrentStateHash == receivedGlobalSnapshot.SourceSnapshotHash) {
						var restoredSnapshot = Fossil.Delta.Apply(_GlobalSnapshotData.Snapshot.State, receivedGlobalSnapshot.Snapshot.State);
						var globalDataDeserialized = _Serializer.Deserialize(_GlobalDataProvider.Data.GetType(),restoredSnapshot);
						_GlobalSnapshotData = new GlobalSnapshotData() {
							Snapshot = new Snapshot(receivedGlobalSnapshot.Snapshot.CurrentStateHash, restoredSnapshot),
							GlobalData = (GlobalData)globalDataDeserialized
						};
						_GlobalDataProvider.Data = _GlobalSnapshotData.GlobalData;
					}
					else {
						Debug.LogWarning($"Receive mismatched snapshot");
						_Socket.SendMatchStateAsync(_CurrentMatchId, OpCodes.RequestGlobalSnapshot, "RequestGlobalSnapshot", new IUserPresence[] {
							obj.UserPresence
						});
					}
					break;
				case OpCodes.RequestGlobalSnapshot:
					// Debug.Log("RequestFullSnapshot received");
					_Socket.SendMatchStateAsync(_CurrentMatchId, OpCodes.ResponseGlobalSnapshot, _GlobalSnapshotData.Snapshot.State, new IUserPresence[] {
						obj.UserPresence
					});
					Debug.LogError("There is no current snapshot");
					break;
				case OpCodes.ResponseGlobalSnapshot:
					var globalSnapshot = new Snapshot(obj.State.GetMd5Hex(), obj.State);
					var globalData = _Serializer.Deserialize(_GlobalDataProvider.Data.GetType(),globalSnapshot.State);
					var globalSnapshotData = new GlobalSnapshotData() {
						Snapshot = globalSnapshot,
						GlobalData = (GlobalData)globalData,
					};
					_GlobalSnapshotData = globalSnapshotData;
					_GlobalDataProvider.Data = _GlobalSnapshotData.GlobalData;
					break;
			}
		}
		private void SaveClientSnapshotData(ClientSnapshotData clientSnapshot, DeltaSnapshot receivedSnapshot, string userId) {
			var restoredSnapshot = Fossil.Delta.Apply(clientSnapshot.Snapshot.State, receivedSnapshot.Snapshot.State);
			var clientData = _Serializer.Deserialize(_ClientDataProvider.Data.GetType(),restoredSnapshot);
			_ClientsSnapshotsContainer.ClientsSnapshots[userId] = new ClientSnapshotData() {
				Snapshot = new Snapshot(receivedSnapshot.Snapshot.CurrentStateHash, restoredSnapshot),
				ClientData = (ClientData)clientData
			};
		}

		private void OnDestroy() {
			_CancellationTokenSource?.Cancel();
		}
		public void InstallBuilders(DataControllerBuilder client, DataControllerBuilder globalBuilder) {
			_ClientMonoProvider.InstallBuilders(client, globalBuilder);
		}
	}

}
