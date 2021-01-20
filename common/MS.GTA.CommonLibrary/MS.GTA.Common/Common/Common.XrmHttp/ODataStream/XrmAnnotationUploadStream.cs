// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.XrmHttp.ODataStream
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    class XrmAnnotationUploadStream : Stream
    {
        Queue<Stream> streamList;

        public XrmAnnotationUploadStream(IEnumerable<Stream> streamList)
        {
            this.streamList = new Queue<Stream>(streamList);
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (streamList.Count == 0)
            {
                return 0;
            }

            int bytesRead = streamList.Peek().Read(buffer, offset, count);
            if (bytesRead == 0)
            {
                streamList.Dequeue().Dispose();
                bytesRead = Read(buffer, offset, count);
            }
            return bytesRead;
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void Flush()
        {
            throw new NotImplementedException("XrmAnnotationUploadStream.Flush");
        }

        public override long Length
        {
            get { throw new NotImplementedException("XrmAnnotationUploadStream.Length"); }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException("XrmAnnotationUploadStream.Position get");
            }
            set
            {
                throw new NotImplementedException("XrmAnnotationUploadStream.Position set");
            }
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException("XrmAnnotationUploadStream.Seek");
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException("XrmAnnotationUploadStream.SetLength");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException("XrmAnnotationUploadStream.Write");
        }
    }
}
