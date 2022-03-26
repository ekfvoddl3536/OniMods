using System.Collections.Generic;
using Klei.AI;

namespace SupportPackage
{
    using static GlobalConsts.SUPER_SUIT.Advance_SuperSuit;
    public sealed class AdvanceSuperSuitConfig : BasicSuperSuitConfig
    {
        public override EquipmentDef CreateEquipmentDef() =>
            Create(ID, CLOTH_MASS, ICON, ANISTR, DECOR_MOD, new List<AttributeModifier>(0));

        protected override ClothingWearer.ClothingInfo CInfo => Info;
    }
}
