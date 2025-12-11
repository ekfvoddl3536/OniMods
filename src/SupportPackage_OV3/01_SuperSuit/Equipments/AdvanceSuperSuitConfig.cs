// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using Klei.AI;

namespace SupportPackage;

using static GlobalConsts.SUPER_SUIT.Advance_SuperSuit;
public sealed class AdvanceSuperSuitConfig : BasicSuperSuitConfig
{
    public override EquipmentDef CreateEquipmentDef() =>
        Create(ID, CLOTH_MASS, ICON, ANISTR, DECOR_MOD, new List<AttributeModifier>(0));

    protected override ClothingWearer.ClothingInfo CInfo => Info;
}
