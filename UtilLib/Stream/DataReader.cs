using System.Buffers.Binary;
using UtilLib.Span;

namespace UtilLib.Stream
{
	public class DataReader : IDisposable
	{
		public long Position => reader.BaseStream.Position;

		public long Remaining => reader.BaseStream.Length - reader.BaseStream.Position;

		BinaryReader reader;
		bool invertEndianess;

		public DataReader(System.IO.Stream stream, bool isLittleEndian = false)
		{
			reader = new BinaryReader(stream);
			this.invertEndianess = isLittleEndian != BitConverter.IsLittleEndian; // If system and data endianess is mismatched invert endianess
		}

		public void Dispose()
		{
			reader.Dispose();
		}

		public ulong ReadUInt64()
			=> invertEndianess ? BinaryPrimitives.ReverseEndianness(reader.ReadUInt64()) : reader.ReadUInt64();

		public long ReadInt64()
			=> invertEndianess ? BinaryPrimitives.ReverseEndianness(reader.ReadInt64()) : reader.ReadInt64();

		public uint ReadUInt32()
			=> invertEndianess ? BinaryPrimitives.ReverseEndianness(reader.ReadUInt32()) : reader.ReadUInt32();

		public int ReadInt32()
			=> invertEndianess ? BinaryPrimitives.ReverseEndianness(reader.ReadInt32()) : reader.ReadInt32();

		public ushort ReadUInt16()
			=> invertEndianess ? BinaryPrimitives.ReverseEndianness(reader.ReadUInt16()) : reader.ReadUInt16();

		public short ReadInt16()
			=> invertEndianess ? BinaryPrimitives.ReverseEndianness(reader.ReadInt16()) : reader.ReadInt16();

		public Half ReadHalf()
			=> reader.ReadHalf();

		public float ReadFloat()
			=> reader.ReadSingle();

		public double ReadDouble()
			=> reader.ReadDouble();

		public byte ReadByte()
			=> reader.ReadByte();

		public char ReadChar()
			=> reader.ReadChar();

		public sbyte ReadSByte()
			=> reader.ReadSByte();

		public int Read(scoped Span<byte> buffer)
			=> reader.Read(buffer);

		public int Read(scoped Span<char> buffer)
			=> reader.Read(buffer);

		public int ReadUntill(scoped SpanList<byte> buffer, byte endByte)
		{
			byte c;
			int read = 0;

			do
			{
				c = reader.ReadByte();
				buffer.Add(c);
				read++;

			} while (c != endByte && !buffer.IsFull);

			return read;
		}

		public void Seek(long position)
		{
			reader.BaseStream.Position = position;
		}
	}
}
