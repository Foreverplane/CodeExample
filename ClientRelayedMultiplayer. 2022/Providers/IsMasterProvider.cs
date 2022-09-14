using System;
using UnityEngine;
namespace ClientRelayedMultiplayer {
	public class IsMasterProvider : MonoBehaviour {
		public bool IsMaster;

		[SerializeField]
		private ClientDataProvider _ClientDataProvider;

		private void OnValidate() {
			_ClientDataProvider = GetComponentInChildren<ClientDataProvider>();
		}
		private void Update() {
			var data = _ClientDataProvider.Data;
			data.IsMaster = IsMaster;
			_ClientDataProvider.Data = data;
		}
	}
}
