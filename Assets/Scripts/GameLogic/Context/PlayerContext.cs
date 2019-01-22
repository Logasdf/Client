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
        public static readonly int IDLE = 0;
        public static readonly int DEATH = 1;
        public static readonly int MOVE = 2;

        private Client client;
        public Client Client { get { return client; } }
        private WorldState _worldState;
        public WorldState WorldState { get { return _worldState; } }
        private GameObject player;
        public GameObject Player { get { return player; } }
        private Transform respawnPos;
        public Transform RespawnPos { get { return respawnPos; } }
        
        //int playerType;
        public void Init(int roomId, Client client, GameObject player, Transform respawnPos)
        {
            this.client = client;
            this.player = player;
            this.respawnPos = respawnPos;

            _worldState = new WorldState
            {
                RoomId = roomId,
                ClntName = client.Name,
                Transform = new TransformProto
                {
                    Position = new Vector3Proto(),
                    Rotation = new Vector3Proto(),
                    Scale = new Vector3Proto()
                },
                Fired = false,
                Health = 100,
                Hit = false,
                HitState = new HitState
                {
                    From = "",
                    To = "",
                    Damage = 0
                },
                KillPoint = 0,
                DeathPoint = 0,
                AnimState = IDLE
            };
        }

        public void CopyToTransformProto()
        {
            SetVector3(_worldState.Transform.Position, player.transform.localPosition);
            SetVector3(_worldState.Transform.Rotation, player.transform.localRotation.eulerAngles);
            SetVector3(_worldState.Transform.Scale, player.transform.localScale);
        }

        public void UpdateHitState(HitState hs)
        {
            _worldState.HitState.From = hs.From;
            _worldState.HitState.To = hs.To;
            _worldState.HitState.Damage = hs.Damage;
        }

        public void UpdateTransform(TransformProto tfp)
        {
            player.transform.localPosition = new Vector3(tfp.Position.X, tfp.Position.Y, tfp.Position.Z);
            player.transform.localRotation = Quaternion.Euler(new Vector3(tfp.Rotation.X, tfp.Rotation.Y, tfp.Rotation.Z));
        }

        public void ResetTransform()
        {
            Debug.Log(respawnPos.localPosition);
            player.transform.localPosition = respawnPos.localPosition;
            player.transform.localRotation = respawnPos.localRotation;
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
