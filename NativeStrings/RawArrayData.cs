namespace System;

public sealed class RawArrayData
{
    public uint Length;
#if !X86 || TARGET_64BIT
    public uint Padding;
#endif
    public byte Data;
}
