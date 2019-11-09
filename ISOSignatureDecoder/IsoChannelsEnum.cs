using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignatureSDKTest.ISOSignatureDecoder
{
    public enum IsoChannelsEnum : int
    {
        [Order(16)]
        X = 7,

        [Order(15)]
        Y = 6,

        [Order(14)]
        Z = 5,

        [Order(13)]
        VX = 4,

        [Order(12)]
        VY = 3,

        [Order(11)]
        AX = 2,

        [Order(10)]
        AY = 1,

        [Order(9)]
        T = 0,

        [Order(8)]
        DT = 15,

        [Order(7)]
        F = 14,

        [Order(6)]
        S = 13,

        [Order(5)]
        TX = 12,

        [Order(4)]
        TY = 11,

        [Order(3)]
        Az = 10,

        [Order(2)]
        E = 9,

        [Order(1)]
        R = 8
    }

    public class OrderAttribute : Attribute
    {
        public readonly int Order;

        public OrderAttribute(int order)
        {
            Order = order;
        }
    }
}
