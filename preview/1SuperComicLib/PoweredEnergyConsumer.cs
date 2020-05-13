using System.Reflection;
using KSerialization;

namespace SuperComicLib.Oni
{
    [SerializationConfig(MemberSerialization.OptIn)]
    public class PoweredEnergyConsumer : EnergyConsumer
    {
        protected bool m_power;
        protected FieldInfo circuitID;

        public override bool IsPowered => m_power;

        protected override void OnPrefabInit() => 
            circuitID = typeof(EnergyConsumer).GetField($"<{nameof(CircuitID)}>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        protected override void OnSpawn()
        {
            base.OnSpawn();
            operational.SetActive(true);
        }

        public override void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
        {
            if (connection_status == CircuitManager.ConnectionStatus.Powered)
            {
                m_power = true;
                Overpwered(true);
                PlayCircuitSound("powered");
            }
            else 
            {
                m_power = false;
                if (connection_status == CircuitManager.ConnectionStatus.Unpowered)
                {
                    Overpwered(false);
                    circuitOverloadTime = 6f;
                    PlayCircuitSound("overdraw");
                }
            }
        }

        protected virtual void Overpwered(bool ison) { }
    }
}
