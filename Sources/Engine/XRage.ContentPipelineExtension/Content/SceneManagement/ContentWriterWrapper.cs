using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework;

namespace AISTek.XRage.Content.SceneManagement
{
    internal class ContentWriterWrapper : IContentWriterWrapper
    {
        public ContentWriterWrapper(ContentWriter writer)
        {
            this.writer = writer;
        }

        public void Write(bool value)
        {
            writer.Write(value);
        }

        public void Write(byte value)
        {
            writer.Write(value);
        }

        public void Write(byte[] buffer)
        {
            writer.Write(buffer);
        }

        public void Write(char ch)
        {
            writer.Write(ch);
        }

        public void Write(char[] chars)
        {
            writer.Write(chars);
        }

        public void Write(decimal value)
        {
            writer.Write(value);
        }

        public void Write(double value)
        {
            writer.Write(value);
        }

        public void Write(float value)
        {
            writer.Write(value);
        }

        public void Write(int value)
        {
            writer.Write(value);
        }

        public void Write(long value)
        {
            writer.Write(value);
        }

        public void Write(sbyte value)
        {
            writer.Write(value);
        }

        public void Write(short value)
        {
            writer.Write(value);
        }

        public void Write(string value)
        {
            writer.Write(value);
        }

        public void Write(uint value)
        {
            writer.Write(value);
        }

        public void Write(ulong value)
        {
            writer.Write(value);
        }

        public void Write(ushort value)
        {
            writer.Write(value);
        }

        public void Write(byte[] buffer, int index, int count)
        {
            writer.Write(buffer, index, count);
        }

        public void Write(char[] chars, int index, int count)
        {
            writer.Write(chars, index, count);
        }

        public void Write(Color value)
        {
            writer.Write(value);
        }

        public void Write(Matrix value)
        {
            writer.Write(value);
        }

        public void Write(Quaternion value)
        {
            writer.Write(value);
        }

        public void Write(Vector2 value)
        {
            writer.Write(value);
        }

        public void Write(Vector3 value)
        {
            writer.Write(value);
        }

        public void Write(Vector4 value)
        {
            writer.Write(value);
        }

        public void WriteObject<T>(T value)
        {
            writer.WriteObject(value);
        }

        public void WriteRawObject<T>(T value)
        {
            writer.WriteRawObject(value);
        }

        public void WriteSharedResource<T>(T value)
        {
            writer.WriteSharedResource(value);
        }

        private ContentWriter writer;
    }
}
