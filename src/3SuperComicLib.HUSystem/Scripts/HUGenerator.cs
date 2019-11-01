using KSerialization;

namespace SuperComicLib.HUSystem
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using static Constants;
    [SerializationConfig(MemberSerialization.OptIn)]
    public class HUGenerator : KMonoBehaviour, ISaveLoadable, IHUGenerator
    {
        [MyCmpReq]
        protected Building building;
        [MyCmpReq]
        protected Operational operational;
        public int generateHU;
        public CellOffset offset;

        protected virtual bool IsConnected
        {
            get => operational.GetFlag(connectedFlag);
            set => operational.SetFlag(connectedFlag, value);
        }

        public virtual int HUCell { get; protected set; }
        public virtual ushort ID => manager.GetID(HUCell);

        public virtual string ObjectName => gameObject.name;
        public virtual string ToolTipText => "HU Generated";

        protected override void OnSpawn()
        {
            manager.Connect(this);
            HUCell = building.GetRotatedOffsetCell(offset);
            IsConnected = false;
        }

        protected override void OnCleanUp()
        {
            IsConnected = false;
            manager.Disconnect(this);
        }

        public virtual int GenerateHeat(float dt) => generateHU;
        public virtual void HUSim200ms(float dt) => IsConnected = ID != ushort.MaxValue;
        public bool IfAddLabel(ICollection<SaveLoadRoot> roots, Vector2I min, Vector2I max)
        {
            Vector2I xy = Grid.PosToXY(transform.GetPosition());
            if (min <= xy && xy <= max)
            {
                SaveLoadRoot root = GetComponent<SaveLoadRoot>();
                if (!roots.Contains(root))
                {
                    roots.Add(root);
                    return true;
                }
            }
            return false;
        }

        public virtual void UpdateVisualizer(LocText label, LocText units, Color32 generator_color, Color32 consumer_color, byte labelHandle)
        {
            label.text = $"+{generateHU}";
            label.color = units.color = generator_color;
            if (GetComponent<BuildingCellVisualizer>().GetOutputIcon() is Image img)
                img.color = generator_color;
        }
    }
}
