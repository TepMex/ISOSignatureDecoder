using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureSDKTest.ISOSignatureDecoder
{
    class ISOChannels
    {
        private BitArray bitFlags;

        public bool X { get { return bitFlags.Get((int)IsoChannelsEnum.X); } }
        public bool Y { get{ return bitFlags.Get((int)IsoChannelsEnum.Y);} }
        public bool Z { get{ return bitFlags.Get((int)IsoChannelsEnum.Z);} }
        public bool VX{ get{ return bitFlags.Get((int)IsoChannelsEnum.VX); } }
        public bool VY{ get{ return bitFlags.Get((int)IsoChannelsEnum.VY); } }
        public bool AX{ get{ return bitFlags.Get((int)IsoChannelsEnum.AX); } }
        public bool AY{ get{ return bitFlags.Get((int)IsoChannelsEnum.AY); } }
        public bool T { get{ return bitFlags.Get((int)IsoChannelsEnum.T);} }
                             
        public bool DT{ get{ return bitFlags.Get((int)IsoChannelsEnum.DT); } }
        public bool F { get{ return bitFlags.Get((int)IsoChannelsEnum.F);} }
        public bool S { get{ return bitFlags.Get((int)IsoChannelsEnum.S);} }
        public bool TX{ get{ return bitFlags.Get((int)IsoChannelsEnum.TX); } }
        public bool TY{ get{ return bitFlags.Get((int)IsoChannelsEnum.TY); } }
        public bool Az{ get{ return bitFlags.Get((int)IsoChannelsEnum.Az); } }
        public bool E { get {return bitFlags.Get((int)IsoChannelsEnum.E); } }
        public bool R { get { return bitFlags.Get((int)IsoChannelsEnum.R); } }

        public int LastByteIndex = 0;

        public int Count
        {
            get
            {
                int acc = 0;
                foreach(bool b in bitFlags)
                {
                    if(b)
                    {
                        acc++;
                    }
                }
                return acc;
            }
        }

        private List<IsoChannelsEnum> channelsEnabled {
            get
            {
                List<IsoChannelsEnum> result = new List<IsoChannelsEnum>();
                for(int i = 0; i<bitFlags.Length; i++)
                {
                    if(bitFlags.Get(i))
                    {
                        result.Add((IsoChannelsEnum)i);
                    }
                }

                result.Sort((a, b) =>
                {
                    OrderAttribute aAttr = (OrderAttribute)a.GetType().GetField(a.ToString())
                        .GetCustomAttributes(typeof(OrderAttribute), false)[0];

                    OrderAttribute bAttr = (OrderAttribute)b.GetType().GetField(b.ToString())
                        .GetCustomAttributes(typeof(OrderAttribute), false)[0];
                    return bAttr.Order - aAttr.Order;
                });

                return result;
            }
        }

        public List<ISOChannelInfo> ChannelInfo = new List<ISOChannelInfo>();

        public ISOChannels(byte[] binary)
        {
            bitFlags = new BitArray(new byte[2] { binary[8], binary[9] });

            int channelNum = 10;
            foreach(IsoChannelsEnum ch in channelsEnabled)
            {
                ISOChannelInfo ci = new ISOChannelInfo(binary, channelNum, ch);
                ChannelInfo.Add(ci);
                channelNum = ci.ToIndex;

            }

            if(binary[channelNum] != 0)
            {
                throw new ArgumentException("Incorrect binary header - non zero byte at the end of sequence");
            }

            LastByteIndex = channelNum;
        }

    }
}
