using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SevenZip;
using SevenZip.Compression.LZMA;
using System.IO;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Diagnostics;
using GLuaDecode;

namespace GLuaDecode
{
    internal class Decode
    {
        public const int JUNK_SIZE = 4;
        public const int PROPERTIES_SIZE = 5;

        public bool doEncode(Stream inStream, Stream outStream)
        {
            try
            {
                Encoder encoder = new Encoder();
                encoder.WriteCoderProperties(outStream);

                long outSize = 0L;
                for (int index = 0; index < 8; ++index)
                {
                    int num = inStream.ReadByte();
                    if (num < 0)
                        return false;
                    outSize |= (long)(byte)num << 8 * index;
                }
                long inSize = inStream.Length - inStream.Position;
                encoder.Code(inStream, outStream, inSize, outSize, (ICodeProgress)null);

                byte[] fileLengthBytes = BitConverter.GetBytes((long)inStream.Length);
                outStream.Write(fileLengthBytes, 0, fileLengthBytes.Length);

                encoder.Code(inStream, outStream, inStream.Length, -1, null);
                outStream.Flush();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool doDecode(Stream inStream, Stream outStream, int junkSize = 4, bool removeJunk = false)
        {
            if (removeJunk)
                inStream.Seek((long)junkSize, SeekOrigin.Begin);
            byte[] numArray = new byte[5];
            if (inStream.Read(numArray, 0, 5) != 5)
                return false;
            Decoder decoder = new Decoder();
            decoder.SetDecoderProperties(numArray);
            long outSize = 0L;
            for (int index = 0; index < 8; ++index)
            {
                int num = inStream.ReadByte();
                if (num < 0)
                    return false;
                outSize |= (long)(byte)num << 8 * index;
            }
            long inSize = inStream.Length - inStream.Position;
            decoder.Code(inStream, outStream, inSize, outSize, (ICodeProgress)null);
            return true;
        }
    }
}
