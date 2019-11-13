using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISOSignatureDecoder
{
    public class ISOChannelInfo
    {
        private BitArray info;

        public ISOChannelType ChannelType;

        public bool HasScale { get { return info.Get(7); } }
        public bool HasMinValue { get { return info.Get(6); } }
        public bool HasMaxValue { get { return info.Get(5); } }
        public bool HasAvgValue { get { return info.Get(4); } }
        public bool HasStd { get { return info.Get(3); } }
        public bool HasConstValue { get { return info.Get(2); } }
        public bool HasLinearComponentDeleted { get { return info.Get(1); } }

        public double Scale;
        public double MinValue;
        public double MaxValue;
        public double Avg;
        public double Std;
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

        public ISOChannelInfo(byte[] signatureBinary, int index, ISOChannelType type)
        {
            ChannelType = type;
            FromIndex = index;
            info = new BitArray(new byte[1] { signatureBinary[index] });
            ToIndex = FromIndex + 1 + 2 * Convert.ToInt32(HasScale) + 2 * Convert.ToInt32(HasMinValue) + 2 * Convert.ToInt32(HasMaxValue) + 2 * Convert.ToInt32(HasAvgValue) + 2 * Convert.ToInt32(HasStd);

            int descBytes = index + 1;
            if(HasScale)
            {
                byte[] raw = new byte[2] { signatureBinary[descBytes], signatureBinary[descBytes + 1] };
                BitArray mantissMask = new BitArray(new byte[2] { 0b00000111, 0b11111111 });
                BitArray exponentMask = new BitArray(new byte[2] { 0b11111000, 0b00000000 });
                BitArray mantissBits = new BitArray(raw);
                BitArray exponentBits = new BitArray(raw);

                exponentBits = exponentBits.And(exponentMask);
                int[] arr = new int[1];
                exponentBits.CopyTo(arr, 0);
                int exponent = (arr[0] >> 3) - 16;

                mantissBits = mantissBits.And(mantissMask);
                byte[] byteArr = new byte[2];
                mantissBits.CopyTo(byteArr, 0);
                int mantiss = (int)BitConverter.ToUInt16(byteArr.Reverse().ToArray(), 0);

                Scale = Math.Pow(1 + mantiss / Math.Pow(2, 11), (double)exponent - 16);
                descBytes += 2;
            }

            if(HasMinValue)
            {
                byte[] raw = new byte[2] { signatureBinary[descBytes+1], signatureBinary[descBytes] };
                switch(ChannelType)
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
                            MinValue = (double)((int)BitConverter.ToUInt16(raw, 0) - 32768);
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
                            MinValue = (double)BitConverter.ToUInt16(raw, 0);
                            break;
                        }
                }
                
                descBytes += 2;
            }

            if (HasMaxValue)
            {
                byte[] raw = new byte[2] { signatureBinary[descBytes+1], signatureBinary[descBytes] };
                switch (ChannelType)
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
                            MaxValue = (double)((int)BitConverter.ToUInt16(raw, 0) - 32768);
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
                            MaxValue = (double)BitConverter.ToUInt16(raw, 0);
                            break;
                        }
                }
                descBytes += 2;
            }

            if(HasAvgValue)
            {
                byte[] raw = new byte[2] { signatureBinary[descBytes+1], signatureBinary[descBytes] };
                switch (ChannelType)
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
                            Avg = (int)BitConverter.ToUInt16(raw, 0) - 32768;
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
                            Avg = (int)BitConverter.ToUInt16(raw, 0);
                            break;
                        }
                }
                Console.WriteLine(Avg/Scale);
                descBytes += 2;
            }

            if (HasStd)
            {
                byte[] raw = new byte[2] { signatureBinary[descBytes+1], signatureBinary[descBytes] };
                switch (ChannelType)
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
                            Std = (int)BitConverter.ToUInt16(raw, 0) - 32768;
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
                            Std = (int)BitConverter.ToUInt16(raw, 0);
                            break;
                        }
                }
                descBytes += 2;
            }

            if(descBytes != ToIndex)
            {
                throw new ArgumentException("Incorrect channel description");
            }

        }
    }
}
