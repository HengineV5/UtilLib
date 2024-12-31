using System.Buffers.Binary;
using System.Reflection.PortableExecutable;

namespace UtilLib.Stream
{
	public class DataWriter : IDisposable
	{
		public long Position => writer.BaseStream.Position;
		public long Remaining => writer.BaseStream.Length - writer.BaseStream.Position;

		BinaryWriter writer;
		bool invertEndianess;

		public DataWriter(System.IO.Stream stream, bool isLittleEndian = false)
		{
			writer = new BinaryWriter(stream);
			this.invertEndianess = isLittleEndian != BitConverter.IsLittleEndian; // If system and data endianess is mismatched invert endianess
		}

		public void Dispose()
		{
			writer.Dispose();
		}

		public void Write(ulong value)
		 => writer.Write(invertEndianess ? BinaryPrimitives.ReverseEndianness(value) : value);

		public void Write(long value)
		 => writer.Write(invertEndianess ? BinaryPrimitives.ReverseEndianness(value) : value);

		public void Write(uint value)
		 => writer.Write(invertEndianess ? BinaryPrimitives.ReverseEndianness(value) : value);

		public void Write(int value)
		 => writer.Write(invertEndianess ? BinaryPrimitives.ReverseEndianness(value) : value);

		public void Write(ushort value)
		 => writer.Write(invertEndianess ? BinaryPrimitives.ReverseEndianness(value) : value);

		public void Write(short value)
		 => writer.Write(invertEndianess ? BinaryPrimitives.ReverseEndianness(value) : value);

		public void Write(float value)
		 => writer.Write(value);

		public void Write(byte value)
		 => writer.Write(value);

		public void Write(sbyte value)
		 => writer.Write(value);

		public void Write(char value)
		 => writer.Write(value);

		public void Write(scoped ReadOnlySpan<byte> buffer)
		 => writer.Write(buffer);

		public void Write(scoped ReadOnlySpan<char> buffer)
		 => writer.Write(buffer);

		public void Seek(long position)
		{
			writer.BaseStream.Position = position;
		}
	}
}
