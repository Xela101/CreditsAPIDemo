/**
 * Autogenerated by Thrift Compiler (0.11.0)
 *
 * DO NOT EDIT UNLESS YOU ARE SURE THAT YOU KNOW WHAT YOU ARE DOING
 *  @generated
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Thrift;
using Thrift.Collections;
using System.Runtime.Serialization;
using Thrift.Protocol;
using Thrift.Transport;

namespace NodeApi
{

  #if !SILVERLIGHT
  [Serializable]
  #endif
  public partial class TransactionId : TBase
  {
    private byte[] _poolHash;
    private int _index;

    public byte[] PoolHash
    {
      get
      {
        return _poolHash;
      }
      set
      {
        __isset.poolHash = true;
        this._poolHash = value;
      }
    }

    public int Index
    {
      get
      {
        return _index;
      }
      set
      {
        __isset.index = true;
        this._index = value;
      }
    }


    public Isset __isset;
    #if !SILVERLIGHT
    [Serializable]
    #endif
    public struct Isset {
      public bool poolHash;
      public bool index;
    }

    public TransactionId() {
    }

    public void Read (TProtocol iprot)
    {
      iprot.IncrementRecursionDepth();
      try
      {
        TField field;
        iprot.ReadStructBegin();
        while (true)
        {
          field = iprot.ReadFieldBegin();
          if (field.Type == TType.Stop) { 
            break;
          }
          switch (field.ID)
          {
            case 1:
              if (field.Type == TType.String) {
                PoolHash = iprot.ReadBinary();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            case 2:
              if (field.Type == TType.I32) {
                Index = iprot.ReadI32();
              } else { 
                TProtocolUtil.Skip(iprot, field.Type);
              }
              break;
            default: 
              TProtocolUtil.Skip(iprot, field.Type);
              break;
          }
          iprot.ReadFieldEnd();
        }
        iprot.ReadStructEnd();
      }
      finally
      {
        iprot.DecrementRecursionDepth();
      }
    }

    public void Write(TProtocol oprot) {
      oprot.IncrementRecursionDepth();
      try
      {
        TStruct struc = new TStruct("TransactionId");
        oprot.WriteStructBegin(struc);
        TField field = new TField();
        if (PoolHash != null && __isset.poolHash) {
          field.Name = "poolHash";
          field.Type = TType.String;
          field.ID = 1;
          oprot.WriteFieldBegin(field);
          oprot.WriteBinary(PoolHash);
          oprot.WriteFieldEnd();
        }
        if (__isset.index) {
          field.Name = "index";
          field.Type = TType.I32;
          field.ID = 2;
          oprot.WriteFieldBegin(field);
          oprot.WriteI32(Index);
          oprot.WriteFieldEnd();
        }
        oprot.WriteFieldStop();
        oprot.WriteStructEnd();
      }
      finally
      {
        oprot.DecrementRecursionDepth();
      }
    }

    public override string ToString() {
      StringBuilder __sb = new StringBuilder("TransactionId(");
      bool __first = true;
      if (PoolHash != null && __isset.poolHash) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("PoolHash: ");
        __sb.Append(PoolHash);
      }
      if (__isset.index) {
        if(!__first) { __sb.Append(", "); }
        __first = false;
        __sb.Append("Index: ");
        __sb.Append(Index);
      }
      __sb.Append(")");
      return __sb.ToString();
    }

  }

}
