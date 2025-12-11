// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#nullable enable
using VYaml.Annotations;
using VYaml.Serialization;

namespace SuperComicLib.ModONI.Configurations;

[YamlObject(NamingConvention.SnakeCase)]
[method: YamlConstructor]
public sealed partial record GlobalConfigurations(
    bool DisableOptimizedDllLoader)
{
    private static GlobalConfigurations? s_globalConfigs;

    public static GlobalConfigurations CurrentActivate => s_globalConfigs ??= LoadOrCreate();

    public static GlobalConfigurations DefaultSettings => new(false);

    private static GlobalConfigurations LoadOrCreate()
    {
        var file = ModFileOrDirectoryHints.GlobalCommonConfigFile;
        GlobalConfigurations? result = null;
        if (File.Exists(file))
        {
            try
            {
                result = YamlSerializer.Deserialize<GlobalConfigurations>(File.ReadAllBytes(file));
            }
            catch (Exception ex)
            {
                FDebug.LogError(@"An error occurred while reading the global settings file.");
                FDebug.LogException(ex);
            }
        }

        if (result is null)
        {
            // write to default value
            result ??= DefaultSettings;

            try
            {
                var mem = YamlSerializer.Serialize(result);
                using var fs = File.Create(file);
                fs.Write(mem.Span);
                fs.Close();
            }
            catch (Exception ex)
            {
                FDebug.LogWarning(@"Failed to write default global settings file to disk.");
                FDebug.LogWarning($"Exception: {ex}");
            }
        }

        return result;
    }
}
