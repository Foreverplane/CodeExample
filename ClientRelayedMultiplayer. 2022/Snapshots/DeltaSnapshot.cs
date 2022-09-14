namespace ClientRelayedMultiplayer {
	public struct DeltaSnapshot {
		public Snapshot Snapshot;
		public string SourceSnapshotHash;

		public DeltaSnapshot(Snapshot snapshot, string sourceSnapshotHash) {
			Snapshot = snapshot;
			SourceSnapshotHash = sourceSnapshotHash;
		}
	}
}
