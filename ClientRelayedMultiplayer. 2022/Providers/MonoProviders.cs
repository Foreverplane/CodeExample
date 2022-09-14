using System;
using UnityEngine;
using UnityEngine.Serialization;
namespace ClientRelayedMultiplayer {
	[Serializable]
	public class MonoProviders {
		[SerializeField]
		public ClientsSnapshotsContainer ClientsSnapshotsContainer;
		[SerializeField]
		public ClientDataProvider ClientDataProvider;
		[SerializeField]
		public GlobalDataProvider GlobalDataProvider;
		[SerializeField]
		public IsMasterProvider IsMasterProvider;
	}
}
