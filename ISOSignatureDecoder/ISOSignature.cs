using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureSDKTest.ISOSignatureDecoder
{
    public class ISOSignature
    {

        private static byte[] SIGNATURE_HEADER_BYTES = { 83, 68, 73, 0 };
        private static int SIGNATURE_HEADER = BitConverter.ToInt32(SIGNATURE_HEADER_BYTES, 0);

        private string standardVersion;
        private ISOChannels channelsEnabled;

        public ISOSignature(byte[] signatureBinary)
        {
            if(!IsISOHeader(signatureBinary))
            {
                throw new ArgumentException("Provided byte array is not ISO compatible signature binary");
            }

            standardVersion = GetStandardVersion(signatureBinary);
            channelsEnabled = new ISOChannels(signatureBinary);
        }

        private bool IsISOHeader(byte[] binary)
        {
            return BitConverter.ToInt32(binary, 0) == SIGNATURE_HEADER;
        }

        private string GetStandardVersion(byte[] binary)
        {
            return Encoding.ASCII.GetString(binary, 4, 4);
        }


    }
}
