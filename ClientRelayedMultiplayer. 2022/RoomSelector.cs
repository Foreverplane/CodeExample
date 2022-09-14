using System;
using Nakama;
using UnityEngine;
namespace ClientRelayedMultiplayer {

	public struct ConnectedMatchData {
		public bool IsMaster;
		public IMatch Match;
	}
	public abstract class RoomSelector : MonoBehaviour {

		public  abstract void RequestRoom(Nakama.Client client, ISession session, ISocket socket, Action<ConnectedMatchData> roomReceived);
	}

}
