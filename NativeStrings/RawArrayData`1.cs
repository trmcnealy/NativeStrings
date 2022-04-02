namespace System;

public sealed class RawArrayData<T>
    where T : unmanaged
{
    public uint Length;
#if !X86 || TARGET_64BIT
    public uint Padding;
#endif
    public unsafe T* Data;
}
