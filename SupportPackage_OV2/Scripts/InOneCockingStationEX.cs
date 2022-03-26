using System;
using System.Collections.Generic;
using UnityEngine;
using TUNING;

namespace SupportPackage
{
    using static STRINGS.UI.BUILDINGEFFECTS;
    public class InOneCockingStationEX : ComplexFabricator, IGameObjectEffectDescriptor
	{
        protected override void OnPrefabInit()
        {
            base.OnPrefabInit();

            var db = Db.Get();
            choreType = db.ChoreTypes.Cook;
            fetchChoreTypeIdHash = db.ChoreTypes.CookFetch.IdHash;
        }

        protected override void OnSpawn()
        {
            base.OnSpawn();

            var db = Db.Get();

            var w = workable;
            w.requiredSkillPerk = db.SkillPerks.CanElectricGrill.Id;
            w.WorkerStatusItem = db.DuplicantStatusItems.Cooking;
            
            w.overrideAnims = new KAnimFile[1]
            {
                Assets.GetAnim((HashedString) "anim_interacts_cookstation_kanim")
            };

            w.AttributeConverter = db.AttributeConverters.CookingSpeed;

            w.AttributeExperienceMultiplier = DUPLICANTSTATS.ATTRIBUTE_LEVELING.MOST_DAY_EXPERIENCE;

            w.SkillExperienceSkillGroup = db.SkillGroups.Cooking.Id;

            w.SkillExperienceMultiplier = SKILLS.MOST_DAY_EXPERIENCE;

            w.OnWorkTickActions += WORK_TICK_ACTION;
        }

        private void WORK_TICK_ACTION(Worker w, float dt)
        {
            Debug.Assert(w != null, "How did we get a null worker?");

            GetComponent<PrimaryElement>()
                .ModifyDiseaseCount(
                    -Math.Max(1, (int)(100d * dt)),
                    nameof(InOneCockingStationEX));
        }

        protected override List<GameObject> SpawnOrderProduct(ComplexRecipe completed_order)
		{
			List<GameObject> gol = base.SpawnOrderProduct(completed_order);

			for (int x = 0; x < gol.Count; x++)
            {
				PrimaryElement c = gol[x].GetComponent<PrimaryElement>();
				c.ModifyDiseaseCount(-c.DiseaseCount, nameof(InOneCockingStationEX) + "CompleteOrder");
                c.Temperature = 253.15f;
			}
			
			GetComponent<Operational>().SetActive(false, false);

			return gol;
		}

        public override List<Descriptor> GetDescriptors(GameObject go)
        {
            var res = base.GetDescriptors(go);
            res.Add(new Descriptor(REMOVES_DISEASE, TOOLTIPS.REMOVES_DISEASE));
            return res;
        }
    }
}
