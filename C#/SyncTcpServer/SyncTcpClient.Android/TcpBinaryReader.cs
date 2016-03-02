using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;

namespace SyncTcpClient
{
    public class TcpBinaryReader : BinaryReader
    {
        private Stream m_stream;
        private byte[] m_charBytes;
        private char[] m_charBuffer;
        private int m_maxCharsSize;
        private Decoder m_decoder;

        private byte[] m_buffer;
        private bool m_2BytesPerChar;
        private bool m_isMemoryStream;

        public TcpBinaryReader(Stream input)
            : this(input, Encoding.Default)
        {
        }

        public TcpBinaryReader(Stream input, Encoding encoding)
            : base(input, encoding)
        {
            m_stream = input;
            this.m_decoder = encoding.GetDecoder();
            this.m_maxCharsSize = encoding.GetMaxCharCount(0x80);
            int maxByteCount = encoding.GetMaxByteCount(1);
            if (maxByteCount < 0x10)
            {
                maxByteCount = 0x10;
            }

            this.m_buffer = new byte[maxByteCount];
            this.m_2BytesPerChar = encoding is UnicodeEncoding;
            this.m_isMemoryStream = this.m_stream.GetType() == typeof(MemoryStream);
        }

        new public string ReadString()
        {
            if (m_stream == null)
            {
                throw new ObjectDisposedException(null, "ObjectDisposed_FileClosed");
            }

            int num = 0;
            int capacity = base.Read7BitEncodedInt();
            if (capacity < 0)
            {
                throw new IOException("IO.IO_InvalidStringLen_Len");
            }

            if (capacity == 0)
            {
                return string.Empty;
            }

            if (this.m_charBytes == null)
            {
                this.m_charBytes = new byte[0x80];
            }
            if (this.m_charBuffer == null)
            {
                this.m_charBuffer = new char[this.m_maxCharsSize];
            }
            StringBuilder sb = null;
            do
            {
                int count = ((capacity - num) > 0x80) ? 0x80 : (capacity - num);
                int byteCount = this.m_stream.Read(this.m_charBytes, 0, count);
                if (byteCount == 0)
                {
                    throw new EndOfStreamException("IO.EOF_ReadBeyondEOF");

                }
                int length = this.m_decoder.GetChars(this.m_charBytes, 0, byteCount, this.m_charBuffer, 0);
                if ((num == 0) && (byteCount == capacity))
                {
                    return new string(this.m_charBuffer, 0, length);
                }
                if (sb == null)
                {
                    sb = StringBuilderCache.Acquire(capacity);
                }
                sb.Append(this.m_charBuffer, 0, length);
                num += byteCount;
            }
            while (num < capacity);
            return StringBuilderCache.GetStringAndRelease(sb);

        }
    }

    internal static class StringBuilderCache
    {
        // Fields
        [ThreadStatic]
        private static StringBuilder CachedInstance;
        private const int MAX_BUILDER_SIZE = 360;

        // Methods
        public static StringBuilder Acquire(int capacity = 0x10)
        {
            if (capacity <= 360)
            {
                StringBuilder cachedInstance = CachedInstance;
                if ((cachedInstance != null) && (capacity <= cachedInstance.Capacity))
                {
                    CachedInstance = null;
                    cachedInstance.Clear();
                    return cachedInstance;
                }
            }
            return new StringBuilder(capacity);
        }


        public static string GetStringAndRelease(StringBuilder sb)
        {
            string str = sb.ToString();
            Release(sb);
            return str;            
        }

        public static void Release(StringBuilder sb)
        {
            if (sb.Capacity <= 360)
            {
                CachedInstance = sb;
            }

        }
    }

 

}