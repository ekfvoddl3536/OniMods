using System;
using System.Collections.Generic;
using UnityEngine;

namespace SupportPackage
{
	// Token: 0x02000006 RID: 6
	public class ConvElementSensorConfig : ConduitSensorConfig
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002A0D File Offset: 0x00000C0D
		protected override ConduitType ConduitType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002A10 File Offset: 0x00000C10
		public override BuildingDef CreateBuildingDef()
		{
			return base.CreateBuildingDef("SolidElementSensor", "gas_element_sensor_kanim", IN_Constants.SolidElementSensor.TMass, IN_Constants.SolidElementSensor.TMates, new List<LogicPorts.Port>
			{
				IN_Constants.SolidElementSensor.OUTPUT_PORT
			});
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002A3C File Offset: 0x00000C3C
		public override void DoPostConfigureComplete(GameObject go)
		{
			base.DoPostConfigureComplete(go);
			EntityTemplateExtensions.AddOrGet<Filterable>(go).filterElementState = 1;
			SoildConduitElementSensor soildConduitElementSensor = EntityTemplateExtensions.AddOrGet<SoildConduitElementSensor>(go);
			soildConduitElementSensor.manuallyControlled = false;
			soildConduitElementSensor.defaultState = false;
		}
	}
}
