using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Content
{
    public interface IContentWriterWrapper
    {
        void Write(bool value);

        void Write(byte value);

        void Write(byte[] buffer);

        void Write(char ch);

        void Write(char[] chars);

        void Write(decimal value);

        void Write(double value);

        void Write(float value);

        void Write(int value);

        void Write(long value);

        void Write(sbyte value);

        void Write(short value);

        void Write(string value);

        void Write(uint value);

        void Write(ulong value);

        void Write(ushort value);

        void Write(byte[] buffer, int index, int count);

        void Write(char[] chars, int index, int count);

        void Write(Color value);

        void Write(Matrix value);

        void Write(Quaternion value);

        void Write(Vector2 value);

        void Write(Vector3 value);

        void Write(Vector4 value);

        void WriteObject<T>(T value);

        void WriteRawObject<T>(T value);

        void WriteSharedResource<T>(T value);
    }
}
