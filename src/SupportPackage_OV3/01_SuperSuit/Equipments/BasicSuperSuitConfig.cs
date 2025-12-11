// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using Klei.AI;
using UnityEngine;

namespace SupportPackage;

using static GlobalConsts.SUPER_SUIT;
using static GlobalConsts.SUPER_SUIT.Basic_SuperSuit;
using static SUPER_SUIT_COMMON_METHODS;
using static TUNING.EQUIPMENT.CLOTHING;
public class BasicSuperSuitConfig : IEquipmentConfig
{
    public virtual EquipmentDef CreateEquipmentDef() => Create(ID, CLOTH_MASS, ICON, ANISTR, DECOR_MOD, []);

    protected EquipmentDef Create(
        string id,
        float cloth_mass,
        string icon,
        string anistr,
        int decorMod,
        List<AttributeModifier> attrbs)
    {
        var res =
            EquipmentTemplates.CreateEquipmentDef(
                id, SLOT,
                SimHashes.Carbon, cloth_mass,
                icon, SNAPON0,
                anistr, 4,
                attrbs,
                SNAPON1, true,
                EntityTemplates.CollisionShape.RECTANGLE,
                0.75f, 0.4f);

        var t_desc = Desc_thermal;
        res.additionalDescriptors.Add(new Descriptor(t_desc, t_desc));

        t_desc = DESC_DECOR(decorMod);
        res.additionalDescriptors.Add(new Descriptor(t_desc, t_desc));

        res.OnEquipCallBack = OnEquipVest;
        res.OnUnequipCallBack = ClothingWearer.ClothingInfo.OnUnequipVest;

        res.RecipeDescription = ID.equipmentEffect();


        return res;
    }

    public void OnEquipVest(Equippable eq) => ClothingWearer.ClothingInfo.OnEquipVest(eq, CInfo);

    protected virtual ClothingWearer.ClothingInfo CInfo => Info;

    public void DoPostConfigure(GameObject go)
    {
        var pid = go.GetComponent<KPrefabID>();
        pid.AddTag(GameTags.Clothes);
        pid.AddTag(GameTags.PedestalDisplayable);

        go.AddOrGet<Equippable>().SetQuality(QualityLevel.Good);

        go.GetComponent<KBatchedAnimController>().sceneLayer = Grid.SceneLayer.BuildingBack;
    }
}
