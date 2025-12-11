// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#nullable enable
using System.Reflection;
using HarmonyLib;
using KMod;
using SuperComicLib.ModONI.Configurations;
using VYaml.Serialization;

namespace SuperComicLib.ModONI;

[HarmonyPatch(typeName: "KMod.DLLLoader", methodName: "LoadDLLs")]
internal static partial class DLLLoader_LoadDLLs_Patch
{
    private static bool s_isPatched;

    public static bool Prepare()
    {
        if (!GlobalConfigurations.CurrentActivate.DisableOptimizedDllLoader)
        {
            if (!s_isPatched)
            {
                Debug.Log(@"================================================================================");
                Debug.Log($"[SCOXNI] A custom mod DLL loader is being used instead of the vanilla mod DLL loader.");
                Debug.Log($"[SCOXNI] This feature is designed to be disabled in case it causes problems.");
                Debug.Log($"[SCOXNI] To permanently disable this feature, edit `config/scoxni-global.common.yaml` in the game's mods directory.");
                Debug.Log(@"================================================================================");
            }

            return s_isPatched = true;
        }
        else
        {
            return false;
        }
    }

    public static bool Prefix(ref LoadedModData? __result, Mod ownerMod, string harmonyId, string path, bool isDev)
    {
        var optFile = Path.Combine(path, ModFileOrDirectoryHints.UserModManifestFileName);
        if (File.Exists(optFile))
        {
            __result = OptimizedLoadDLLs(ownerMod, harmonyId, path, isDev, optFile);
            return false;
        }
        
        return true;
    }

    private static LoadedModData? OptimizedLoadDLLs(Mod ownerMod, string harmonyId, string path, bool isDev, string optFile)
    {
        const string DLL = ".dll";

        using var __s = FDebug.EnterScope("OptimizedDLLLoader");
        FDebug.Log($"Current Mod Location: '{path}'");

        LoadedModData? loadedModData = null;
        Assembly assembly;
        try
        {
            const uint ENUM_COMPARE = Testing.DLLLoading.UseModLoaderDLLExclusively - Testing.DLLLoading.Fail;
            if ((uint)(Testing.dll_loading - Testing.DLLLoading.Fail) <= ENUM_COMPARE) goto _Return;

            FDebug.Log($"Read TargetAssembly information from the `{ModFileOrDirectoryHints.UserModManifestFileName}` file. Mod {harmonyId} at {path}.");

            var manifest = YamlSerializer.Deserialize<UserModManifest>(File.ReadAllBytes(optFile));
            var filename = manifest.TargetAssembly.AsSpan().Trim();
            if (filename.Length <= 0) goto _TargetAssemblyNotFound;

            FDebug.Log($"   - {nameof(UserModManifest.TargetAssembly)}: {manifest.TargetAssembly}");

            // weak, lightweight safety cehck (path traversal)
            if (filename.IndexOfAny('\\', '/') >= 0) goto _TargetAssemblyNamePathTraversal;

            if (!filename.EndsWith(DLL, StringComparison.OrdinalIgnoreCase))
            {
                FDebug.LogWarning($"{manifest.TargetAssembly} does not end in `{DLL}`. Loading may fail.");
            }

            var assemblyFile = Path.Combine(path, manifest.TargetAssembly);
            if (!File.Exists(assemblyFile)) goto _TargetAssemblyNotFound;

            assembly = Assembly.LoadFrom(assemblyFile);

            var assemblyList = new List<Assembly>() { assembly };
            foreach (var file in Directory.EnumerateFiles(path))
            {
                var fileSpan = file.AsSpan();
                if (!Path.GetExtension(fileSpan).Equals(DLL, StringComparison.OrdinalIgnoreCase))
                {
                    // not 'DLL'
                    continue;
                }

                if (Path.GetFileName(fileSpan).Equals(filename, StringComparison.Ordinal))
                {
                    // already loaded assembly (skip)
                    continue;
                }

                FDebug.Log($"    - Loading dependency DLL: {Path.GetFileName(file)}");
                assemblyList.Add(Assembly.LoadFrom(file));
            }

            V1XXModImpl? userMod = null;
            {
                var exportedTypes = assembly.GetExportedTypes();
                for (int i = 0; i < exportedTypes.Length; ++i)
                {
                    var k = exportedTypes[i];
                    if (k is not null && typeof(V1XXModImpl).IsAssignableFrom(k))
                    {
                        if (userMod is not null) goto _MultipleUserMod;

                        userMod = Activator.CreateInstance(k) as V1XXModImpl;
                    }
                }

                if (userMod is null) goto _UserModNotFound;
            }

            userMod.assembly = assembly;
            userMod.path = path;
            userMod.mod = ownerMod;

            var tempData = new LoadedModData
            {
                dlls = assemblyList,
                userMod2Instances = new() { { assembly, userMod } },
                harmony = new Harmony(harmonyId)
            };

            FDebug.Log($"Loading MOD dll: {manifest.TargetAssembly}");
            userMod.OnLoad(tempData.harmony);

            tempData.patched_methods = [.. tempData.harmony.GetPatchedMethods().Where(m => Harmony.GetPatchInfo(m).Owners.Contains(harmonyId))];
            loadedModData = tempData;
        }
        catch (Exception ex)
        {
            DebugUtil.LogException(null, $"Exception while loading mod {harmonyId} at {path}.", ex);
        }

    _Return:
        return loadedModData;

    _TargetAssemblyNotFound:
        FDebug.LogError($"Target assembly specified in usermod manifest is missing or incorrect. Mod {harmonyId} at {path}.");
        goto _Return;

    _MultipleUserMod:
        FDebug.LogError($"Found more than one class inheriting `{nameof(V1XXModImpl)}` in {assembly.FullName}, only one per assembly is allowed. Aborting load.");
        goto _Return;

    _UserModNotFound:
        FDebug.LogError($"Class inheriting {nameof(V1XXModImpl)} not found in {assembly.FullName}, one is required per assembly. Aborting load.");
        goto _Return;

    _TargetAssemblyNamePathTraversal:
        FDebug.LogError($"Target assembly name cannot contain Path Traversal. Mod {harmonyId} at {path}.");
        goto _Return;
    }
}
