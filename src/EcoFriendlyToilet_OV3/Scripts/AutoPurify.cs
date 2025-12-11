// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

using KSerialization;
using UnityEngine;

namespace EcoFriendlyToilet;

using static CircuitManager.ConnectionStatus;
using static GlobalConsts.AutoPurifyWashsink;
[SerializationConfig(MemberSerialization.OptIn)]
public class AutoPurify : EnergyConsumer, IEnergyConsumer, ISim1000ms, ICircuitConnected
{
    protected ElementConverter converter_;
    protected ushort circuitID_;
    protected bool power_;
    protected int powerCell_;
    protected CircuitManager.ConnectionStatus prevConnectionStatus_;

    float IEnergyConsumer.WattsNeededWhenActive => USE_POWER;

    bool IEnergyConsumer.IsConnected => circuitID_ != ushort.MaxValue;

    bool IEnergyConsumer.IsPowered => power_;

    int ICircuitConnected.PowerCell => powerCell_;

    protected override void OnPrefabInit()
    {
        converter_ = GetComponent<ElementConverter>();
        BaseWattageRating = USE_POWER;
    }

    protected override void OnSpawn()
    {
        Components.EnergyConsumers.Add(this);

        powerCell_ = GetComponent<Building>().GetPowerInputCell();

        Game.Instance.circuitManager.Connect(this);
        Game.Instance.energySim.AddEnergyConsumer(this);

        operational.SetActive(true);
    }

    public override void SetConnectionStatus(CircuitManager.ConnectionStatus connection_status)
    {
        bool res = connection_status == Powered;
        if (res)
        {
            if (prevConnectionStatus_ != Powered)
                PlayCircuitSound("powered");
        }
        else if ((connection_status != 0) ^ (prevConnectionStatus_ == Unpowered))
        {
            circuitOverloadTime = 6f;
            PlayCircuitSound("overdraw");
        }

        prevConnectionStatus_ = connection_status;

        SetPowerState(res);
    }

    protected virtual void SetPowerState(bool flag)
    {
        var conv = converter_;
        if (circuitID_ == ushort.MaxValue || flag == false)
        {
            conv.consumedElements[0].MassConsumptionRate =
                conv.outputElements[0].massGenerationRate = UNPOWERED_CONV;

            conv.consumedElements[1].MassConsumptionRate =
                conv.outputElements[1].massGenerationRate = UNPOWERED_FCON;
        }
        else
        {
            conv.consumedElements[0].MassConsumptionRate =
                conv.outputElements[0].massGenerationRate = POWERED_CONV;

            conv.consumedElements[1].MassConsumptionRate =
                conv.outputElements[1].massGenerationRate = POWERED_FCON;
        }

        power_ = flag;
    }

    public override void EnergySim200ms(float dt)
    {
        ushort id = Game.Instance.circuitManager.GetCircuitID(this);

        if (id == ushort.MaxValue)
            SetPowerState(false);

        circuitID_ = id;

        circuitOverloadTime = Mathf.Max(0f, circuitOverloadTime - dt);
    }

    public void Sim1000ms(float dt)
    {
        if (operational.IsActive ^ converter_.HasEnoughMassToStartConverting())
            operational.SetActive(!operational.IsActive, false);
    }
}
