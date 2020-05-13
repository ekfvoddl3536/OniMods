using SuperComicLib.Sandfile;
using System;
using System.Collections.Generic;
using System.IO;

namespace MoreSpecialGeysers
{
    public sealed class GeyserSandfileScanner : SandfileScanner
    {
        private static readonly Type simhashes_t = typeof(SimHashes);

        private Dictionary<string, float> floats;
        private string m_id;
        private SimHashes m_element;

        public GeyserSandfileScanner(string path) : base(path)
        {
            floats = new Dictionary<string, float>();
            m_id = Path.GetFileNameWithoutExtension(path);
        }

        public string ID => m_id;

        public SimHashes Element => m_element;

        public float GetFValue(string id)
        {
            floats.TryGetValue(id, out float res);
            return res;
        }

        protected override void UndetectString(string id, string val)
        {
            if (val.StartsWith("SimHashes.") && Enum.TryParse(val.Substring(10), true, out m_element))
                return;
            else if (float.TryParse(val, out float _res))
                floats.Add(id.ToLower(), _res);
            else
                Debug.LogWarning("Unknown data-type detected!");
        }

        protected override void DetectString(string id, string val)
        {
            id = id.ToLower();
            if (id == "id")
            {
                m_id = val;
                return;
            }
            if (id == "kanim")
            {
                if (!val.StartsWith("geyser_"))
                    val = "geyser_" + val;
                if (!val.EndsWith("_kanim"))
                    val += "_kanim";
            }
            if (id == "description")
                val = val.Replace("\\n", "\n");

            strDatas.Add(id, val);
        }

        public GeyserGenericConfig.GeyserPrefabParams Create() =>
            new GeyserGenericConfig.GeyserPrefabParams(
                GetStringValue(IDList.Anistr),
                (int)GetFValue(IDList.Width),
                (int)GetFValue(IDList.Height),
                    new GeyserConfigurator.GeyserType(
                        m_id,
                        m_element,
                        GetFValue(IDList.Temp),
                        GetFValue(IDList.MinRate),
                        GetFValue(IDList.MaxRate),
                        GetFValue(IDList.MaxPrsu),
                        GetFValue(IDList.MinActn),
                        GetFValue(IDList.MaxActn),
                        GetFValue(IDList.MinActPrnt),
                        GetFValue(IDList.MaxActPrnt),
                        GetFValue(IDList.MinYear),
                        GetFValue(IDList.MaxYear),
                        GetFValue(IDList.MinYearPrnt),
                        GetFValue(IDList.MaxYearPrnt)
                        ));

        public static GeyserSandfileScanner Load(string path)
        {
            GeyserSandfileScanner scanner = new GeyserSandfileScanner(path);
            scanner.Start();
            return scanner;
        }
    }
}
