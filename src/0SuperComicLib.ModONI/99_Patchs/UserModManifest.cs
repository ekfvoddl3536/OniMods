// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

#nullable enable
using VYaml.Annotations;

namespace SuperComicLib.ModONI;

[YamlObject(NamingConvention.SnakeCase)]
[method: YamlConstructor]
internal readonly partial struct UserModManifest(string targetAssembly/*, string[] dependsOn*/)
{
    public readonly string TargetAssembly { get; } = targetAssembly;
    // public readonly string[] DependsOn { get; } = dependsOn;
}
