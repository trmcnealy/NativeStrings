using System.Runtime.CompilerServices;

namespace System;

public sealed class RawStringData
{
    public int  Length;
    public char Data;


    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public static unsafe char* GetPointerToData(string? value)
    {
        if(value is null)
        {
            return null;
        }

        return (char*)Unsafe.AsPointer(ref Unsafe.As<RawStringData>(value).
                                                  Data);
    }
}
