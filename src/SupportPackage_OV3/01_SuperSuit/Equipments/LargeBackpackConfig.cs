// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace SupportPackage;

using static GlobalConsts.SUPER_SUIT.LargeBackpack;
public sealed class LargeBackpackConfig : SmallBackpackConfig
{
    public override EquipmentDef CreateEquipmentDef() =>
        CreateBackpack(ID, CLOTH_MASS, ICON, ANISTR, ADD_KG, SPEED_PENALTY);

    protected override ClothingWearer.ClothingInfo CInfo => Info;
}
