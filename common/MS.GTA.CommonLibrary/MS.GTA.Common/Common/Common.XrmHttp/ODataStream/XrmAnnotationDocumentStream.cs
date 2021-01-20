using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MS.GTA.Common.XrmHttp.ODataStream
{
    public class XrmAnnotationDocumentStream : Stream
    {
        private static readonly byte[] StartSearchPattern = Encoding.UTF8.GetBytes("\"documentbody\":\"");
        private static readonly byte[] EndSearchPattern = Encoding.UTF8.GetBytes("\"");

        private byte[] tempBuffer;

        private int tempBufferOffset;
        private int tempBufferFilled;

        private StreamState streamState;

        public XrmAnnotationDocumentStream(Stream innerStream, int bufferSize = 2048)
        {
            int size = Math.Max(bufferSize, StartSearchPattern.Length + 1);

            this.InnerStream = innerStream;
            this.streamState = StreamState.Begin;
            this.tempBuffer = new byte[size];
            this.tempBufferOffset = 0;
            this.tempBufferFilled = 0;
        }

        private enum StreamState
        {
            Begin,
            Middle,
            End
        }

        public Stream InnerStream { get; }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        #region Not Implemented
        public override long Length => throw new NotImplementedException("XrmAnnotationDocumentStream.Length");

        public override long Position { get => throw new NotImplementedException("XrmAnnotationDocumentStream.Position get"); set => throw new NotImplementedException("XrmAnnotationDocumentStream.Position set"); }


        public override void Flush()
        {
            throw new NotImplementedException("XrmAnnotationDocumentStream.Flush");
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException("XrmAnnotationDocumentStream.Seek");
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException("XrmAnnotationDocumentStream.SetLength");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException("XrmAnnotationDocumentStream.Write");
        }
        #endregion Not Implemented

        public override int Read(byte[] outputBuffer, int outputOffset, int count)
        {
            if (this.FillTempBufferIfExhausted())
            {
                // If at the start of the stream, find the start pattern and skip past it.
                if (this.streamState == StreamState.Begin)
                {
                    if (this.FindAndSetIndexOfStart() != -1)
                    {
                        this.streamState = StreamState.Middle;
                    }
                    else
                    {
                        throw new InvalidDataException("Unable to find start of Annotation document body from stream.");
                    }
                }

                this.FillTempBufferIfExhausted();

                // If at the middle of the stream, search for the end pattern and truncate before it if found.
                if (this.streamState == StreamState.Middle)
                {
                    // Filled the buffer, now find the end in buffer if present.
                    if (this.FindAndSetIndexOfEnd() != -1)
                    {
                        this.streamState = StreamState.End;
                    }
                }
            }

            // Read the bytes requested.
            var amountToCopy = Math.Min(count, this.tempBufferFilled - this.tempBufferOffset);
            Array.Copy(this.tempBuffer, this.tempBufferOffset, outputBuffer, outputOffset, amountToCopy);
            this.tempBufferOffset += amountToCopy;

            return amountToCopy;
        }

        private static int SearchForPattern(byte[] searchPattern, byte[] bufferToSearch, int bufferToSearchStart, int bufferToSearchEnd)
        {
            var indexOf = -1;
            for (var i = bufferToSearchStart; i < bufferToSearchEnd - searchPattern.Length + 1; i++)
            {
                var found = true;
                for (var j = 0; j < searchPattern.Length; j++)
                {
                    if (searchPattern[j] != bufferToSearch[i + j])
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    indexOf = i;
                    break;
                }
            }

            return indexOf;
        }

        private static int FillBuffer(Stream stream, byte[] buffer, int filled)
        {
            var innerBytesRead = -1;

            // ensures we get tempBuffer size bytes or the entire buffer
            while (filled < buffer.Length && innerBytesRead != 0)
            {
                innerBytesRead = stream.Read(buffer, filled, buffer.Length - filled);
                filled += innerBytesRead;
            }

            return filled;
        }

        /// <summary>
        /// Step through the stream until the wanted start has been found. Sets the <c>tempBufferOffset</c>.
        /// </summary>
        /// <returns></returns>
        private int FindAndSetIndexOfStart()
        {
            do
            {
                var indexOfStart = SearchForPattern(StartSearchPattern, this.tempBuffer, 0, this.tempBufferFilled);
                if (indexOfStart != -1)
                {
                    this.tempBufferOffset = indexOfStart + StartSearchPattern.Length;
                    return indexOfStart;
                }

                // Start was not found, advance the offset to just before the end of the buffer and then refill the buffer with the next block of data before searching for the start again.
                this.tempBufferOffset = this.tempBufferFilled - StartSearchPattern.Length;
            }
            while (this.FillTempBufferIfExhausted());

            return -1;
        }

        /// <summary>
        /// Scan through the current buffer to search for the wanted end. Sets the <c>tempBufferFilled</c>.
        /// </summary>
        /// <returns></returns>
        private int FindAndSetIndexOfEnd()
        {
            var indexOfEnd = SearchForPattern(EndSearchPattern, this.tempBuffer, this.tempBufferOffset, this.tempBufferFilled);
            if (indexOfEnd != -1)
            {
                this.tempBufferFilled = indexOfEnd;
                return indexOfEnd;
            }

            return -1;
        }

        private bool FillTempBufferIfExhausted()
        {
            // Minimum block size is the byte pattern for the start search pattern.
            if (this.streamState != StreamState.End && this.tempBufferFilled - this.tempBufferOffset <= StartSearchPattern.Length)
            {
                // Shift existing data smaller than the block size to the start for the temp buffer.
                Array.Copy(this.tempBuffer, this.tempBufferOffset, this.tempBuffer, 0, this.tempBufferFilled - this.tempBufferOffset);
                this.tempBufferFilled -= this.tempBufferOffset;
                this.tempBufferOffset = 0;

                // Fill the rest of the temp buffer with more data, if possible.
                var updatedTempBufferFilled = FillBuffer(this.InnerStream, this.tempBuffer, this.tempBufferFilled);

                // If the amount has not changed then no additional data was loaded.
                if (updatedTempBufferFilled != this.tempBufferFilled)
                {
                    this.tempBufferFilled = updatedTempBufferFilled;
                    return true;
                }
            }

            return false;
        }
    }
}
