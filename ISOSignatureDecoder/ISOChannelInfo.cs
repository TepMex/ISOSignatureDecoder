using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureSDKTest.ISOSignatureDecoder
{
    public class ISOChannelInfo
    {
        private BitArray info;

        public IsoChannelsEnum ChannelType;

        public bool Scale { get { return info.Get(7); } }
        public bool MinValue { get { return info.Get(6); } }
        public bool MaxValue { get { return info.Get(5); } }
        public bool AvgValue { get { return info.Get(4); } }
        public bool Std { get { return info.Get(3); } }
        public bool ConstValue { get { return info.Get(2); } }
        public bool LinearComponentDeleted { get { return info.Get(1); } }

        public int Count
        {
            get
            {
                int acc = 0;
                foreach (bool b in info)
                {
                    if (b)
                    {
                        acc++;
                    }
                }
                return acc;
            }
        }

        public int FromIndex;
        public int ToIndex;

        public ISOChannelInfo(byte[] signatureBinary, int index, IsoChannelsEnum type)
        {
            ChannelType = type;
            FromIndex = index;
            info = new BitArray(new byte[1] { signatureBinary[index] });
            ToIndex = FromIndex + 2 * Convert.ToInt32(Scale) + 2 * Convert.ToInt32(MinValue) + 2 * Convert.ToInt32(MaxValue) + 2 * Convert.ToInt32(AvgValue) + 2 * Convert.ToInt32(Std);
        }
    }
}
