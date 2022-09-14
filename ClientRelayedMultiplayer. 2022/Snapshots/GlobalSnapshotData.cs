using System;
namespace ClientRelayedMultiplayer {
	[Serializable]
	public struct GlobalSnapshotData {
		public Snapshot Snapshot;
		public GlobalData GlobalData;
	}

}
