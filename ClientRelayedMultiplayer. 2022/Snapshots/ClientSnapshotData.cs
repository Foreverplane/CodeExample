using System;
namespace ClientRelayedMultiplayer {
	
	
	[Serializable]
	public struct ClientSnapshotData {
		public Snapshot Snapshot;
		public ClientData ClientData;
	}

}
