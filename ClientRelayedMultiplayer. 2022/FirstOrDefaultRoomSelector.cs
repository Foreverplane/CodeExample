using System;
using System.Linq;
using Nakama;
namespace ClientRelayedMultiplayer {
	public class FirstOrDefaultRoomSelector : RoomSelector {
		private IMatch _Match;
		private bool _IsMaster;

		public override async void RequestRoom(Nakama.Client client,ISession session, ISocket socket, Action<ConnectedMatchData> roomReceived) {
			
			var matches = await client.ListMatchesAsync(session, 0, 100, 100, false, null, null);
			var match = matches.Matches.FirstOrDefault();

			_IsMaster = match == null;
			if (_IsMaster) {
				_Match = await socket.CreateMatchAsync();
			}
			else {
				_Match = await socket.JoinMatchAsync(match.MatchId);
			}
			roomReceived.Invoke(new ConnectedMatchData() {
				IsMaster = _IsMaster,
				Match = _Match
			});
		}
	}

}
