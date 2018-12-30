// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: room.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Protobuf.Packet.Room {

  /// <summary>Holder for reflection information generated from room.proto</summary>
  public static partial class RoomReflection {

    #region Descriptor
    /// <summary>File descriptor for room.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static RoomReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "Cgpyb29tLnByb3RvEgZwYWNrZXQaH2dvb2dsZS9wcm90b2J1Zi90aW1lc3Rh",
            "bXAucHJvdG8iIgoGQ2xpZW50EgoKAmlwGAEgASgJEgwKBHBvcnQYAiABKAUi",
            "hwEKBFJvb20SDAoEbmFtZRgBIAEoCRINCgVsaW1pdBgCIAEoBRIPCgdjdXJy",
            "ZW50GAMgASgFEh8KB2NsaWVudHMYBCADKAsyDi5wYWNrZXQuQ2xpZW50EjAK",
            "DGxhc3RfdXBkYXRlZBgFIAEoCzIaLmdvb2dsZS5wcm90b2J1Zi5UaW1lc3Rh",
            "bXAiJwoIUm9vbUxpc3QSGwoFcm9vbXMYASADKAsyDC5wYWNrZXQuUm9vbUIe",
            "qgIbR29vZ2xlLlByb3RvYnVmLlBhY2tldC5Sb29tYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Protobuf.WellKnownTypes.TimestampReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Protobuf.Packet.Room.Client), global::Google.Protobuf.Packet.Room.Client.Parser, new[]{ "Ip", "Port" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Protobuf.Packet.Room.Room), global::Google.Protobuf.Packet.Room.Room.Parser, new[]{ "Name", "Limit", "Current", "Clients", "LastUpdated" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Protobuf.Packet.Room.RoomList), global::Google.Protobuf.Packet.Room.RoomList.Parser, new[]{ "Rooms" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class Client : pb::IMessage<Client> {
    private static readonly pb::MessageParser<Client> _parser = new pb::MessageParser<Client>(() => new Client());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Client> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.Packet.Room.RoomReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Client() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Client(Client other) : this() {
      ip_ = other.ip_;
      port_ = other.port_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Client Clone() {
      return new Client(this);
    }

    /// <summary>Field number for the "ip" field.</summary>
    public const int IpFieldNumber = 1;
    private string ip_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Ip {
      get { return ip_; }
      set {
        ip_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "port" field.</summary>
    public const int PortFieldNumber = 2;
    private int port_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Port {
      get { return port_; }
      set {
        port_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Client);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Client other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Ip != other.Ip) return false;
      if (Port != other.Port) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Ip.Length != 0) hash ^= Ip.GetHashCode();
      if (Port != 0) hash ^= Port.GetHashCode();
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
      if (Ip.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Ip);
      }
      if (Port != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Port);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Ip.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Ip);
      }
      if (Port != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Port);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Client other) {
      if (other == null) {
        return;
      }
      if (other.Ip.Length != 0) {
        Ip = other.Ip;
      }
      if (other.Port != 0) {
        Port = other.Port;
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
            Ip = input.ReadString();
            break;
          }
          case 16: {
            Port = input.ReadInt32();
            break;
          }
        }
      }
    }

  }

  public sealed partial class Room : pb::IMessage<Room> {
    private static readonly pb::MessageParser<Room> _parser = new pb::MessageParser<Room>(() => new Room());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<Room> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.Packet.Room.RoomReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Room() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Room(Room other) : this() {
      name_ = other.name_;
      limit_ = other.limit_;
      current_ = other.current_;
      clients_ = other.clients_.Clone();
      lastUpdated_ = other.lastUpdated_ != null ? other.lastUpdated_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public Room Clone() {
      return new Room(this);
    }

    /// <summary>Field number for the "name" field.</summary>
    public const int NameFieldNumber = 1;
    private string name_ = "";
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string Name {
      get { return name_; }
      set {
        name_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "limit" field.</summary>
    public const int LimitFieldNumber = 2;
    private int limit_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Limit {
      get { return limit_; }
      set {
        limit_ = value;
      }
    }

    /// <summary>Field number for the "current" field.</summary>
    public const int CurrentFieldNumber = 3;
    private int current_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int Current {
      get { return current_; }
      set {
        current_ = value;
      }
    }

    /// <summary>Field number for the "clients" field.</summary>
    public const int ClientsFieldNumber = 4;
    private static readonly pb::FieldCodec<global::Google.Protobuf.Packet.Room.Client> _repeated_clients_codec
        = pb::FieldCodec.ForMessage(34, global::Google.Protobuf.Packet.Room.Client.Parser);
    private readonly pbc::RepeatedField<global::Google.Protobuf.Packet.Room.Client> clients_ = new pbc::RepeatedField<global::Google.Protobuf.Packet.Room.Client>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Google.Protobuf.Packet.Room.Client> Clients {
      get { return clients_; }
    }

    /// <summary>Field number for the "last_updated" field.</summary>
    public const int LastUpdatedFieldNumber = 5;
    private global::Google.Protobuf.WellKnownTypes.Timestamp lastUpdated_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Protobuf.WellKnownTypes.Timestamp LastUpdated {
      get { return lastUpdated_; }
      set {
        lastUpdated_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as Room);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(Room other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Name != other.Name) return false;
      if (Limit != other.Limit) return false;
      if (Current != other.Current) return false;
      if(!clients_.Equals(other.clients_)) return false;
      if (!object.Equals(LastUpdated, other.LastUpdated)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Name.Length != 0) hash ^= Name.GetHashCode();
      if (Limit != 0) hash ^= Limit.GetHashCode();
      if (Current != 0) hash ^= Current.GetHashCode();
      hash ^= clients_.GetHashCode();
      if (lastUpdated_ != null) hash ^= LastUpdated.GetHashCode();
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
      if (Name.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(Name);
      }
      if (Limit != 0) {
        output.WriteRawTag(16);
        output.WriteInt32(Limit);
      }
      if (Current != 0) {
        output.WriteRawTag(24);
        output.WriteInt32(Current);
      }
      clients_.WriteTo(output, _repeated_clients_codec);
      if (lastUpdated_ != null) {
        output.WriteRawTag(42);
        output.WriteMessage(LastUpdated);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Name.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Name);
      }
      if (Limit != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Limit);
      }
      if (Current != 0) {
        size += 1 + pb::CodedOutputStream.ComputeInt32Size(Current);
      }
      size += clients_.CalculateSize(_repeated_clients_codec);
      if (lastUpdated_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(LastUpdated);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(Room other) {
      if (other == null) {
        return;
      }
      if (other.Name.Length != 0) {
        Name = other.Name;
      }
      if (other.Limit != 0) {
        Limit = other.Limit;
      }
      if (other.Current != 0) {
        Current = other.Current;
      }
      clients_.Add(other.clients_);
      if (other.lastUpdated_ != null) {
        if (lastUpdated_ == null) {
          lastUpdated_ = new global::Google.Protobuf.WellKnownTypes.Timestamp();
        }
        LastUpdated.MergeFrom(other.LastUpdated);
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
            Name = input.ReadString();
            break;
          }
          case 16: {
            Limit = input.ReadInt32();
            break;
          }
          case 24: {
            Current = input.ReadInt32();
            break;
          }
          case 34: {
            clients_.AddEntriesFrom(input, _repeated_clients_codec);
            break;
          }
          case 42: {
            if (lastUpdated_ == null) {
              lastUpdated_ = new global::Google.Protobuf.WellKnownTypes.Timestamp();
            }
            input.ReadMessage(lastUpdated_);
            break;
          }
        }
      }
    }

  }

  public sealed partial class RoomList : pb::IMessage<RoomList> {
    private static readonly pb::MessageParser<RoomList> _parser = new pb::MessageParser<RoomList>(() => new RoomList());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<RoomList> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Protobuf.Packet.Room.RoomReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RoomList() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RoomList(RoomList other) : this() {
      rooms_ = other.rooms_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public RoomList Clone() {
      return new RoomList(this);
    }

    /// <summary>Field number for the "rooms" field.</summary>
    public const int RoomsFieldNumber = 1;
    private static readonly pb::FieldCodec<global::Google.Protobuf.Packet.Room.Room> _repeated_rooms_codec
        = pb::FieldCodec.ForMessage(10, global::Google.Protobuf.Packet.Room.Room.Parser);
    private readonly pbc::RepeatedField<global::Google.Protobuf.Packet.Room.Room> rooms_ = new pbc::RepeatedField<global::Google.Protobuf.Packet.Room.Room>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Google.Protobuf.Packet.Room.Room> Rooms {
      get { return rooms_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as RoomList);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(RoomList other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if(!rooms_.Equals(other.rooms_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      hash ^= rooms_.GetHashCode();
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
      rooms_.WriteTo(output, _repeated_rooms_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      size += rooms_.CalculateSize(_repeated_rooms_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(RoomList other) {
      if (other == null) {
        return;
      }
      rooms_.Add(other.rooms_);
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
            rooms_.AddEntriesFrom(input, _repeated_rooms_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code
