// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace SuperComicLib;

internal static class FDebug
{
    public const string PrefixSCOXNI = "SCOXNI";

    public readonly ref struct ScopeBlock(string name) : IDisposable
    {
        private readonly string _name = name;
        public readonly void Dispose() => s_currentScope = _name;
    }

    private static string? s_currentScope;

    public static ScopeBlock EnterScope(string? name)
    {
        var old = s_currentScope;
        s_currentScope = name;
        return new(old);
    }

    public static void Log(string text)
    {
        var scope = s_currentScope;
        if (scope is not null)
        {
            Debug.Log($"[{PrefixSCOXNI}/{scope}] {text}");
        }
        else
        {
            Debug.Log($"[{PrefixSCOXNI}] {text}");
        }
    }

    public static void LogWarning(string text)
    {
        var scope = s_currentScope;
        if (scope is not null)
        {
            Debug.LogWarning($"[{PrefixSCOXNI}/{scope}] {text}");
        }
        else
        {
            Debug.LogWarning($"[{PrefixSCOXNI}] {text}");
        }
    }

    public static void LogError(string text)
    {
        var scope = s_currentScope;
        if (scope is not null)
        {
            Debug.LogError($"[{PrefixSCOXNI}/{scope}] {text}");
        }
        else
        {
            Debug.LogError($"[{PrefixSCOXNI}] {text}");
        }
    }

    public static void LogException(Exception exception) => Debug.LogException(exception);
}
