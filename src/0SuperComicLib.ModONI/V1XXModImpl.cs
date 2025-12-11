// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using HarmonyLib;
using SuperComicLib.ModONI.Configurations;

namespace SuperComicLib.ModONI;

public partial class V1XXModImpl : KMod.UserMod2
{
    public override void OnLoad(Harmony harmony)
    {
        var assemblyName = assembly.GetName().Name;
        
        using var s = FDebug.EnterScope(null);
        FDebug.Log($"Starting to load '{assemblyName}' assembly...");

        var context = new StringResourceLoadContext(this);
        context.Init();
        try
        {
            // 전역 설정을 읽는다.
            _ = GlobalConfigurations.CurrentActivate;

            OnLoadCore(harmony, ref context);
        }
        finally
        {
            context.Restore();
        }

        if (!context.Cancel)
        {
            FDebug.Log($"'{assemblyName}' assembly loaded successfully!");
        }
    }

    protected virtual void OnLoadCore(Harmony harmony, ref StringResourceLoadContext context)
    {
        OnPreResourceLoad(ref context);
        if (context.Cancel)
        {
            // if self?
            if (GetType() == typeof(V1XXModImpl))
            {
                OnPatch(harmony);
            }

            FDebug.LogWarning($"'{GetType().FullName}' is not in a loadable Mod, or the load was canceled.");
            return;
        }

        context.Build();

        var resource = ModStringResource.Create(in context);
        if (!resource.TryLoad())
        {
            OnLoadFailed();
            return;
        }
        
        var initializeContext = new InitializeContext();
        initializeContext.MakeLoopReference();
        OnReady(ref initializeContext);

        OnPatch(harmony);
    }

    protected virtual void OnPreResourceLoad(ref StringResourceLoadContext context)
    {
        context.Cancel = true;
    }

    protected virtual void OnReady(ref InitializeContext context) { }

    protected virtual void OnPatch(Harmony harmony) => harmony.PatchAll(assembly);

    protected virtual void OnLoadFailed()
    {
        throw new InvalidOperationException("Load string resources fail.");
    }
}