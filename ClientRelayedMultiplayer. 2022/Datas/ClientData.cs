using System;
using UnityEngine;
namespace ClientRelayedMultiplayer {
	[Serializable]
	public class ClientData : IHeartbeatable  {

		[field: SerializeField]
		public string Id { get; set; }

		[field: SerializeField]
		public bool IsMaster { get; set; }

		[field: SerializeField]
		public long Heartbeat { get; set; }
		
	}

}
