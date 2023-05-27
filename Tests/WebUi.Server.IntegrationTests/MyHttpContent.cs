using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CleanEjdg.Tests.WebUi.Server.IntegrationTests
{
    public class MyHttpContent : HttpContent
    {
        private readonly string _data;
        public MyHttpContent(string data)
        {
            _data = data;
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext? context)
        {
            return stream.WriteAsync(Encoding.UTF8.GetBytes(_data)).AsTask();
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }
    }
}
