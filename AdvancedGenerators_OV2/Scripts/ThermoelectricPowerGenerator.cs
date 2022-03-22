#region LICENSE
/*
MIT License

Copyright (c) 2022. Super Comic (ekfvoddl3535@naver.com)

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion
namespace AdvancedGenerators
{
    public sealed class ThermoelectricPowerGenerator : AdvancedEnergyGenerator
    {
        public override void EnergySim200ms(float dt)
        {
            base.EnergySim200ms(dt);

            var op = operational;
            op.SetFlag(wireConnectedFlag, CircuitID != ushort.MaxValue);
            op.SetActive(op.IsOperational);

            if (HasMeter)
                meter.SetPositionPercent(op.IsActive ? 1 : 0);

            if (op.IsOperational)
            {
                GenerateJoules(WattageRating * dt, true);

                var db = Db.Get();
                selectable.SetStatusItem(db.StatusItemCategories.Power, db.BuildingStatusItems.Wattage, this);
            }
        }
    }
}
