using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using UnityEngine;

namespace FixPhysics
{
    public struct FixNum
    {
        /// <summary> 小数点定点位:不能大于32 </summary>
        private const int BITS = 16;
        /// <summary> 小数部分掩码 </summary>
        private const int MASK = (1 << BITS) - 1;
        /// <summary> 可记录的最大值 </summary>
        public static float MaxValue
        {
            get
            {
                long v = 1; // 1默认使用int会数据溢出
                v <<= (sizeof(long) * 8 - BITS - 1);
                return (float)v - 1 + ((1 << BITS) - 1) / (float)(1 << BITS);   // 位数小于32不做long处理
            }
        }
        #region 本类&本类运算符重载
        public static FixNum operator +(FixNum a, FixNum b) => new FixNum(a.Value + b.Value);
        public static FixNum operator -(FixNum a, FixNum b) => new FixNum(a.Value - b.Value);
        public static FixNum operator *(FixNum a, FixNum b) => new FixNum(a.Value * b.Value);
        public static FixNum operator /(FixNum a, FixNum b) => new FixNum(a.Value / b.Value);
        #endregion 本类&本类运算符重载

        #region 本类&他类运算符重载
        #endregion 本类&他类运算符重载

        // int的隐式转换可以取消,因为在某些API中int会优先于float被转换,导致使用int运算方式。
        // 如随机数
        #region 隐式转换
        // 隐式转换：float 与 FixFloat
        public static implicit operator FixNum(float number) => new FixNum(number);
        public static implicit operator float(FixNum number) => number.Value;
        public static implicit operator FixNum(int number) => new FixNum(number);
        public static implicit operator int(FixNum number) => (int)number.Value;
        #endregion 隐式转换

        /// <summary> 浮点数转定点数(rawValue原始数) </summary>
        private static long FloatToRawValue(float number) => (long)(number * (1 << BITS));
        /// <summary> 双精度浮点数转定点数(rawValue原始数) </summary>
        private static long DoubleToRawValue(double number) => (long)(number * (1 << BITS));
        /// <summary> 定点数(rawValue原始数)转浮点数 </summary>
        private static float RawValueToFloat(FixNum ff) => (float)ff.rawValue / (1 << BITS);

        public readonly long rawValue;

        public readonly float Value => RawValueToFloat(this);

        public FixNum(float number) => rawValue = FloatToRawValue(number);
        public FixNum(double number) => rawValue = DoubleToRawValue(number);
        public FixNum(int value) : this((long)value) { }
        public FixNum(long value) => this.rawValue = value << 32 - BITS;

        /// <summary> 小数部分 </summary>
        public float FractionalPart() => (float)(rawValue & MASK) / (1 << BITS);

        public override string ToString() => Value.ToString();
    }
}
