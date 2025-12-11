// The MIT License (MIT)
// Copyright (c) 2019-2025. Super Comic (ekfvoddl3535@naver.com)

namespace AdvancedGenerators;

public class ConsumGasPowerGenerator : AdvancedEnergyGenerator
{
    public ElementConsumer Consumer;

    protected override void OnSpawn()
    {
        base.OnSpawn();

        Consumer.EnableConsumption(true);
    }

    protected override bool IsWorkable(bool isOperational)
    {
        Consumer.EnableConsumption(isOperational);
        return isOperational;
    }
}
