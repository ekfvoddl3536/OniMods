using System.Collections.Generic;
using Klei.AI;

namespace SupportPackage
{
    using static GlobalConsts.SUPER_SUIT;
    using static GlobalConsts.SUPER_SUIT.SmallBackpack;
    public class SmallBackpackConfig : BasicSuperSuitConfig
    {
        public override EquipmentDef CreateEquipmentDef() =>
            CreateBackpack(ID, CLOTH_MASS, ICON, ANISTR, ADD_KG, SPEED_PENALTY);

        protected EquipmentDef CreateBackpack(
            string id, 
            int cloth_mass, 
            string icon, 
            string anistr, 
            int addkg,
            int speed)
        {
            string name = id.equipmentName();
            return
                Create(
                    id,
                    cloth_mass,
                    icon,
                    anistr,
                    DECOR_MOD_DEF,
                    new List<AttributeModifier>(2)
                    {
                        new AttributeModifier(AT_AHID, speed, name),
                        new AttributeModifier(AT_CAID, addkg, name),
                    });
        }

        protected override ClothingWearer.ClothingInfo CInfo => Info;
    }
}
