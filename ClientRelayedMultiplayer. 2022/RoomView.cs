using System;
using UnityEngine;
using UnityEngine.UI;
namespace ClientRelayedMultiplayer {
	public class RoomView : MonoBehaviour {

		public RoomIDView RoomIDView;
		public PlayerCountView PlayerCountView;
		public ModeView ModeView;
		public StateView StateView;

		public Button Button;

		private void OnValidate() {
			RoomIDView = GetComponentInChildren<RoomIDView>();
			PlayerCountView = GetComponentInChildren<PlayerCountView>();
			ModeView = GetComponentInChildren<ModeView>();
			StateView = GetComponentInChildren<StateView>();
		}

	}

}
