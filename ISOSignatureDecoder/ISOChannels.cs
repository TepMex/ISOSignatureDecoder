using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISOSignatureDecoder
{
    class ISOChannels
    {
        private BitArray bitFlags;

        public bool X { get { return bitFlags.Get((int)ISOChannelType.X); } }
        public bool Y { get{ return bitFlags.Get((int)ISOChannelType.Y);} }
        public bool Z { get{ return bitFlags.Get((int)ISOChannelType.Z);} }
        public bool VX{ get{ return bitFlags.Get((int)ISOChannelType.VX); } }
        public bool VY{ get{ return bitFlags.Get((int)ISOChannelType.VY); } }
        public bool AX{ get{ return bitFlags.Get((int)ISOChannelType.AX); } }
        public bool AY{ get{ return bitFlags.Get((int)ISOChannelType.AY); } }
        public bool T { get{ return bitFlags.Get((int)ISOChannelType.T);} }
                             
        public bool DT{ get{ return bitFlags.Get((int)ISOChannelType.DT); } }
        public bool F { get{ return bitFlags.Get((int)ISOChannelType.F);} }
        public bool S { get{ return bitFlags.Get((int)ISOChannelType.S);} }
        public bool TX{ get{ return bitFlags.Get((int)ISOChannelType.TX); } }
        public bool TY{ get{ return bitFlags.Get((int)ISOChannelType.TY); } }
        public bool Az{ get{ return bitFlags.Get((int)ISOChannelType.Az); } }
        public bool E { get {return bitFlags.Get((int)ISOChannelType.E); } }
        public bool R { get { return bitFlags.Get((int)ISOChannelType.R); } }

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

        private List<ISOChannelType> channelsEnabled {
            get
            {
                List<ISOChannelType> result = new List<ISOChannelType>();
                for(int i = 0; i<bitFlags.Length; i++)
                {
                    if(bitFlags.Get(i))
                    {
                        result.Add((ISOChannelType)i);
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
            foreach(ISOChannelType ch in channelsEnabled)
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
