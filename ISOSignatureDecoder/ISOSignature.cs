using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISOSignatureDecoder
{
    public class ISOSignature
    {

        private static byte[] SIGNATURE_HEADER_BYTES = { 83, 68, 73, 0 };
        private static int SIGNATURE_HEADER = BitConverter.ToInt32(SIGNATURE_HEADER_BYTES, 0);

        private string standardVersion;
        private ISOChannels channelsDescription;

        private List<ISOSignaturePoint> points = new List<ISOSignaturePoint>();

        public ISOSignature(byte[] signatureBinary)
        {
            if(!IsISOHeader(signatureBinary))
            {
                throw new ArgumentException("Provided byte array is not ISO compatible signature binary");
            }

            standardVersion = GetStandardVersion(signatureBinary);
            channelsDescription = new ISOChannels(signatureBinary);

            int idx = channelsDescription.LastByteIndex + 1;
            bool hasExtData = (signatureBinary[idx] & 0b10000000) > 0;

            if ((signatureBinary[idx] & 0b01111111) > 0)
            {
                throw new ArgumentException("Non ISO channel value header");
            }
            idx++;

            byte[] rawSampleSize = new byte[4] { signatureBinary[idx + 2], signatureBinary[idx + 1], signatureBinary[idx], 0 };
            int sampleCount = (int)BitConverter.ToUInt32(rawSampleSize, 0);
            idx += 3;

            if(!hasExtData && (signatureBinary.Length-idx) % sampleCount != 0)
            {
                throw new ArgumentException("ISO signature body has incorrect amount of samples or incorrect sample size");
            }

            byte[] samples = signatureBinary.Skip(idx).Take(signatureBinary.Length - idx).ToArray();

            points = ParsePoints(samples, sampleCount);

        }

        private List<ISOSignaturePoint> ParsePoints(byte[] samples, int sampleCount)
        {

            List<ISOSignaturePoint> result = new List<ISOSignaturePoint>();
            int sampleSize = samples.Length / sampleCount;
            for (int i = 0; i < samples.Length; i = i + sampleSize)
            {
                Dictionary<ISOChannelType, double> point = new Dictionary<ISOChannelType, double>();
                int idx = i;
                foreach(ISOChannelInfo ci in channelsDescription.ChannelInfo)
                {
                    ISOChannelType channelType = ci.ChannelType;
                    ByteSizeAttribute bsAttr = (ByteSizeAttribute)channelType.GetType().GetField(channelType.ToString())
                        .GetCustomAttributes(typeof(ByteSizeAttribute), false)[0];

                    int byteSize = bsAttr.ByteSize;

                    byte[] valueRaw = new byte[byteSize];
                    Array.Copy(samples, idx, valueRaw, 0, byteSize);
                    valueRaw = valueRaw.Reverse().ToArray();

                    double value;

                    switch (channelType)
                    {
                        case ISOChannelType.X:
                        case ISOChannelType.Y:
                        case ISOChannelType.VX:
                        case ISOChannelType.VY:
                        case ISOChannelType.AX:
                        case ISOChannelType.AY:
                        case ISOChannelType.TX:
                        case ISOChannelType.TY:
                            {
                                value = (int)BitConverter.ToUInt16(valueRaw, 0) - 32768;
                                break;
                            }

                        case ISOChannelType.Z:
                        case ISOChannelType.T:
                        case ISOChannelType.DT:
                        case ISOChannelType.F:
                        case ISOChannelType.Az:
                        case ISOChannelType.E:
                        case ISOChannelType.R:
                            {
                                value = (int)BitConverter.ToUInt16(valueRaw, 0);
                                break;
                            }
                        case ISOChannelType.S:
                            {
                                value = valueRaw[0] > 0 ? 1.0 : 0;
                                break;
                            }
                        default:
                            {
                                throw new Exception("Cannot parse ISO signature sample");
                            }
                    }

                    point[channelType] = value;

                    idx += byteSize;

                }

                result.Add(new ISOSignaturePoint(point));
            }

            return result;
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
