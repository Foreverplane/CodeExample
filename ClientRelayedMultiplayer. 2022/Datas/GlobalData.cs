using System;
using UnityEngine;
namespace ClientRelayedMultiplayer {
	[Serializable]
	public class GlobalData : IHeartbeatable {

		[field: SerializeField]
		public  long Heartbeat { get; set; }
	}
}
