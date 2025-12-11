// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

global using FuncTargetDict = System.Collections.Generic.Dictionary<string, SuperComicLib.YamlXX.VFuncRStr>;
global using MappedStringDict = System.Collections.Generic.Dictionary<string, string>;

namespace SuperComicLib.YamlXX;

internal static class LiteralHelpers
{
    private const string DefaultParsingFailedMessage = "!!<YAMLXX.PARSING.ERROR>";

    internal static string GetString(ReadOnlySpan<char> expression) => expression.ToString();
    
    internal static unsafe string Expression(
        ref ReadOnlySpan<char> input,
        FuncTargetDict target,
        MappedStringDict strmap,
        [DisallowNull, NotNull] YamlXXParseError* errno)
    {
        Debug.Assert(errno != null);

        var __retVal = DefaultParsingFailedMessage;
        var funcName = ReadToken(ref input);

        // 함수 이름 취득 실패함
        if (funcName.Length <= 0)
        {
            *errno = YamlXXParseError.ExprActionNameInvalidOrEmpty;
            goto _Return;
        }

        // 함수 이름이 테이블에 존재하지 않음
        var strFuncTargetName = funcName.ToString();
        if (!target.TryGetValue(strFuncTargetName, out var fpTarget))
        {
            *errno = YamlXXParseError.ExprActionNameNotFound;
            goto _Return;
        }

        // 임시용 로컬 오류
        var localError = YamlXXParseError.Success;

        // 인수 읽기
        FuncArgsOnStack argsOnStack;
        argsOnStack._arg0 = ResolveArgumentText(ref input, strmap, errno);
        argsOnStack._arg1 = ResolveArgumentText(ref input, strmap, &localError);

        // 둘 다 성공했는지?
        if ((*errno | localError) == YamlXXParseError.Success)
        {
            // 타겟 실행
            __retVal = fpTarget.Invoke(in argsOnStack);
        }

    _Return:
        SkipAfter(ref input, ')');
        return __retVal;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ReadOnlySpan<char> ReadToken(ref ReadOnlySpan<char> s)
    {
        var v = s;

        v = v.TrimStart();

        int i = 0;
        while (i < v.Length && !char.IsWhiteSpace(v[i])) i++;

        s = v[i..];
        return v[..i];
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe string ResolveArgumentText(
        ref ReadOnlySpan<char> input,
        MappedStringDict strmap,
        [DisallowNull, NotNull] YamlXXParseError* errno)
    {
        Debug.Assert(errno != null);

        var __retVal = string.Empty;

        var v = input.TrimStart();

        // 1. empty string
        // 2. block end
        if (v.Length > 0 && v[0] != ')')
        {
            if (v[0] == '$')
            {
                // hashtable key
                __retVal = ReadHashtableValue(ref v, strmap, errno);
            }
            else if (v[0] == '\'')
            {
                // string literal
                __retVal = ReadStringLiteral(ref v, errno);
            }
            else
            {
                // not supported type
                *errno = YamlXXParseError.ExprNotSupportedLiteralType;
            }
        }

        input = v;
        return __retVal;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe string ReadHashtableValue(
        ref ReadOnlySpan<char> s,
        MappedStringDict strmap,
        [DisallowNull, NotNull] YamlXXParseError* errno)
    {
        Debug.Assert(errno != null);

        // remove '$'
        s = s[1..];
        var k = s;
        var idx = k.IndexOfAny('\'', ')');
        if (idx < 0)
        {
            *errno = YamlXXParseError.ExprUnexpectedEndOfBlock;
            return string.Empty;
        }

        // if ends ')', keep character
        s = k[(idx + (k[idx] != ')' ? 1 : 0))..];

        k = k[..idx].TrimEnd();
        if (k.Length <= 0)
        {
            *errno = YamlXXParseError.ExprHashtableEmptyKey;
            return string.Empty;
        }

        if (!strmap.TryGetValue(k.ToString(), out var result))
        {
            *errno = YamlXXParseError.ExprHashtableKeyNotFound;
            return string.Empty;
        }

        return result;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe string ReadStringLiteral(
        ref ReadOnlySpan<char> s,
        [DisallowNull, NotNull] YamlXXParseError* errno)
    {
        Debug.Assert(errno != null);

        // remove '\''
        s = s[1..];
        var k = s;
        var idx = k.IndexOfAny('\'', ')');
        if (idx < 0)
        {
            // string literal is required close quote e.g.: 'is text'
            // not allowd, e.g.: 'is my text
            *errno = YamlXXParseError.ExprStringLiteralNotClosed;
            return string.Empty;
        }

        // if ends ')' keep this character
        if (k[idx] == ')')
        {
            s = k[..idx];
            *errno = YamlXXParseError.ExprStringLiteralNotClosed;
            return string.Empty;
        }

        s = k[(idx + 1)..];
        return k[..idx].ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SkipAfter(ref ReadOnlySpan<char> s, char find)
    {
        var idx = s.IndexOf(find);
        if (idx >= 0)
        {
            s = s[(idx + 1)..];
        }
    }
}
