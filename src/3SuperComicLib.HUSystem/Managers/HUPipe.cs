using System.Collections;
using UnityEngine;

namespace SuperComicLib.HUSystem
{
    using static Constants;
    [SkipSaveFileSerialization]
    public class HUPipe : KMonoBehaviour, IDisconnectable, IFirstFrameCallback, IHaveUtilityNetworkMgr, IUtilityNetworkItem, IHaveHUCell
    {
        [SerializeField]
        protected bool disconnected = true;
        private System.Action callback;

        public int HUCell { get; protected set; }
        public ushort NetworkID => manager.GetID(HUCell);

        protected override void OnSpawn()
        {
            HUCell = Grid.PosToCell(transform.GetPosition());
            hupipeSystem.AddToNetworks(HUCell, this, false);
            Connect();
        }

        public bool Connect()
        {
            BuildingHP com = GetComponent<BuildingHP>();
            if (com == null || com.HitPoints > 0)
            {
                disconnected = false;
                hupipeSystem.ForceRebuildNetworks();
            }
            return !disconnected;
        }
        public void Disconnect()
        {
            disconnected = true;
            hupipeSystem.ForceRebuildNetworks();
        }
        public IUtilityNetworkMgr GetNetworkManager() => hupipeSystem;
        public bool IsDisconnected() => disconnected;
        public void SetFirstFrameCallback(System.Action ffCb)
        {
            callback = ffCb;
            StartCoroutine(RunCallback());
        }

        private IEnumerator RunCallback()
        {
            yield return null;
            callback?.Invoke();
            yield return null;
            yield break;
        }
    }
}
