using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class ByteTool
{
    public static byte[] Object2Bytes(object obj)
    {
        byte[] buff;
        using (MemoryStream ms = new MemoryStream())
        {
            IFormatter iFormatter = new BinaryFormatter();
            iFormatter.Serialize(ms, obj);
            buff = ms.GetBuffer();
        }
        return buff;
    }


    static public int ReadInt(byte[] bytes, ref int offset)
    {
        int value;
        value = (int)(((bytes[offset] & 0xFF) << 24)
                | ((bytes[offset + 1] & 0xFF) << 16)
                | ((bytes[offset + 2] & 0xFF) << 8)
                | (bytes[offset + 3] & 0xFF));
        offset += 4;
        return value;
    }

    static public void WriteInt(int value, byte[] bytes, ref int offset)
    {
        bytes[offset] = (byte)(value >> 24 & 0xff);
        bytes[offset + 1] = (byte)(value >> 16 & 0xff);
        bytes[offset + 2] = (byte)(value >> 8 & 0xff);
        bytes[offset + 3] = (byte)(value & 0xff);
        offset += 4;
    }



    static public bool ReadBoolean(byte[] bytes, ref int offset)
    {
        byte b = bytes[offset];
        offset += 1;
        return b == (byte)0 ? false : true;
    }


    static public void WriteBoolean(bool value, byte[] bytes, ref int offset)
    {
        bytes[offset] = value ? ((byte)1) : ((byte)0);
        offset += 1;
    }
}
