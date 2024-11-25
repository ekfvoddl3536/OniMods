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

using Database;
using ci_ = ClothingWearer.ClothingInfo;

namespace SupportPackage
{
    partial class GlobalConsts
    {
        public static class SUPER_SUIT
        {
            public const string BASICFABRIC_ID = "BasicFabric";
            public const string CLOTH_FABRIC = ClothingFabricatorConfig.ID;
            
            public const float TEMP_DEF = 0.0025f;
            public const float BACKPACK_TEMP_DEF = 0.0035f;

            public const int DECOR_MOD_DEF = -10;

            public const string SNAPON0 = "snapTo_body";
            public const string SNAPON1 = "snapTo_arm";

            public const string AT_AHID = nameof(Attributes.Athletics);
            public const string AT_CAID = nameof(Attributes.CarryAmount);

            public static class Basic_SuperSuit
            {
                public const string ID = nameof(Basic_SuperSuit);

                public const string ANISTR = "body_shirt_decor01_kanim";

                public const string ICON = "shirt_decor01_kanim";

                public const int CLOTH_MASS = 8;
                public const int FAB_TIME = 30;

                public const int DECOR_MOD = 120;

                public static ci_ Info;
            }

            public static class Advance_SuperSuit
            {
                public const string ID = nameof(Advance_SuperSuit);

                // public const string ANISTR = "body_shirt_cold01_kanim";
                public const string ANISTR = "body_shirt_decor01_kanim";

                // public const string ICON = "shirt_cold01_kanim";
                public const string ICON = "shirt_decor02_kanim";

                public const int CLOTH_MASS = 20;
                public const int FAB_TIME = 120;

                public const int DECOR_MOD = 9999;

                public static ci_ Info;
            }

            public static class SmallBackpack
            {
                public const string ID = nameof(SmallBackpack);

                public const string ANISTR = "body_shirt_decor01_kanim";

                // public const string ICON = "shirt_decor01_kanim";
                public const string ICON = "shirt_decor01_kanim";

                public const int CLOTH_MASS = 4;
                public const int FAB_TIME = 30;

                // public const int ADD_KG = 100;
                public const int ADD_KG = 128 * 100;
                public const int SPEED_PENALTY = 0;

                public static ci_ Info;
            }

            public static class MediumBackpack
            {
                public const string ID = nameof(MediumBackpack);

                // public const string ANISTR = "body_shirt_hot01_kanim";
                public const string ANISTR = "body_shirt_hot_shearling_kanim";

                // public const string ICON = "shirt_hot01_kanim";
                public const string ICON = "shirt_decor02_kanim";

                public const int CLOTH_MASS = 8;
                public const int FAB_TIME = 60;

                // public const int ADD_KG = 400;
                public const int ADD_KG = 2048 * 100;
                public const int SPEED_PENALTY = -2;

                public static ci_ Info;
            }

            public static class LargeBackpack
            {
                public const string ID = nameof(LargeBackpack);

                // public const string ANISTR = "body_shirt_cold01_kanim";
                public const string ANISTR = "body_shirt_decor01_kanim";

                // public const string ICON = "shirt_cold01_kanim";
                public const string ICON = "shirt_decor03_kanim";

                public const int CLOTH_MASS = 20;
                public const int FAB_TIME = 120;

                // public const int ADD_KG = 4800;
                public const int ADD_KG = 65536 * 100;
                public const int SPEED_PENALTY = -4;

                public static ci_ Info;
            }
        }
    }
}
