// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using VYaml.Annotations;

namespace SuperComicLib.ModONI.Configurations;

[YamlObject(NamingConvention.SnakeCase)]
public partial struct ConfigOptionItem : IEquatable<ConfigOptionItem>
{
    public string Id { get; internal set; }
    public string Name { get; internal set; }
    
    [YamlConstructor]
    public ConfigOptionItem(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public readonly bool Equals(ConfigOptionItem other) => this == other;

    public override readonly bool Equals(object obj) => obj is ConfigOptionItem other && this == other;
    public override readonly int GetHashCode() => Id.GetHashCode();

    public static bool operator ==(in ConfigOptionItem x, in ConfigOptionItem y) => x.Id.Equals(y.Id, StringComparison.OrdinalIgnoreCase);
    public static bool operator !=(in ConfigOptionItem x, in ConfigOptionItem y) => !(x == y);
}
