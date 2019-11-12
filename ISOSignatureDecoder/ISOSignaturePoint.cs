using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureSDKTest.ISOSignatureDecoder
{
    class ISOSignaturePoint
    {
        private Dictionary<ISOChannelType, double> _point = new Dictionary<ISOChannelType, double>();

        public double? X { get { return GetValue(ISOChannelType.X); } }
        public double? Y { get { return GetValue(ISOChannelType.Y); } }
        public double? Z { get { return GetValue(ISOChannelType.Z); } }
        public double? VX { get { return GetValue(ISOChannelType.VX); } }
        public double? VY { get { return GetValue(ISOChannelType.VY); } }
        public double? AX { get { return GetValue(ISOChannelType.AX); } }
        public double? AY { get { return GetValue(ISOChannelType.AY); } }
        public double? T { get { return GetValue(ISOChannelType.T); } }

        public double? DT { get { return GetValue(ISOChannelType.DT); } }
        public double? F { get { return GetValue(ISOChannelType.F); } }
        public double? S { get { return GetValue(ISOChannelType.S); } }
        public double? TX { get { return GetValue(ISOChannelType.TX); } }
        public double? TY { get { return GetValue(ISOChannelType.TY); } }
        public double? Az { get { return GetValue(ISOChannelType.Az); } }
        public double? E { get { return GetValue(ISOChannelType.E); } }
        public double? R { get { return GetValue(ISOChannelType.R); } }

        public ISOSignaturePoint(Dictionary<ISOChannelType, double> point)
        {
            _point = point;
        }

        private double? GetValue(ISOChannelType type)
        {
            if(_point.ContainsKey(type))
            {
                return _point[type];
            }

            return null;
        }

    }
}
