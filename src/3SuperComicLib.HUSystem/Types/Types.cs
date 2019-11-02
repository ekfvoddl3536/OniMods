using System.Collections.Generic;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
    public interface IHaveHUCell
    {
        int HUCell { get; }
    }

    public interface IHUOverlayUpdate : IHaveHUCell
    {
        bool IfAddLabel(ICollection<SaveLoadRoot> roots, Vector2I min, Vector2I max);
        void UpdateVisualizer(LocText label, LocText units, Color32 generator_color, Color32 consumer_color, byte labelHandle);
        string ObjectName { get; }
        string ToolTipText { get; }
    }

    public interface IHUSim200ms
    {
        void HUSim200ms(float dt);
    }

    public interface IHUGenerator : IHUSim200ms, IHUOverlayUpdate
    {
        int GenerateHeat(float dt);
    }

    public interface IHUConsumer : IHUSim200ms, IHUOverlayUpdate
    {
        int HUMax { get; }
        int HUMin { get; }

        int ConsumedHU(int HUavailable, float dt);
    }
}
