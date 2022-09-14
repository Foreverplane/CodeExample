using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
namespace ClientRelayedMultiplayer {
	public class ClientsSnapshotsContainer : MonoBehaviour {
		public readonly ConcurrentDictionary<string, ClientSnapshotData> ClientsSnapshots = new ConcurrentDictionary<string, ClientSnapshotData>();
		[SerializeField]
		private List<ClientSnapshotData> _SnapshotDataPreview = new List<ClientSnapshotData>();
		private void Update() {
			_SnapshotDataPreview.Clear();
			_SnapshotDataPreview.AddRange(ClientsSnapshots.Values);
		}
	}
}
