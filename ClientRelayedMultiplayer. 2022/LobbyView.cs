using UnityEngine;
using UnityEngine.UI;
namespace ClientRelayedMultiplayer {
	public class LobbyView : MonoBehaviour {
		public RoomView CurrentRoomView;
		[Space(10)]
		public RoomView RoomViewPrefab;
		public PlayerEntryView PlayerEntryViewPrefab;

		public LoopVerticalScrollRect Rooms;
		public LoopVerticalScrollRect Players;
	}
}
