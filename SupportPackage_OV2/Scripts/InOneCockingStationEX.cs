// MIT License
//
// Copyright (c) 2022-2023. Super Comic (ekfvoddl3535@naver.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
