#if USE_UNI_LUA
using LuaAPI = UniLua.Lua;
using RealStatePtr = UniLua.ILuaState;
using LuaCSFunction = UniLua.CSharpFunctionDelegate;
#else
using LuaAPI = XLua.LuaDLL.Lua;
using RealStatePtr = System.IntPtr;
using LuaCSFunction = XLua.LuaDLL.lua_CSFunction;
#endif

using XLua;
using System.Collections.Generic;


namespace XLua.CSObjectWrap
{
    using Utils = XLua.Utils;
    public class BootFlowWarmupFlowWrap 
    {
        public static void __Register(RealStatePtr L)
        {
			ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			System.Type type = typeof(BootFlow.WarmupFlow);
			Utils.BeginObjectRegister(type, L, translator, 0, 15, 6, 0);
			
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadLaunch", _m_ReadLaunch);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "TryOpenInboxPage", _m_TryOpenInboxPage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "IsWebLink", _m_IsWebLink);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ShowMenu", _m_ShowMenu);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "SetLoader", _m_SetLoader);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "Trace", _m_Trace);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "OpenLesson", _m_OpenLesson);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FailBoot", _m_FailBoot);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "EnsureSignedIn", _m_EnsureSignedIn);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PullRemotePage", _m_PullRemotePage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "FillPocket", _m_FillPocket);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PersistSkip", _m_PersistSkip);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "PersistPage", _m_PersistPage);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ReadDeskGate", _m_ReadDeskGate);
			Utils.RegisterFunc(L, Utils.METHOD_IDX, "ResolveLesson", _m_ResolveLesson);
			
			
			Utils.RegisterFunc(L, Utils.GETTER_IDX, "HasBell", _g_get_HasBell);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "WaitCode", _g_get_WaitCode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LastPage", _g_get_LastPage);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "PageOpened", _g_get_PageOpened);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "OpenedWithCode", _g_get_OpenedWithCode);
            Utils.RegisterFunc(L, Utils.GETTER_IDX, "LessonOn", _g_get_LessonOn);
            
			
			
			Utils.EndObjectRegister(type, L, translator, null, null,
			    null, null, null);

		    Utils.BeginClassRegister(type, L, __CreateInstance, 1, 0, 0);
			
			
            
			
			
			
			Utils.EndClassRegister(type, L, translator);
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int __CreateInstance(RealStatePtr L)
        {
            return LuaAPI.luaL_error(L, "BootFlow.WarmupFlow does not have a constructor!");
        }
        
		
        
		
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadLaunch(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ReadLaunch(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_TryOpenInboxPage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                        var gen_ret = gen_to_be_invoked.TryOpenInboxPage(  );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_IsWebLink(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _page = LuaAPI.lua_tostring(L, 2);
                    
                        var gen_ret = gen_to_be_invoked.IsWebLink( _page );
                        LuaAPI.lua_pushboolean(L, gen_ret);
                    
                    
                    
                    return 1;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ShowMenu(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    
                    gen_to_be_invoked.ShowMenu(  );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_SetLoader(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    float _normalized = (float)LuaAPI.lua_tonumber(L, 2);
                    
                    gen_to_be_invoked.SetLoader( _normalized );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_Trace(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _message = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.Trace( _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_OpenLesson(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _page = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.OpenLesson( _page );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FailBoot(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _message = LuaAPI.lua_tostring(L, 2);
                    
                    gen_to_be_invoked.FailBoot( _message );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_EnsureSignedIn(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<string> _done = translator.GetDelegate<System.Action<string>>(L, 2);
                    
                    gen_to_be_invoked.EnsureSignedIn( _done );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PullRemotePage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<string> _done = translator.GetDelegate<System.Action<string>>(L, 2);
                    
                    gen_to_be_invoked.PullRemotePage( _done );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_FillPocket(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<string> _done = translator.GetDelegate<System.Action<string>>(L, 2);
                    
                    gen_to_be_invoked.FillPocket( _done );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PersistSkip(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action _done = translator.GetDelegate<System.Action>(L, 2);
                    
                    gen_to_be_invoked.PersistSkip( _done );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_PersistPage(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    string _page = LuaAPI.lua_tostring(L, 2);
                    System.Action _done = translator.GetDelegate<System.Action>(L, 3);
                    
                    gen_to_be_invoked.PersistPage( _page, _done );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ReadDeskGate(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<bool, string, string> _done = translator.GetDelegate<System.Action<bool, string, string>>(L, 2);
                    
                    gen_to_be_invoked.ReadDeskGate( _done );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _m_ResolveLesson(RealStatePtr L)
        {
		    try {
            
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
            
            
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
            
            
                
                {
                    System.Action<string, string> _done = translator.GetDelegate<System.Action<string, string>>(L, 2);
                    
                    gen_to_be_invoked.ResolveLesson( _done );
                    
                    
                    
                    return 0;
                }
                
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            
        }
        
        
        
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_HasBell(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.HasBell);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_WaitCode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.WaitCode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LastPage(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.LastPage);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_PageOpened(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.PageOpened);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_OpenedWithCode(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushstring(L, gen_to_be_invoked.OpenedWithCode);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        [MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
        static int _g_get_LessonOn(RealStatePtr L)
        {
		    try {
                ObjectTranslator translator = ObjectTranslatorPool.Instance.Find(L);
			
                BootFlow.WarmupFlow gen_to_be_invoked = (BootFlow.WarmupFlow)translator.FastGetCSObj(L, 1);
                LuaAPI.lua_pushboolean(L, gen_to_be_invoked.LessonOn);
            } catch(System.Exception gen_e) {
                return LuaAPI.luaL_error(L, "c# exception:" + gen_e);
            }
            return 1;
        }
        
        
        
		
		
		
		
    }
}
