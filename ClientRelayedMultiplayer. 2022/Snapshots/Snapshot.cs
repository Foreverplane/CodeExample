using System;
using System.Text;

namespace ClientRelayedMultiplayer {
	[Serializable]
	public struct Snapshot {
		public string CurrentStateHash;
		public byte[] State;

		public Snapshot(string state) : this() {
			State = Encoding.ASCII.GetBytes(state);
			CurrentStateHash = State.GetMd5Hex();
		}
		public Snapshot(string currentStateHash, byte[] state) {
			CurrentStateHash = currentStateHash;
			State = state;
		}
		public override string ToString() {
			var hash = State.GetMd5Hex();
			return $" SIZE: {State.Length} HASH {CurrentStateHash} MD5 {hash} - STATE: {Encoding.ASCII.GetString(State)}";
		}

	}
}
