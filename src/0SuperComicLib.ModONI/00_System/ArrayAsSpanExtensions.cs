// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace System;

public static class ArrayAsSpanExtensions
{
    extension<T>(T[] array)
    {
        // easy convert as 'readonlyspan'
        public ReadOnlySpan<T> AsReadOnlySpan() => array.AsSpan();
    }
}
