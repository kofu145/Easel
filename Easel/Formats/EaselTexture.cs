using System;
using System.IO;
using Easel.Math;
using Easel.Utilities;
using Pie;

namespace Easel.Formats;

public struct EaselTexture
{
    public TextureType Type;
    
    public Size Size;

    public Bitmap[] Cubemap;

    public EaselTexture(Bitmap[] cubemap)
    {
        Type = TextureType.Cubemap;
        Size = cubemap[0].Size;
        Cubemap = cubemap;
    }

    public static EaselTexture Deserialize(byte[] data)
    {
        using MemoryStream stream = new MemoryStream(data);
        using BinaryReader reader = new BinaryReader(stream);

        if (new string(reader.ReadChars(4)) != "ETF ")
            throw new EaselException("Given file is not an Easel texture.");

        reader.ReadUInt16(); // Version

        if (reader.ReadByte() == 1)
            throw new EaselException("Compressed files not supported.");

        TextureType type = (TextureType) reader.ReadByte();

        Size size = new Size(reader.ReadInt32(), reader.ReadInt32());

        switch (type)
        {
            case TextureType.Bitmap:
                break;
            
            case TextureType.Cubemap:
                PixelFormat format = (PixelFormat) reader.ReadByte();

                byte length = reader.ReadByte();

                Bitmap[] bitmaps = new Bitmap[length];
                
                for (int i = 0; i < length; i++)
                {
                    if (new string(reader.ReadChars(4)) != "TEXC")
                        throw new EaselException("\"TEXC\" expected, was not found.");

                    int dataLength = reader.ReadInt32();
                    byte[] cData = reader.ReadBytes(dataLength);
                    bitmaps[i] = new Bitmap(size.Width, size.Height, format, cData);
                }

                return new EaselTexture(bitmaps);
            
            default:
                throw new ArgumentOutOfRangeException();
        }

        return default;
    }

    public byte[] Serialize(bool compress)
    {
        if (compress)
            throw new EaselException("Compression not supported.");
        
        const ushort version = 1;

        using MemoryStream stream = new MemoryStream();
        using BinaryWriter writer = new BinaryWriter(stream);
        
        // HEADER
        
        writer.Write("ETF ".ToCharArray());
        writer.Write(version);
        writer.Write((byte) (compress ? 1 : 0));
        writer.Write((byte) Type);
        writer.Write(Size.Width);
        writer.Write(Size.Height);

        switch (Type)
        {
            case TextureType.Bitmap:
                break;
            case TextureType.Cubemap:
                writer.Write((byte) Cubemap[0].Format);
                
                writer.Write((byte) Cubemap.Length);
                for (int i = 0; i < Cubemap.Length; i++)
                {
                    writer.Write("TEXC".ToCharArray());
                    writer.Write(Cubemap[i].Data.Length);
                    writer.Write(Cubemap[i].Data);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        
        return stream.ToArray();
    }

    public enum TextureType : byte
    {
        Bitmap,
        Cubemap
    }
}