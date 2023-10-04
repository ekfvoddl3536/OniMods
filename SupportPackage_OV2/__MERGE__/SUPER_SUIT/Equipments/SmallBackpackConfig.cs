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
