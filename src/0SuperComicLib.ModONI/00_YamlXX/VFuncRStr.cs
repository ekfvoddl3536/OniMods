// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace SuperComicLib.YamlXX;

public readonly unsafe struct VFuncRStr
{
    private readonly delegate* managed<void*, in FuncArgsOnStack, string> _Target;
    private readonly void* _TargetAux;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VFuncRStr(delegate* managed<string, string> fp) : this(&__jmp01, fp) { }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public VFuncRStr(delegate* managed<string, string, string> fp) : this(&__jmp02, fp) { }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private VFuncRStr(delegate* managed<void*, in FuncArgsOnStack, string> target, void* targetAux)
    {
        _Target = target;
        _TargetAux = targetAux;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string Invoke(in FuncArgsOnStack argsOnStack)
    {
        // NOTE:
        //  x86-64 기준으로 이론적으로 volatile-register를 수정한다고 문제가 되지 않으므로
        //  이 부분을 최대 길이의 인수에 맞춰서 direct call 하더라도 문제는 없을 것이다. like: ((str, str, str)aux)(a, b)
        //  그러나, aarch64 와 같이 x86-64 calling convention이 아닌 환경에서
        //  violation access 또는 stack clean-up issue, etc. 의 잠재적 문제가 있을 수 있다.
        //  따라서, virtcall style을 취하기로 한다
        return _Target(_TargetAux, in argsOnStack);
    }

    private static string __jmp01(void* fpTarget, in FuncArgsOnStack argsOnStack)
    {
        Debug.Assert(fpTarget != null);
        
        return ((delegate* managed<string, string>)fpTarget)(argsOnStack._arg0);
    }
    
    private static string __jmp02(void* fpTarget, in FuncArgsOnStack argsOnStack)
    {
        Debug.Assert(fpTarget != null);
        
        return ((delegate* managed<string, string, string>)fpTarget)(argsOnStack._arg0, argsOnStack._arg1);
    }
}