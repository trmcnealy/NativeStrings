using System.Runtime.CompilerServices;

namespace System;

public sealed class Utf8CStringEqualityComparer : IEqualityComparer<utf8cstring>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Utf8CStringEqualityComparer()
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Equals(utf8cstring x,
                       utf8cstring y)
    {
        return x.Equals(y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public int GetHashCode(utf8cstring key)
    {
        unsafe
        {
            return key.Value == null ? 0 : key.GetHashCode();
        }
    }
}
