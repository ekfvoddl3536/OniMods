using Harmony;
using System.Collections.Generic;
using System.IO;
using System;
using KSerialization;
using System.Reflection;
using KMod;
using ProcGenGame;
using Klei.CustomSettings;

namespace AsynchronousAutosave
{
    [HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Load), typeof(string))]
    public class LOADPATCH
    {
        private static PropertyInfo gameinfo;

        public static bool Prepare()
        {
            Type t = typeof(SaveLoader);
            BindingFlags flag = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty;
            gameinfo = t.GetProperty(nameof(SaveLoader.GameInfo), flag);
            return true;
        }

        public static bool Prefix(ref bool __result, SaveLoader __instance, string filename)
        {
            KPlayerPrefs.SetString("SaveFilenameKey/", filename);
            try
            {
                KSerialization.Manager.Clear();
                byte[] bytes = File.ReadAllBytes(filename);
                IReader reader = new FastReader(bytes);
                reader.GetHeader(out SaveGame.GameInfo gi, out SaveGame.Header header);
                gameinfo.SetValue(__instance, gi, null);

                DebugUtil.LogArgs($"Loading save file: {filename}\n headerVersion:{header.headerVersion}, buildVersion:{header.buildVersion}, headerSize:{header.headerSize}, IsCompressed:{header.IsCompressed}");
                DebugUtil.LogArgs($"GameInfo: numberOfCycles:{gi.numberOfCycles}, numberOfDuplicants:{gi.numberOfDuplicants}, baseName:{gi.baseName}, isAutoSave:{gi.isAutoSave}, originalSaveName:{gi.originalSaveName}, saveVersion:{gi.saveMajorVersion}.{gi.saveMinorVersion}");

                if (gi.saveMajorVersion == 7 && gi.saveMinorVersion < 4)
                    Helper.SetTypeInfoMask(SerializationTypeInfo.VALUE_MASK | SerializationTypeInfo.IS_GENERIC_TYPE);
                KSerialization.Manager.DeserializeDirectory(reader);

                // Load(IReader reader)
                Debug.Assert(reader.ReadKleiString() == "world");

                SaveFileRoot sfr = new SaveFileRoot();
                sfr.WidthInCells = reader.GetInt32SMART();
                sfr.HeightInCells = reader.GetInt32SMART();
                sfr.Sim = reader.GetBytesSMART();
                sfr.GridVisible = reader.GetBytesSMART();
                sfr.GridSpawnable = reader.GetBytesSMART();
                sfr.GridDamage = reader.GetBytesSMART();
                sfr.Camera = reader.GetBytesSMART();

                sfr.active_mods = new List<Label>();
                int len = reader.GetInt32SMART();
                for (int x = 0; x < len; x++)
                    sfr.active_mods.Add(new Label
                    {
                        distribution_platform = (Label.DistributionPlatform)reader.ReadByte(),
                        id = reader.ReadKleiString(),
                        version = reader.GetInt64SMART(),
                        title = reader.ReadKleiString()
                    });

                Deserializer deser = new Deserializer(reader);
                KMod.Manager modManager = Global.Instance.modManager;
                modManager.Load(Content.LayerableFiles);
                if (!modManager.MatchFootprint(sfr.active_mods, Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation))
                    DebugUtil.LogWarningArgs("Mod footprint of save file doesn't match current mod configuration");
                modManager.SendMetricsEvent();

                WorldGen.LoadSettings();
                CustomGameSettings.Instance.LoadWorlds();
                if (gi.worldID == null)
                {
                    if (!string.IsNullOrEmpty(sfr.worldID))
                        gi.worldID = sfr.worldID;
                    else
                    {
                        try
                        {
                            gi.worldID = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.World).id;
                        }
                        catch
                        {
                            gi.worldID = "worlds/SandstoneDefault";
                        }
                    }
                    gameinfo.SetValue(__instance, gi, null);
                }
                if (gi.worldTraits == null)
                    gi.worldTraits = new string[0];
                
                __instance.worldGen = new WorldGen(gi.worldID, new List<string>(gi.worldTraits));

                Game.LoadSettings(deser);
                GridSettings.Reset(sfr.WidthInCells, sfr.HeightInCells);
                Singleton<KBatchedAnimUpdater>.Instance.InitializeGrid();
                Sim.SIM_Initialize(Sim.DLL_MessageHandler);
                SimMessages.CreateSimElementsTable(ElementLoader.elements);
                SimMessages.CreateDiseaseTable();
                if (Sim.Load(new FastReader(sfr.Sim)) != 0)
                {
                    DebugUtil.LogWarningArgs("\n--- Error loading save ---\nSimDLL found bad data\n");
                    Sim.Shutdown();
                    __result = false;
                    return false;
                }
                SceneInitializer.Instance.PostLoadPrefabs();
                if (!__instance.saveManager.Load(reader))
                {
                    DebugUtil.LogWarningArgs("\n--- Error loading save ---\n");
                    Sim.Shutdown();
                    SaveLoader.SetActiveSaveFilePath(null);
                    __result = false;
                    return false;
                }
                Grid.Visible = sfr.GridVisible;
                Grid.Spawnable = sfr.GridSpawnable;
                Grid.Damage = ToFloat(sfr.GridDamage);
                Game.Instance.Load(deser);
                CameraSaveData.Load(new FastReader(sfr.Camera));
                // end

                if (gi.isAutoSave && !string.IsNullOrEmpty(gi.originalSaveName))
                    SaveLoader.SetActiveSaveFilePath(gi.originalSaveName);

                
            }
            catch (Exception ex)
            {
                DebugUtil.LogWarningArgs((object)("\n--- Error loading save ---\n" + ex.Message + "\n" + ex.StackTrace));
                Sim.Shutdown();
                SaveLoader.SetActiveSaveFilePath(null);
                __result = false;
                return false;
            }
            // Stats.Print();
            DebugUtil.LogArgs("Loaded", ("[" + filename + "]"));
            DebugUtil.LogArgs("World Seeds", "[" + __instance.worldDetailSave.globalWorldSeed + "/" + __instance.worldDetailSave.globalWorldLayoutSeed + "/" + __instance.worldDetailSave.globalTerrainSeed + "/" + __instance.worldDetailSave.globalNoiseSeed + "]");
            GC.Collect();
            __result = true;
            return false;
        }

        static float[] ToFloat(byte[] bytes)
        {
            float[] num = new float[bytes.Length / 4];
            Buffer.BlockCopy(bytes, 0, num, 0, bytes.Length);
            return num;
        }
    }
}
