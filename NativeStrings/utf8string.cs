#region

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace System
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct utf8string : IDisposable,
                                      IEquatable<utf8string>,
                                      IComparable<utf8string>,
                                      IComparable<string>
    {
        public static readonly utf8string                    Null             = new((string?)null);
        public static readonly IEqualityComparer<utf8string> EqualityComparer = new Utf8StringEqualityComparer();

        public sbyte* Value;

        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get
            {
                return GetLength(&Value[0]);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public utf8string(utf8string value)
        {
            int length = value.Length + 1;

            Value = (sbyte*)Marshal.AllocHGlobal(length);

            System.Buffer.MemoryCopy(value.Value,
                                     Value,
                                     length,
                                     length);

            GC.KeepAlive(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public utf8string(sbyte* value)
        {
            int length = GetLength(value) + 1;

            Value = (sbyte*)Marshal.AllocHGlobal(length);

            System.Buffer.MemoryCopy(value,
                                     Value,
                                     length,
                                     length);

            GC.KeepAlive(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public utf8string(string? value)
        {
            if(value == null)
            {
                Value = null;
                return;
            }

            if(value.Length == 0)
            {
                Value    = (sbyte*)Marshal.AllocHGlobal(1);
                Value[0] = 0;
                return;
            }

            if(value[^1] != char.MinValue)
            {
                value += char.MinValue;
            }

            Value = (sbyte*)Marshal.AllocHGlobal(value.Length);

            byte[] bytes = Encoding.ASCII.GetBytes(value);

            for(int i = 0; i < bytes.Length; i++)
            {
                Value[i] = (sbyte)bytes[i];
            }

            GC.KeepAlive(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public void Dispose()
        {
            DisposeUnmanaged();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        private void DisposeUnmanaged()
        {
            if(Value is not null)
            {
                Marshal.FreeHGlobal((IntPtr)Value);
            }
        }

        public sbyte this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get
            {
                return Value[index];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set
            {
                Value[index] = value;
            }
        }

        public sbyte this[uint index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            get
            {
                return Value[index];
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
            set
            {
                Value[index] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int GetLength(sbyte* str)
        {
            if(str is null || str[0] == char.MinValue)
            {
                return 0;
            }

            sbyte* value = str;

            int length = 0;

            while(*value++ != char.MinValue)
            {
                ++length;
            }

            return length;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public int CompareTo(utf8string other)
        {
            if(other.Value is null)
            {
                return 1;
            }

            return Compare(this,
                           other.Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public int CompareTo(sbyte* other)
        {
            if(other is null)
            {
                return 1;
            }

            return Compare(this,
                           other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public int CompareTo(string? other)
        {
            if(other is null)
            {
                return 1;
            }

            return Compare(this,
                           other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int Compare(utf8string lhs,
                                  utf8string rhs)
        {
            int ret = 0;

            byte* src = (byte*)lhs.Value;
            byte* dst = (byte*)rhs.Value;

            while((ret = *src - *dst) == 0 && *dst != 0)
            {
                ++src;
                ++dst;
            }

            return (-ret < 0 ? 1 : 0) - (ret < 0 ? 1 : 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int Compare(sbyte*     lhs,
                                  utf8string rhs)
        {
            int ret = 0;

            byte* src = (byte*)lhs;
            byte* dst = (byte*)rhs.Value;

            while((ret = *src - *dst) == 0 && *dst != 0)
            {
                ++src;
                ++dst;
            }

            return (-ret < 0 ? 1 : 0) - (ret < 0 ? 1 : 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int Compare(utf8string lhs,
                                  sbyte*     rhs)
        {
            int ret = 0;

            byte* src = (byte*)lhs.Value;
            byte* dst = (byte*)rhs;

            while((ret = *src - *dst) == 0 && *dst != 0)
            {
                ++src;
                ++dst;
            }

            return (-ret < 0 ? 1 : 0) - (ret < 0 ? 1 : 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static int Compare(utf8string lhs,
                                  string     rhs)
        {
            int ret = 0;

            byte[] asciiString = Encoding.ASCII.GetBytes(rhs);

            byte* src = (byte*)lhs.Value;
            byte* dst = (byte*)Unsafe.AsPointer(ref Unsafe.As<RawArrayData>(asciiString).
                                                           Data);
            ;

            while((ret = *src - *dst) == 0 && *dst != 0)
            {
                ++src;
                ++dst;
            }

            return (-ret < 0 ? 1 : 0) - (ret < 0 ? 1 : 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator sbyte*(utf8string value)
        {
            return value.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static implicit operator utf8string(string? value)
        {
            return new utf8string(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static explicit operator utf8string(sbyte* value)
        {
            return new utf8string(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override string? ToString()
        {
            return new string(Value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool Equals(string other)
        {
            if(Length != other.Length)
            {
                return false;
            }

            byte[] asciiString = Encoding.ASCII.GetBytes(other);

            byte* src = (byte*)Value;
            byte* dst = (byte*)Unsafe.AsPointer(ref Unsafe.As<RawArrayData>(asciiString).
                                                           Data);

            while(*dst != 0)
            {
                if(*src++ != *dst++)
                {
                    return false;
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool Equals(utf8string other)
        {
            if(Length != other.Length)
            {
                return false;
            }

            byte* src = (byte*)Value;
            byte* dst = (byte*)other.Value;

            while(*dst != 0)
            {
                if(*src++ != *dst++)
                {
                    return false;
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public bool Equals(sbyte* other)
        {
            if(Length != GetLength(other))
            {
                return false;
            }

            byte* src = (byte*)Value;
            byte* dst = (byte*)other;

            while(*dst != 0)
            {
                if(*src++ != *dst++)
                {
                    return false;
                }
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override bool Equals(object? obj)
        {
            return obj is utf8string other && Equals(other);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public override int GetHashCode()
        {
            sbyte* value = Value;

            HashCode code = new();
            code.Add(*value++);

            while(*value++ != char.MinValue)
            {
                code.Add(*value++);
            }

            return code.ToHashCode();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator ==(utf8string left,
                                       utf8string right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator !=(utf8string left,
                                       utf8string right)
        {
            return !left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator ==(utf8string left,
                                       string     right)
        {
            return left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator !=(utf8string left,
                                       string     right)
        {
            return !left.Equals(right);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator ==(string     left,
                                       utf8string right)
        {
            return right.Equals(left);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator !=(string     left,
                                       utf8string right)
        {
            return !right.Equals(left);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator ==(sbyte*     left,
                                       utf8string right)
        {
            return right.Equals(left);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
        public static bool operator !=(sbyte*     left,
                                       utf8string right)
        {
            return !right.Equals(left);
        }
    }


    //[StructLayout(LayoutKind.Sequential)]
    //public unsafe struct utf16string : IDisposable
    //{
    //    public char* Value;

    //    public int Length
    //    {
    //        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //        get
    //        {
    //            if (Value is null)
    //            {
    //                return 0;
    //            }

    //            int length = 0;

    //            char* value = &Value[0];

    //            while (*value++ != char.MinValue)
    //            {
    //                ++length;
    //            }

    //            return length;
    //        }
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public utf16string(char* value)
    //    {
    //        Value = value;

    //        GC.KeepAlive(this);
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public utf16string(string? value)
    //    {
    //        if (value == null)
    //        {
    //            Value = null;
    //            return;
    //        }

    //        if (value.Length == 0)
    //        {
    //            Value = (char*)Marshal.AllocHGlobal(1 * sizeof(char));
    //            Value[0] = char.MinValue;
    //            return;
    //        }

    //        if (value[^1] != char.MinValue)
    //        {
    //            value += char.MinValue;
    //        }

    //        Value = (char*)Marshal.AllocHGlobal(value.Length * sizeof(char));

    //        char[] bytes = value.ToCharArray();

    //        for (int i = 0; i < bytes.Length; i++)
    //        {
    //            Value[i] = bytes[i];
    //        }

    //        GC.KeepAlive(this);
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public void Dispose()
    //    {
    //        DisposeUnmanaged();
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    private void DisposeUnmanaged()
    //    {
    //        if (Value is not null)
    //        {
    //            Marshal.FreeHGlobal((IntPtr)Value);
    //        }
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public static implicit operator char*(utf16string value)
    //    {
    //        return value.Value;
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public static implicit operator utf16string(string? value)
    //    {
    //        return new utf16string(value);
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public static explicit operator utf16string(char* value)
    //    {
    //        return new utf16string(value);
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public override string? ToString()
    //    {
    //        return new string(Value);
    //    }
    //}


    //[StructLayout(LayoutKind.Sequential)]
    //public readonly unsafe struct utf16cstring : IDisposable
    //{
    //    public readonly char* Value;

    //    public int Length
    //    {
    //        [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //        get
    //        {
    //            if (Value is null)
    //            {
    //                return 0;
    //            }

    //            int length = 0;

    //            char* value = &Value[0];

    //            while (*value++ != char.MinValue)
    //            {
    //                ++length;
    //            }

    //            return length;
    //        }
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public utf16cstring(char* value)
    //    {
    //        Value = value;

    //        GC.KeepAlive(this);
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public utf16cstring(string? value)
    //    {
    //        if (value == null)
    //        {
    //            Value = null;
    //            return;
    //        }

    //        if (value.Length == 0)
    //        {
    //            Value = (char*)Marshal.AllocHGlobal(1 * sizeof(char));
    //            Value[0] = char.MinValue;
    //            return;
    //        }

    //        if (value[^1] != char.MinValue)
    //        {
    //            value += char.MinValue;
    //        }

    //        Value = (char*)Marshal.AllocHGlobal(value.Length * sizeof(char));

    //        char[] bytes = value.ToCharArray();

    //        for (int i = 0; i < bytes.Length; i++)
    //        {
    //            Value[i] = bytes[i];
    //        }

    //        GC.KeepAlive(this);
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public void Dispose()
    //    {
    //        DisposeUnmanaged();
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    private void DisposeUnmanaged()
    //    {
    //        if (Value is not null)
    //        {
    //            Marshal.FreeHGlobal((IntPtr)Value);
    //        }
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public static implicit operator char*(utf16cstring value)
    //    {
    //        return value.Value;
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public static implicit operator utf16cstring(utf16string value)
    //    {
    //        return new utf16cstring(value.Value);
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public static implicit operator utf16cstring(string? value)
    //    {
    //        return new utf16cstring(value);
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public static explicit operator utf16cstring(char* value)
    //    {
    //        return new utf16cstring(value);
    //    }

    //    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    //    public override string? ToString()
    //    {
    //        return new string(Value);
    //    }
    //}
}
