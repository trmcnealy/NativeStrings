using System.Runtime.CompilerServices;

namespace System;

public sealed class Utf8StringEqualityComparer : IEqualityComparer<utf8string>
{
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public Utf8StringEqualityComparer()
    {
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public bool Equals(utf8string x,
                       utf8string y)
    {
        return x.Equals(y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public int GetHashCode(utf8string key)
    {
        unsafe
        {
            return key.Value == null ? 0 : key.GetHashCode();
        }
    }
}
