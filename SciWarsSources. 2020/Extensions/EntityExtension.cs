using ExitGames.Client.Photon;

namespace Assets.Scripts.Core.Services
{
    public static class EntityExtension {

        public static Hashtable ConvertToRoomProperties(this Entity entity) {
            var roomProperties = new Hashtable();
            entity.datas.ForEach(data => {
                var dataByteCode = data.GetByteCode();
                var kString = new KeyData(entity.Id, dataByteCode);
                var serialized = MessagePack.MessagePackSerializer.SerializeToJson(kString);
                dynamic trueType = data;
                roomProperties[serialized] = trueType;
            });
            return roomProperties;
        }

        public static Hashtable ConvertToDestroyableRoomProperties(this Entity entity) {
            var roomProperties = new Hashtable();
            entity.datas.ForEach(data => {
                var dataByteCode = data.GetByteCode();
                var kString = new KeyData(entity.Id, dataByteCode);
                var serialized = MessagePack.MessagePackSerializer.SerializeToJson(kString);
                roomProperties[serialized] = null;
            });
            return roomProperties;
        }

    }
}