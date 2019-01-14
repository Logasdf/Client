using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Google.Protobuf;
using Google.Protobuf.Packet.Room;
using Google.Protobuf.State;

namespace Assets.Scripts.GameLogic.Context
{
    public class PlayerContext : ScriptableObject
    {
        public enum Animation
        {
            IDLE, DEATH, MOVE
        }

        private Client client;
        public Client Client { get { return client; } }
        private PlayState state;
        public PlayState State { get { return state; } }
        private GameObject player;
        public GameObject Player { get { return player; } }
        
        //int playerType;
        public void Init(int roomId, Client client, GameObject player)
        {
            this.client = client;
            state = new PlayState();
            state.AnimState = (int)Animation.IDLE;
            state.KillCount = state.DeathCount = 0;
            state.Health = 100;
            state.ClntName = client.Name;
            state.RoomId = roomId;
            this.player = player;
            state.Transform = new TransformProto
            {
                Position = new Vector3Proto(),
                Rotation = new Vector3Proto(),
                Scale = new Vector3Proto()
            };
        }

        public void CopyToTransFormProto()
        {
            SetVector3(state.Transform.Position, player.transform.localPosition);
            SetVector3(state.Transform.Rotation, player.transform.localRotation.eulerAngles);
            SetVector3(state.Transform.Scale, player.transform.localScale);
        }

        public void UpdateTransform(TransformProto tfp)
        {
            player.transform.localPosition = new Vector3(tfp.Position.X, tfp.Position.Y, tfp.Position.Z);
            player.transform.localRotation = Quaternion.Euler(new Vector3(tfp.Rotation.X, tfp.Rotation.Y, tfp.Rotation.Z));
        }

        private void SetVector3(Vector3 v3, Vector3Proto v3Proto)
        {
            v3.x = v3Proto.X;
            v3.y = v3Proto.Y;
            v3.z = v3Proto.Z;
        }

        private void SetVector3(Vector3Proto v3Proto, Vector3 v3)
        {
            v3Proto.X = v3.x;
            v3Proto.Y = v3.y;
            v3Proto.Z = v3.z;
        }

        private void SetVector3(Vector3Proto toV3Proto, Vector3Proto fromV3Proto)
        {
            toV3Proto.X = fromV3Proto.X;
            toV3Proto.Y = fromV3Proto.Y;
            toV3Proto.Z = fromV3Proto.Z;
        }

        private void OnEnable()
        {

        }
    }
}
