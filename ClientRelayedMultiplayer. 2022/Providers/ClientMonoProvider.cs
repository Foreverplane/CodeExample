using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI.Extensions;
namespace ClientRelayedMultiplayer {

	public class ClientMonoProvider : MonoBehaviour {

		[SerializeField]
		private MonoProviders _Providers;

		
		private readonly Dictionary<string,IDataController> _UnityClientDataControllers = new Dictionary<string, IDataController>();

		private IDataController _GlobalDataController;
		
		[SerializeField][UnityEngine.UI.Extensions.ReadOnly]
		private DataControllerBuilder _ClientDataControllerBuilder;
		[SerializeField][UnityEngine.UI.Extensions.ReadOnly]
		private DataControllerBuilder _GlobalDataControllerBuilder;

		public void InstallBuilders(DataControllerBuilder clientDataControllerBuilder, DataControllerBuilder globalDataControllerBuilder) {
			_ClientDataControllerBuilder = clientDataControllerBuilder;
			_GlobalDataControllerBuilder = globalDataControllerBuilder;
			_GlobalDataController =_GlobalDataControllerBuilder.Build(this._Providers);
		}

		private void OnValidate() {
			_Providers.ClientsSnapshotsContainer = GetComponentInChildren<ClientsSnapshotsContainer>();
			_Providers.ClientDataProvider = GetComponentInChildren<ClientDataProvider>();
			_Providers.IsMasterProvider = GetComponentInChildren<IsMasterProvider>();
			_Providers.GlobalDataProvider = GetComponentInChildren<GlobalDataProvider>();
			
		}

		void Update() {
			UpdateClients();
			var updateData = _Providers.GlobalDataProvider.Data;
			_GlobalDataController.Update(_Providers.IsMasterProvider.IsMaster?LocalityFlags.Master:LocalityFlags.Slave,updateData);
		}
		private void UpdateClients() {
			foreach (var clientSnapshotData in _Providers.ClientsSnapshotsContainer.ClientsSnapshots) {
				var isMine = clientSnapshotData.Value.ClientData.Id == _Providers.ClientDataProvider.Data.Id;
				if (!_UnityClientDataControllers.TryGetValue(clientSnapshotData.Value.ClientData.Id, out IDataController controller)) {
					controller = _ClientDataControllerBuilder.Build(this._Providers);
					_UnityClientDataControllers[clientSnapshotData.Value.ClientData.Id] = controller;
				}
				var updateData = isMine ? _Providers.ClientDataProvider.Data : clientSnapshotData.Value.ClientData;
				controller.Update(isMine?LocalityFlags.Local:LocalityFlags.Remote, updateData);
			}
		}

	}

}
