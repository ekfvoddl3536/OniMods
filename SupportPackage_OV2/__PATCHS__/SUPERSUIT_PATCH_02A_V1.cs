using HarmonyLib;

namespace SupportPackage
{
    [HarmonyPatch(typeof(GeneratedEquipment), nameof(GeneratedEquipment.LoadGeneratedEquipment))]
    public static class SUPERSUIT_PATCH_02A_V1
    {
        public static void Postfix()
        {
            // add equipment
            var inst = EquipmentConfigManager.Instance;

            inst.RegisterEquipment(new BasicSuperSuitConfig());
            inst.RegisterEquipment(new AdvanceSuperSuitConfig());

            inst.RegisterEquipment(new SmallBackpackConfig());
            inst.RegisterEquipment(new MediumBackpackConfig());
            inst.RegisterEquipment(new LargeBackpackConfig());
        }
    }
}
