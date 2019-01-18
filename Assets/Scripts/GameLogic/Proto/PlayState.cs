// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: PlayState.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Protobuf.State {

  /// <summary>Holder for reflection information generated from PlayState.proto</summary>
  public static partial class PlayStateReflection {

    #region Descriptor
    /// <summary>File descriptor for PlayState.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static PlayStateReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cg9QbGF5U3RhdGUucHJvdG8SBXN0YXRlIi8KDFZlY3RvcjNQcm90bxIJCgF4",
            "GAEgASgCEgkKAXkYAiABKAISCQoBehgDIAEoAiKCAQoOVHJhbnNmb3JtUHJv",
            "dG8SJQoIcG9zaXRpb24YASABKAsyEy5zdGF0ZS5WZWN0b3IzUHJvdG8SJQoI",
            "cm90YXRpb24YAiABKAsyEy5zdGF0ZS5WZWN0b3IzUHJvdG8SIgoFc2NhbGUY",
            "AyABKAsyEy5zdGF0ZS5WZWN0b3IzUHJvdG8ioQEKCVBsYXlTdGF0ZRIoCgl0",
            "cmFuc2Zvcm0YASABKAsyFS5zdGF0ZS5UcmFuc2Zvcm1Qcm90bxIRCglhbmlt",
            "U3RhdGUYAiABKAUSDgoGaGVhbHRoGAMgASgFEhEKCWtpbGxDb3VudBgEIAEo",
            "BRISCgpkZWF0aENvdW50GAUgASgFEg4KBnJvb21JZBgGIAEoBRIQCghjbG50",
            "TmFtZRgHIAEoCSJnCgpXb3JsZFN0YXRlEg4KBnJvb21JZBgBIAEoBRIQCghj",
            "bG50TmFtZRgCIAEoCRIoCgl0cmFuc2Zvcm0YAyABKAsyFS5zdGF0ZS5UcmFu",
            "c2Zvcm1Qcm90bxINCgVmaXJlZBgEIAEoCEIYqgIVR29vZ2xlLlByb3RvYnVm",
            "LlN0YXRlYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Protobuf.State.Vector3Proto), global::Google.Protobuf.State.Vector3Proto.Parser, new[]{ "X", "Y", "Z" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Protobuf.State.TransformProto), global::Google.Protobuf.State.TransformProto.Parser, new[]{ "Position", "Rotation", "Scale" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Protobuf.State.PlayState), global::Google.Protobuf.State.PlayState.Parser, new[]{ "Transform", "AnimState", "Health", "KillCount", "DeathCount", "RoomId", "ClntName" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Protobuf.State.WorldState), global::Google.Protobuf.State.WorldState.Parser, new[]{ "RoomId", "ClntName", "Transform", "Fired" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Vector3Proto : pb::IMessage<Vector3Proto> {
    private static readonly pb::MessageParser<Vector3Proto> _parser = new pb::MessageParser<Vector3Proto>(() => new Vector3Proto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Vector3Proto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.State.PlayStateReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Vector3Proto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Vector3Proto(Vector3Proto other) : this() {
      x_ = other.x_;
      y_ = other.y_;
      z_ = other.z_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Vector3Proto Clone() {
      return new Vector3Proto(this);
    }

    /// <summary>Field number for the "x" field.</summary>
    public const int XFieldNumber = 1;
    private float x_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float X {
      get { return x_; }
      set {
        x_ = value;
      }
    }

    /// <summary>Field number for the "y" field.</summary>
    public const int YFieldNumber = 2;
    private float y_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Y {
      get { return y_; }
      set {
        y_ = value;
      }
    }

    /// <summary>Field number for the "z" field.</summary>
    public const int ZFieldNumber = 3;
    private float z_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float Z {
      get { return z_; }
      set {
        z_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Vector3Proto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Vector3Proto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(X, other.X)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Y, other.Y)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(Z, other.Z)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (X != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(X);
      if (Y != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Y);
      if (Z != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(Z);
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (X != 0F) {
        output.WriteRawTag(13);
        output.WriteFloat(X);
      }
      if (Y != 0F) {
        output.WriteRawTag(21);
        output.WriteFloat(Y);
      }
      if (Z != 0F) {
        output.WriteRawTag(29);
        output.WriteFloat(Z);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (X != 0F) {
        size += 1 + 4;
      }
      if (Y != 0F) {
        size += 1 + 4;
      }
      if (Z != 0F) {
        size += 1 + 4;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Vector3Proto other) {
      if (other == null) {
        return;
      }
      if (other.X != 0F) {
        X = other.X;
      }
      if (other.Y != 0F) {
        Y = other.Y;
      }
      if (other.Z != 0F) {
        Z = other.Z;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 13: {
            X = input.ReadFloat();
            break;
          }
          case 21: {
            Y = input.ReadFloat();
            break;
          }
          case 29: {
            Z = input.ReadFloat();
            break;
          }
        }
      }
    }

  }

  public sealed partial class TransformProto : pb::IMessage<TransformProto> {
    private static readonly pb::MessageParser<TransformProto> _parser = new pb::MessageParser<TransformProto>(() => new TransformProto());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<TransformProto> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.State.PlayStateReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TransformProto() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TransformProto(TransformProto other) : this() {
      position_ = other.position_ != null ? other.position_.Clone() : null;
      rotation_ = other.rotation_ != null ? other.rotation_.Clone() : null;
      scale_ = other.scale_ != null ? other.scale_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public TransformProto Clone() {
      return new TransformProto(this);
    }

    /// <summary>Field number for the "position" field.</summary>
    public const int PositionFieldNumber = 1;
    private global::Google.Protobuf.State.Vector3Proto position_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Protobuf.State.Vector3Proto Position {
      get { return position_; }
      set {
        position_ = value;
      }
    }

    /// <summary>Field number for the "rotation" field.</summary>
    public const int RotationFieldNumber = 2;
    private global::Google.Protobuf.State.Vector3Proto rotation_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Protobuf.State.Vector3Proto Rotation {
      get { return rotation_; }
      set {
        rotation_ = value;
      }
    }

    /// <summary>Field number for the "scale" field.</summary>
    public const int ScaleFieldNumber = 3;
    private global::Google.Protobuf.State.Vector3Proto scale_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Protobuf.State.Vector3Proto Scale {
      get { return scale_; }
      set {
        scale_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as TransformProto);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(TransformProto other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Position, other.Position)) return false;
      if (!object.Equals(Rotation, other.Rotation)) return false;
      if (!object.Equals(Scale, other.Scale)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (position_ != null) hash ^= Position.GetHashCode();
      if (rotation_ != null) hash ^= Rotation.GetHashCode();
      if (scale_ != null) hash ^= Scale.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (position_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Position);
      }
      if (rotation_ != null) {
        output.WriteRawTag(18);
        output.WriteMessage(Rotation);
      }
      if (scale_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(Scale);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (position_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Position);
      }
      if (rotation_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Rotation);
      }
      if (scale_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Scale);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(TransformProto other) {
      if (other == null) {
        return;
      }
      if (other.position_ != null) {
        if (position_ == null) {
          position_ = new global::Google.Protobuf.State.Vector3Proto();
        }
        Position.MergeFrom(other.Position);
      }
      if (other.rotation_ != null) {
        if (rotation_ == null) {
          rotation_ = new global::Google.Protobuf.State.Vector3Proto();
        }
        Rotation.MergeFrom(other.Rotation);
      }
      if (other.scale_ != null) {
        if (scale_ == null) {
          scale_ = new global::Google.Protobuf.State.Vector3Proto();
        }
        Scale.MergeFrom(other.Scale);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (position_ == null) {
              position_ = new global::Google.Protobuf.State.Vector3Proto();
            }
            input.ReadMessage(position_);
            break;
          }
          case 18: {
            if (rotation_ == null) {
              rotation_ = new global::Google.Protobuf.State.Vector3Proto();
            }
            input.ReadMessage(rotation_);
            break;
          }
          case 26: {
            if (scale_ == null) {
              scale_ = new global::Google.Protobuf.State.Vector3Proto();
            }
            input.ReadMessage(scale_);
            break;
          }
        }
      }
    }

  }

  public sealed partial class PlayState : pb::IMessage<PlayState> {
    private static readonly pb::MessageParser<PlayState> _parser = new pb::MessageParser<PlayState>(() => new PlayState());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<PlayState> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.State.PlayStateReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PlayState() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PlayState(PlayState other) : this() {
      transform_ = other.transform_ != null ? other.transform_.Clone() : null;
      animState_ = other.animState_;
      health_ = other.health_;
      killCount_ = other.killCount_;
      deathCount_ = other.deathCount_;
      roomId_ = other.roomId_;
      clntName_ = other.clntName_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public PlayState Clone() {
      return new PlayState(this);
    }

    /// <summary>Field number for the "transform" field.</summary>
    public const int TransformFieldNumber = 1;
    private global::Google.Protobuf.State.TransformProto transform_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Protobuf.State.TransformProto Transform {
      get { return transform_; }
      set {
        transform_ = value;
      }
    }

    /// <summary>Field number for the "animState" field.</summary>
    public const int AnimStateFieldNumber = 2;
    private int animState_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int AnimState {
      get { return animState_; }
      set {
        animState_ = value;
      }
    }

    /// <summary>Field number for the "health" field.</summary>
    public const int HealthFieldNumber = 3;
    private int health_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Health {
      get { return health_; }
      set {
        health_ = value;
      }
    }

    /// <summary>Field number for the "killCount" field.</summary>
    public const int KillCountFieldNumber = 4;
    private int killCount_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int KillCount {
      get { return killCount_; }
      set {
        killCount_ = value;
      }
    }

    /// <summary>Field number for the "deathCount" field.</summary>
    public const int DeathCountFieldNumber = 5;
    private int deathCount_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int DeathCount {
      get { return deathCount_; }
      set {
        deathCount_ = value;
      }
    }

    /// <summary>Field number for the "roomId" field.</summary>
    public const int RoomIdFieldNumber = 6;
    private int roomId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int RoomId {
      get { return roomId_; }
      set {
        roomId_ = value;
      }
    }

    /// <summary>Field number for the "clntName" field.</summary>
    public const int ClntNameFieldNumber = 7;
    private string clntName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ClntName {
      get { return clntName_; }
      set {
        clntName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as PlayState);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(PlayState other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Transform, other.Transform)) return false;
      if (AnimState != other.AnimState) return false;
      if (Health != other.Health) return false;
      if (KillCount != other.KillCount) return false;
      if (DeathCount != other.DeathCount) return false;
      if (RoomId != other.RoomId) return false;
      if (ClntName != other.ClntName) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (transform_ != null) hash ^= Transform.GetHashCode();
      if (AnimState != 0) hash ^= AnimState.GetHashCode();
      if (Health != 0) hash ^= Health.GetHashCode();
      if (KillCount != 0) hash ^= KillCount.GetHashCode();
      if (DeathCount != 0) hash ^= DeathCount.GetHashCode();
      if (RoomId != 0) hash ^= RoomId.GetHashCode();
      if (ClntName.Length != 0) hash ^= ClntName.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (transform_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Transform);
      }
      if (AnimState != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(AnimState);
      }
      if (Health != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Health);
      }
      if (KillCount != 0) {
        output.WriteRawTag(32);
        output.WriteInt32(KillCount);
      }
      if (DeathCount != 0) {
        output.WriteRawTag(40);
        output.WriteInt32(DeathCount);
      }
      if (RoomId != 0) {
        output.WriteRawTag(48);
        output.WriteInt32(RoomId);
      }
      if (ClntName.Length != 0) {
        output.WriteRawTag(58);
        output.WriteString(ClntName);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (transform_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Transform);
      }
      if (AnimState != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(AnimState);
      }
      if (Health != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Health);
      }
      if (KillCount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(KillCount);
      }
      if (DeathCount != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(DeathCount);
      }
      if (RoomId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(RoomId);
      }
      if (ClntName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ClntName);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(PlayState other) {
      if (other == null) {
        return;
      }
      if (other.transform_ != null) {
        if (transform_ == null) {
          transform_ = new global::Google.Protobuf.State.TransformProto();
        }
        Transform.MergeFrom(other.Transform);
      }
      if (other.AnimState != 0) {
        AnimState = other.AnimState;
      }
      if (other.Health != 0) {
        Health = other.Health;
      }
      if (other.KillCount != 0) {
        KillCount = other.KillCount;
      }
      if (other.DeathCount != 0) {
        DeathCount = other.DeathCount;
      }
      if (other.RoomId != 0) {
        RoomId = other.RoomId;
      }
      if (other.ClntName.Length != 0) {
        ClntName = other.ClntName;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (transform_ == null) {
              transform_ = new global::Google.Protobuf.State.TransformProto();
            }
            input.ReadMessage(transform_);
            break;
          }
          case 16: {
            AnimState = input.ReadInt32();
            break;
          }
          case 24: {
            Health = input.ReadInt32();
            break;
          }
          case 32: {
            KillCount = input.ReadInt32();
            break;
          }
          case 40: {
            DeathCount = input.ReadInt32();
            break;
          }
          case 48: {
            RoomId = input.ReadInt32();
            break;
          }
          case 58: {
            ClntName = input.ReadString();
            break;
          }
        }
      }
    }

  }

  public sealed partial class WorldState : pb::IMessage<WorldState> {
    private static readonly pb::MessageParser<WorldState> _parser = new pb::MessageParser<WorldState>(() => new WorldState());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<WorldState> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.State.PlayStateReflection.Descriptor.MessageTypes[3]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public WorldState() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public WorldState(WorldState other) : this() {
      roomId_ = other.roomId_;
      clntName_ = other.clntName_;
      transform_ = other.transform_ != null ? other.transform_.Clone() : null;
      fired_ = other.fired_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public WorldState Clone() {
      return new WorldState(this);
    }

    /// <summary>Field number for the "roomId" field.</summary>
    public const int RoomIdFieldNumber = 1;
    private int roomId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int RoomId {
      get { return roomId_; }
      set {
        roomId_ = value;
      }
    }

    /// <summary>Field number for the "clntName" field.</summary>
    public const int ClntNameFieldNumber = 2;
    private string clntName_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string ClntName {
      get { return clntName_; }
      set {
        clntName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "transform" field.</summary>
    public const int TransformFieldNumber = 3;
    private global::Google.Protobuf.State.TransformProto transform_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Protobuf.State.TransformProto Transform {
      get { return transform_; }
      set {
        transform_ = value;
      }
    }

    /// <summary>Field number for the "fired" field.</summary>
    public const int FiredFieldNumber = 4;
    private bool fired_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Fired {
      get { return fired_; }
      set {
        fired_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as WorldState);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(WorldState other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (RoomId != other.RoomId) return false;
      if (ClntName != other.ClntName) return false;
      if (!object.Equals(Transform, other.Transform)) return false;
      if (Fired != other.Fired) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (RoomId != 0) hash ^= RoomId.GetHashCode();
      if (ClntName.Length != 0) hash ^= ClntName.GetHashCode();
      if (transform_ != null) hash ^= Transform.GetHashCode();
      if (Fired != false) hash ^= Fired.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (RoomId != 0) {
        output.WriteRawTag(8);
        output.WriteInt32(RoomId);
      }
      if (ClntName.Length != 0) {
        output.WriteRawTag(18);
        output.WriteString(ClntName);
      }
      if (transform_ != null) {
        output.WriteRawTag(26);
        output.WriteMessage(Transform);
      }
      if (Fired != false) {
        output.WriteRawTag(32);
        output.WriteBool(Fired);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (RoomId != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(RoomId);
      }
      if (ClntName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ClntName);
      }
      if (transform_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Transform);
      }
      if (Fired != false) {
        size += 1 + 1;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(WorldState other) {
      if (other == null) {
        return;
      }
      if (other.RoomId != 0) {
        RoomId = other.RoomId;
      }
      if (other.ClntName.Length != 0) {
        ClntName = other.ClntName;
      }
      if (other.transform_ != null) {
        if (transform_ == null) {
          transform_ = new global::Google.Protobuf.State.TransformProto();
        }
        Transform.MergeFrom(other.Transform);
      }
      if (other.Fired != false) {
        Fired = other.Fired;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 8: {
            RoomId = input.ReadInt32();
            break;
          }
          case 18: {
            ClntName = input.ReadString();
            break;
          }
          case 26: {
            if (transform_ == null) {
              transform_ = new global::Google.Protobuf.State.TransformProto();
            }
            input.ReadMessage(transform_);
            break;
          }
          case 32: {
            Fired = input.ReadBool();
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
