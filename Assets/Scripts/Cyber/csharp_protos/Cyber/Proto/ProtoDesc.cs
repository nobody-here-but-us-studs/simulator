// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: cyber/proto/proto_desc.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Apollo.Cyber.Proto {

  /// <summary>Holder for reflection information generated from cyber/proto/proto_desc.proto</summary>
  public static partial class ProtoDescReflection {

    #region Descriptor
    /// <summary>File descriptor for cyber/proto/proto_desc.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ProtoDescReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChxjeWJlci9wcm90by9wcm90b19kZXNjLnByb3RvEhJhcG9sbG8uY3liZXIu",
            "cHJvdG8iTgoJUHJvdG9EZXNjEgwKBGRlc2MYASABKAwSMwoMZGVwZW5kZW5j",
            "aWVzGAIgAygLMh0uYXBvbGxvLmN5YmVyLnByb3RvLlByb3RvRGVzY2IGcHJv",
            "dG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Apollo.Cyber.Proto.ProtoDesc), global::Apollo.Cyber.Proto.ProtoDesc.Parser, new[]{ "Desc", "Dependencies" }, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class ProtoDesc : pb::IMessage<ProtoDesc> {
    private static readonly pb::MessageParser<ProtoDesc> _parser = new pb::MessageParser<ProtoDesc>(() => new ProtoDesc());
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<ProtoDesc> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Apollo.Cyber.Proto.ProtoDescReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ProtoDesc() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ProtoDesc(ProtoDesc other) : this() {
      desc_ = other.desc_;
      dependencies_ = other.dependencies_.Clone();
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public ProtoDesc Clone() {
      return new ProtoDesc(this);
    }

    /// <summary>Field number for the "desc" field.</summary>
    public const int DescFieldNumber = 1;
    private pb::ByteString desc_ = pb::ByteString.Empty;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString Desc {
      get { return desc_; }
      set {
        desc_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "dependencies" field.</summary>
    public const int DependenciesFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Apollo.Cyber.Proto.ProtoDesc> _repeated_dependencies_codec
        = pb::FieldCodec.ForMessage(18, global::Apollo.Cyber.Proto.ProtoDesc.Parser);
    private readonly pbc::RepeatedField<global::Apollo.Cyber.Proto.ProtoDesc> dependencies_ = new pbc::RepeatedField<global::Apollo.Cyber.Proto.ProtoDesc>();
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Apollo.Cyber.Proto.ProtoDesc> Dependencies {
      get { return dependencies_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as ProtoDesc);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(ProtoDesc other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Desc != other.Desc) return false;
      if(!dependencies_.Equals(other.dependencies_)) return false;
      return true;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (Desc.Length != 0) hash ^= Desc.GetHashCode();
      hash ^= dependencies_.GetHashCode();
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
      if (Desc.Length != 0) {
        output.WriteRawTag(10);
        output.WriteBytes(Desc);
      }
      dependencies_.WriteTo(output, _repeated_dependencies_codec);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (Desc.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Desc);
      }
      size += dependencies_.CalculateSize(_repeated_dependencies_codec);
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(ProtoDesc other) {
      if (other == null) {
        return;
      }
      if (other.Desc.Length != 0) {
        Desc = other.Desc;
      }
      dependencies_.Add(other.dependencies_);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            input.SkipLastField();
            break;
          case 10: {
            Desc = input.ReadBytes();
            break;
          }
          case 18: {
            dependencies_.AddEntriesFrom(input, _repeated_dependencies_codec);
            break;
          }
        }
      }
    }

  }

  #endregion

}

#endregion Designer generated code