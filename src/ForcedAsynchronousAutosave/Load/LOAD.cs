using AsynchronousAutosave.Translation;
using Harmony;
using Klei.CustomSettings;
using KMod;
using KSerialization;
using ProcGenGame;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AsynchronousAutosave
{
    [HarmonyPatch(typeof(SaveLoader), nameof(SaveLoader.Load), typeof(string))]
    public partial class LOADPATCH
    {
        private static PropertyInfo gameinfo;
        // private static FieldInfo prefabMap;

        public static bool Prepare()
        {
            gameinfo = typeof(SaveLoader).GetProperty(nameof(SaveLoader.GameInfo), BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty);
            MODMESSAGE.SetDefaultMessage();
            StringPatch.SetMessage(MODMESSAGE.Message);
            return true;
        }

        public static bool Prefix(ref bool __result, SaveLoader __instance, string filename)
        {
            SaveLoader.SetActiveSaveFilePath(filename);
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
                #region Load(IReader)
                Debug.Assert(reader.ReadByte() == AsyncAutosaveCS.WORLDHEADER);
                
                SaveFileRoot sfr = new SaveFileRoot();
                sfr.WidthInCells = reader.ReadInt32();
                sfr.HeightInCells = reader.ReadInt32();
                sfr.Sim = reader.GetBytes();
                sfr.GridVisible = reader.GetBytes();
                sfr.GridSpawnable = reader.GetBytes();
                sfr.GridDamage = reader.GetBytes();
                sfr.Camera = reader.GetBytes();

                sfr.active_mods = new List<Label>();
                int len = reader.ReadInt32();
                for (int x = 0; x < len; x++)
                    sfr.active_mods.Add(new Label
                    {
                        distribution_platform = (Label.DistributionPlatform)reader.ReadByte(),
                        id = reader.GetString(),
                        version = reader.ReadInt64(),
                        title = reader.GetString()
                    });

                Deserializer deser = new Deserializer(reader);
                KMod.Manager modManager = Global.Instance.modManager;
                modManager.Load(Content.LayerableFiles);
                if (!modManager.MatchFootprint(sfr.active_mods, Content.LayerableFiles | Content.Strings | Content.DLL | Content.Translation | Content.Animation))
                    DebugUtil.LogWarningArgs("Mod footprint of save file doesn't match current mod configuration");
                modManager.SendMetricsEvent();

                WorldGen.LoadSettings();
                CustomGameSettings.Instance.LoadWorlds();

                SaveGame.GameInfo _gameinfo = (SaveGame.GameInfo)gameinfo.GetValue(__instance, null);
                if (_gameinfo.worldID == null)
                {
                    if (!string.IsNullOrEmpty(sfr.worldID))
                        gi.worldID = sfr.worldID;
                    else
                        try
                        {
                            gi.worldID = CustomGameSettings.Instance.GetCurrentQualitySetting(CustomGameSettingConfigs.World).id;
                        }
                        catch
                        {
                            gi.worldID = "worlds/SandstoneDefault";
                        }
                    gameinfo.SetValue(__instance, gi, null);
                }
                if (_gameinfo.worldTraits == null)
                {
                    gi.worldTraits = new string[0];
                    gameinfo.SetValue(__instance, gi, null);
                }

                __instance.worldGen = new WorldGen(gi.worldID, new List<string>(gi.worldTraits), false);

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
                // if (!LOAD(__instance.saveManager, reader))
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
                #endregion
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
