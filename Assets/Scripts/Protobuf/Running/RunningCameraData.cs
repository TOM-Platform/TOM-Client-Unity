// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: running_camera_data.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
/// <summary>Holder for reflection information generated from running_camera_data.proto</summary>
public static partial class RunningCameraDataReflection {

  #region Descriptor
  /// <summary>File descriptor for running_camera_data.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static RunningCameraDataReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "ChlydW5uaW5nX2NhbWVyYV9kYXRhLnByb3RvIroBChFSdW5uaW5nQ2FtZXJh",
          "RGF0YRIVCgl0aW1lc3RhbXAYASABKAM6Ai0xEhYKCnBvc2l0aW9uX3gYAiAB",
          "KAE6Ai0xEhYKCnBvc2l0aW9uX3kYAyABKAE6Ai0xEhYKCnBvc2l0aW9uX3oY",
          "BCABKAE6Ai0xEhYKCnJvdGF0aW9uX3gYBSABKAE6Ai0xEhYKCnJvdGF0aW9u",
          "X3kYBiABKAE6Ai0xEhYKCnJvdGF0aW9uX3oYByABKAE6Ai0x"));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
          new pbr::GeneratedClrTypeInfo(typeof(global::RunningCameraData), global::RunningCameraData.Parser, new[]{ "Timestamp", "PositionX", "PositionY", "PositionZ", "RotationX", "RotationY", "RotationZ" }, null, null, null, null)
        }));
  }
  #endregion

}
#region Messages
public sealed partial class RunningCameraData : pb::IMessage<RunningCameraData>
#if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    , pb::IBufferMessage
#endif
{
  private static readonly pb::MessageParser<RunningCameraData> _parser = new pb::MessageParser<RunningCameraData>(() => new RunningCameraData());
  private pb::UnknownFieldSet _unknownFields;
  private int _hasBits0;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pb::MessageParser<RunningCameraData> Parser { get { return _parser; } }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public static pbr::MessageDescriptor Descriptor {
    get { return global::RunningCameraDataReflection.Descriptor.MessageTypes[0]; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public RunningCameraData() {
    OnConstruction();
  }

  partial void OnConstruction();

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public RunningCameraData(RunningCameraData other) : this() {
    _hasBits0 = other._hasBits0;
    timestamp_ = other.timestamp_;
    positionX_ = other.positionX_;
    positionY_ = other.positionY_;
    positionZ_ = other.positionZ_;
    rotationX_ = other.rotationX_;
    rotationY_ = other.rotationY_;
    rotationZ_ = other.rotationZ_;
    _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public RunningCameraData Clone() {
    return new RunningCameraData(this);
  }

  /// <summary>Field number for the "timestamp" field.</summary>
  public const int TimestampFieldNumber = 1;
  private readonly static long TimestampDefaultValue = -1L;

  private long timestamp_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public long Timestamp {
    get { if ((_hasBits0 & 1) != 0) { return timestamp_; } else { return TimestampDefaultValue; } }
    set {
      _hasBits0 |= 1;
      timestamp_ = value;
    }
  }
  /// <summary>Gets whether the "timestamp" field is set</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool HasTimestamp {
    get { return (_hasBits0 & 1) != 0; }
  }
  /// <summary>Clears the value of the "timestamp" field</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void ClearTimestamp() {
    _hasBits0 &= ~1;
  }

  /// <summary>Field number for the "position_x" field.</summary>
  public const int PositionXFieldNumber = 2;
  private readonly static double PositionXDefaultValue = -1D;

  private double positionX_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public double PositionX {
    get { if ((_hasBits0 & 2) != 0) { return positionX_; } else { return PositionXDefaultValue; } }
    set {
      _hasBits0 |= 2;
      positionX_ = value;
    }
  }
  /// <summary>Gets whether the "position_x" field is set</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool HasPositionX {
    get { return (_hasBits0 & 2) != 0; }
  }
  /// <summary>Clears the value of the "position_x" field</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void ClearPositionX() {
    _hasBits0 &= ~2;
  }

  /// <summary>Field number for the "position_y" field.</summary>
  public const int PositionYFieldNumber = 3;
  private readonly static double PositionYDefaultValue = -1D;

  private double positionY_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public double PositionY {
    get { if ((_hasBits0 & 4) != 0) { return positionY_; } else { return PositionYDefaultValue; } }
    set {
      _hasBits0 |= 4;
      positionY_ = value;
    }
  }
  /// <summary>Gets whether the "position_y" field is set</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool HasPositionY {
    get { return (_hasBits0 & 4) != 0; }
  }
  /// <summary>Clears the value of the "position_y" field</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void ClearPositionY() {
    _hasBits0 &= ~4;
  }

  /// <summary>Field number for the "position_z" field.</summary>
  public const int PositionZFieldNumber = 4;
  private readonly static double PositionZDefaultValue = -1D;

  private double positionZ_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public double PositionZ {
    get { if ((_hasBits0 & 8) != 0) { return positionZ_; } else { return PositionZDefaultValue; } }
    set {
      _hasBits0 |= 8;
      positionZ_ = value;
    }
  }
  /// <summary>Gets whether the "position_z" field is set</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool HasPositionZ {
    get { return (_hasBits0 & 8) != 0; }
  }
  /// <summary>Clears the value of the "position_z" field</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void ClearPositionZ() {
    _hasBits0 &= ~8;
  }

  /// <summary>Field number for the "rotation_x" field.</summary>
  public const int RotationXFieldNumber = 5;
  private readonly static double RotationXDefaultValue = -1D;

  private double rotationX_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public double RotationX {
    get { if ((_hasBits0 & 16) != 0) { return rotationX_; } else { return RotationXDefaultValue; } }
    set {
      _hasBits0 |= 16;
      rotationX_ = value;
    }
  }
  /// <summary>Gets whether the "rotation_x" field is set</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool HasRotationX {
    get { return (_hasBits0 & 16) != 0; }
  }
  /// <summary>Clears the value of the "rotation_x" field</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void ClearRotationX() {
    _hasBits0 &= ~16;
  }

  /// <summary>Field number for the "rotation_y" field.</summary>
  public const int RotationYFieldNumber = 6;
  private readonly static double RotationYDefaultValue = -1D;

  private double rotationY_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public double RotationY {
    get { if ((_hasBits0 & 32) != 0) { return rotationY_; } else { return RotationYDefaultValue; } }
    set {
      _hasBits0 |= 32;
      rotationY_ = value;
    }
  }
  /// <summary>Gets whether the "rotation_y" field is set</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool HasRotationY {
    get { return (_hasBits0 & 32) != 0; }
  }
  /// <summary>Clears the value of the "rotation_y" field</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void ClearRotationY() {
    _hasBits0 &= ~32;
  }

  /// <summary>Field number for the "rotation_z" field.</summary>
  public const int RotationZFieldNumber = 7;
  private readonly static double RotationZDefaultValue = -1D;

  private double rotationZ_;
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public double RotationZ {
    get { if ((_hasBits0 & 64) != 0) { return rotationZ_; } else { return RotationZDefaultValue; } }
    set {
      _hasBits0 |= 64;
      rotationZ_ = value;
    }
  }
  /// <summary>Gets whether the "rotation_z" field is set</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool HasRotationZ {
    get { return (_hasBits0 & 64) != 0; }
  }
  /// <summary>Clears the value of the "rotation_z" field</summary>
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void ClearRotationZ() {
    _hasBits0 &= ~64;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override bool Equals(object other) {
    return Equals(other as RunningCameraData);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public bool Equals(RunningCameraData other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (Timestamp != other.Timestamp) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PositionX, other.PositionX)) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PositionY, other.PositionY)) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(PositionZ, other.PositionZ)) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(RotationX, other.RotationX)) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(RotationY, other.RotationY)) return false;
    if (!pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.Equals(RotationZ, other.RotationZ)) return false;
    return Equals(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override int GetHashCode() {
    int hash = 1;
    if (HasTimestamp) hash ^= Timestamp.GetHashCode();
    if (HasPositionX) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PositionX);
    if (HasPositionY) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PositionY);
    if (HasPositionZ) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(PositionZ);
    if (HasRotationX) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(RotationX);
    if (HasRotationY) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(RotationY);
    if (HasRotationZ) hash ^= pbc::ProtobufEqualityComparers.BitwiseDoubleEqualityComparer.GetHashCode(RotationZ);
    if (_unknownFields != null) {
      hash ^= _unknownFields.GetHashCode();
    }
    return hash;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void WriteTo(pb::CodedOutputStream output) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    output.WriteRawMessage(this);
  #else
    if (HasTimestamp) {
      output.WriteRawTag(8);
      output.WriteInt64(Timestamp);
    }
    if (HasPositionX) {
      output.WriteRawTag(17);
      output.WriteDouble(PositionX);
    }
    if (HasPositionY) {
      output.WriteRawTag(25);
      output.WriteDouble(PositionY);
    }
    if (HasPositionZ) {
      output.WriteRawTag(33);
      output.WriteDouble(PositionZ);
    }
    if (HasRotationX) {
      output.WriteRawTag(41);
      output.WriteDouble(RotationX);
    }
    if (HasRotationY) {
      output.WriteRawTag(49);
      output.WriteDouble(RotationY);
    }
    if (HasRotationZ) {
      output.WriteRawTag(57);
      output.WriteDouble(RotationZ);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(output);
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
    if (HasTimestamp) {
      output.WriteRawTag(8);
      output.WriteInt64(Timestamp);
    }
    if (HasPositionX) {
      output.WriteRawTag(17);
      output.WriteDouble(PositionX);
    }
    if (HasPositionY) {
      output.WriteRawTag(25);
      output.WriteDouble(PositionY);
    }
    if (HasPositionZ) {
      output.WriteRawTag(33);
      output.WriteDouble(PositionZ);
    }
    if (HasRotationX) {
      output.WriteRawTag(41);
      output.WriteDouble(RotationX);
    }
    if (HasRotationY) {
      output.WriteRawTag(49);
      output.WriteDouble(RotationY);
    }
    if (HasRotationZ) {
      output.WriteRawTag(57);
      output.WriteDouble(RotationZ);
    }
    if (_unknownFields != null) {
      _unknownFields.WriteTo(ref output);
    }
  }
  #endif

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public int CalculateSize() {
    int size = 0;
    if (HasTimestamp) {
      size += 1 + pb::CodedOutputStream.ComputeInt64Size(Timestamp);
    }
    if (HasPositionX) {
      size += 1 + 8;
    }
    if (HasPositionY) {
      size += 1 + 8;
    }
    if (HasPositionZ) {
      size += 1 + 8;
    }
    if (HasRotationX) {
      size += 1 + 8;
    }
    if (HasRotationY) {
      size += 1 + 8;
    }
    if (HasRotationZ) {
      size += 1 + 8;
    }
    if (_unknownFields != null) {
      size += _unknownFields.CalculateSize();
    }
    return size;
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(RunningCameraData other) {
    if (other == null) {
      return;
    }
    if (other.HasTimestamp) {
      Timestamp = other.Timestamp;
    }
    if (other.HasPositionX) {
      PositionX = other.PositionX;
    }
    if (other.HasPositionY) {
      PositionY = other.PositionY;
    }
    if (other.HasPositionZ) {
      PositionZ = other.PositionZ;
    }
    if (other.HasRotationX) {
      RotationX = other.RotationX;
    }
    if (other.HasRotationY) {
      RotationY = other.RotationY;
    }
    if (other.HasRotationZ) {
      RotationZ = other.RotationZ;
    }
    _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
  }

  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  public void MergeFrom(pb::CodedInputStream input) {
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    input.ReadRawMessage(this);
  #else
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
          break;
        case 8: {
          Timestamp = input.ReadInt64();
          break;
        }
        case 17: {
          PositionX = input.ReadDouble();
          break;
        }
        case 25: {
          PositionY = input.ReadDouble();
          break;
        }
        case 33: {
          PositionZ = input.ReadDouble();
          break;
        }
        case 41: {
          RotationX = input.ReadDouble();
          break;
        }
        case 49: {
          RotationY = input.ReadDouble();
          break;
        }
        case 57: {
          RotationZ = input.ReadDouble();
          break;
        }
      }
    }
  #endif
  }

  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
  [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
  [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
  void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
          break;
        case 8: {
          Timestamp = input.ReadInt64();
          break;
        }
        case 17: {
          PositionX = input.ReadDouble();
          break;
        }
        case 25: {
          PositionY = input.ReadDouble();
          break;
        }
        case 33: {
          PositionZ = input.ReadDouble();
          break;
        }
        case 41: {
          RotationX = input.ReadDouble();
          break;
        }
        case 49: {
          RotationY = input.ReadDouble();
          break;
        }
        case 57: {
          RotationZ = input.ReadDouble();
          break;
        }
      }
    }
  }
  #endif

}

#endregion


#endregion Designer generated code