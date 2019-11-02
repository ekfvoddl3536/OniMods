using System.Collections.Generic;

namespace SuperComicLib.HUSystem
{
    using System;
    using UnityEngine;
    using UnityEngine.UI;
    using static Constants;
    public class HUConsumer : KMonoBehaviour, ISaveLoadable, IHUConsumer, IEffectDescriptor
    {
        public static readonly Operational.Flag heatedFlag = new Operational.Flag("Heated", Operational.Flag.Type.Requirement);

        public CellOffset offset;
        [MyCmpReq]
        protected Building building;
        [MyCmpGet]
        protected Operational operational;
        public int huMinimum, huMaximum;

        protected virtual bool IsHeated
        {
            get => operational.GetFlag(heatedFlag);
            set => operational.SetFlag(heatedFlag, value);
        }

        public virtual bool IsConnected
        {
            get => operational.GetFlag(connectedFlag);
            set => operational.SetFlag(connectedFlag, value);
        }

        public virtual int HUMax => huMaximum;
        public virtual int HUMin => huMinimum;
        public virtual int HUCell { get; protected set; }

        public virtual ushort ID => manager.GetID(HUCell);

        public virtual string ObjectName => gameObject.name;

        public virtual string ToolTipText => "HU Consumed";

        protected override void OnSpawn()
        {
            HUCell = building.GetRotatedOffsetCell(offset);
            manager.Connect(this);

            IsHeated = false;
            IsConnected = false;
        }

        protected override void OnCleanUp()
        {
            IsConnected = IsHeated = false;
            manager.Disconnect(this);
        }

        public virtual List<Descriptor> GetDescriptors(BuildingDef def) => null;
        public virtual int ConsumedHU(int HUavailable, float dt)
        {
            IsHeated = true;
            return OnConsumedHU(Math.Min(HUMax, HUavailable), dt);
        }

        protected virtual int OnConsumedHU(int hu, float dt) => hu;

        public virtual void HUSim200ms(float dt) { }

        public virtual void SetConnectionState(bool connected) => IsConnected = connected;

        public virtual bool IfAddLabel(ICollection<SaveLoadRoot> roots, Vector2I min, Vector2I max)
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
            if (labelHandle == 1)
            {
                consumer_color = new Color32(250, 80, 130, 255);
                label.text = $"-{huMinimum}";
            }
            else
                label.text = $"-{huMaximum}";
            label.color = units.color = consumer_color;
            if (GetComponent<BuildingCellVisualizer>().GetOutputIcon() is Image img)
                img.color = consumer_color;
        }
    }
}
