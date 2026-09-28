using System;
using System.Collections.Generic;
using XLua;

public static class WarmupLuaConfig
{
    [CSharpCallLua]
    public static List<Type> CSharpCallLua = new List<Type>
    {
        typeof(Action),
        typeof(Action<string>),
        typeof(Action<string, string>),
        typeof(Action<bool, string, string>),
    };

    [LuaCallCSharp]
    public static List<Type> LuaCallCSharp = new List<Type>
    {
        typeof(BootFlow.WarmupFlow),
    };
}
