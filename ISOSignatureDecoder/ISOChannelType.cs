using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISOSignatureDecoder
{
    public enum ISOChannelType : int
    {
        [ByteSize(2)]
        [Order(16)]
        X = 7,

        [ByteSize(2)]
        [Order(15)]
        Y = 6,

        [ByteSize(2)]
        [Order(14)]
        Z = 5,

        [ByteSize(2)]
        [Order(13)]
        VX = 4,

        [ByteSize(2)]
        [Order(12)]
        VY = 3,

        [ByteSize(2)]
        [Order(11)]
        AX = 2,

        [ByteSize(2)]
        [Order(10)]
        AY = 1,

        [ByteSize(2)]
        [Order(9)]
        T = 0,

        [ByteSize(2)]
        [Order(8)]
        DT = 15,

        [ByteSize(2)]
        [Order(7)]
        F = 14,

        [ByteSize(1)]
        [Order(6)]
        S = 13,

        [ByteSize(2)]
        [Order(5)]
        TX = 12,

        [ByteSize(2)]
        [Order(4)]
        TY = 11,

        [ByteSize(2)]
        [Order(3)]
        Az = 10,

        [ByteSize(2)]
        [Order(2)]
        E = 9,

        [ByteSize(2)]
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


    public class ByteSizeAttribute : Attribute
    {
        public readonly int ByteSize;

        public ByteSizeAttribute(int size)
        {
            ByteSize = size;
        }
    }
}
