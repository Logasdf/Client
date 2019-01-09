using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Google.Protobuf.Packet.Room;

namespace Assets.Scripts
{
    public static class MessageType
    {
        public static readonly int ACCEPT = 0;
        public static readonly int REJECT = 1;
        public static readonly int REFRESH = 2;
        public static readonly int CREATE = 3;
        public static readonly int ENTER = 4;

        public static readonly int DATA = 100;
        public static readonly int ROOMLIST = 101;
        public static readonly int ROOM = 102;
        public static readonly int CLIENT = 103;

        public static readonly Dictionary<Type, int> typeTable = new Dictionary<Type, int>()
        {
            { typeof(Data), DATA },
            { typeof(RoomList), ROOMLIST },
            { typeof(RoomInfo), ROOM },
            { typeof(Client), CLIENT }
        };

        public static readonly Dictionary<int, Type> invTypeTable = new Dictionary<int, Type>()
        {
            { DATA, typeof(Data) },
            { ROOMLIST, typeof(RoomList) },
            { ROOM, typeof(RoomInfo) },
            { CLIENT, typeof(Client) }
        };
    }
}
