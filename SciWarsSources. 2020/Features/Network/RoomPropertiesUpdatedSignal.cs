using ExitGames.Client.Photon;

namespace Assets.Scripts.Core.Services
{
    public class RoomPropertiesUpdatedSignal : ISignal {
        public readonly Hashtable roomProperties;

        public RoomPropertiesUpdatedSignal(Hashtable roomProperties) {
            this.roomProperties = roomProperties;
        }
    }
}