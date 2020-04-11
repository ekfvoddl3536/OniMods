using System;

namespace Afterlife
{
	// Token: 0x02000007 RID: 7
	internal static class Funcs
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000021FC File Offset: 0x000003FC
		public static float GetFeedingTime(Worker worker, Edible ed)
		{
			float num = ed.Calories * Constants.feedingTime;
			if (worker != null)
			{
				BingeEatChore.StatesInstance smi = StateMachineControllerExtensions.GetSMI<BingeEatChore.StatesInstance>(worker);
				if (smi != null && smi.IsBingeEating())
				{
					num /= 2f;
				}
			}
			return num;
		}
	}
}
