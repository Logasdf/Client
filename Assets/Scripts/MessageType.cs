using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf.Packet.Room;
using Google.Protobuf.State;

namespace Assets.Scripts
{
    public static class MessageType
    {
        public static readonly int ACCEPT = 0;
        public static readonly int REJECT = 1;
        public static readonly int REFRESH = 2;
        public static readonly int CREATE = 3;
        public static readonly int ENTER = 4;
        public static readonly int READY_EVENT = 5;
        public static readonly int START_GAME = 6;

        public static readonly int DATA = 100;
        public static readonly int ROOMLIST = 101;
        public static readonly int ROOM = 102;
        public static readonly int CLIENT = 103;
        public static readonly int PLAY_STATE = 104;
        public static readonly int TRANSFORM = 105;
        public static readonly int VECTOR_3 = 106;

        public static readonly Dictionary<Type, int> typeTable = new Dictionary<Type, int>()
        {
            { typeof(Data), DATA },
            { typeof(RoomList), ROOMLIST },
            { typeof(RoomInfo), ROOM },
            { typeof(Client), CLIENT },
            { typeof(PlayState), PLAY_STATE },
            { typeof(TransformProto), TRANSFORM },
            { typeof(Vector3Proto), VECTOR_3}
        };

        public static readonly Dictionary<int, Type> invTypeTable = new Dictionary<int, Type>()
        {
            { DATA, typeof(Data) },
            { ROOMLIST, typeof(RoomList) },
            { ROOM, typeof(RoomInfo) },
            { CLIENT, typeof(Client) },
            { PLAY_STATE, typeof(PlayState) },
            { TRANSFORM, typeof(TransformProto) },
            { VECTOR_3, typeof(Vector3Proto) }
        };
    }
}
