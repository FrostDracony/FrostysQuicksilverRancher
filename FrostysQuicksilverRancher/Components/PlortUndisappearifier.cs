using SRML.SR.SaveSystem;
using SRML.SR.SaveSystem.Data;

namespace FrostysQuicksilverRancher.Components
{
	class PlortUndisappearifier : SRBehaviour, ExtendedData.Participant
	{
		// Token: 0x06000029 RID: 41 RVA: 0x00002E1C File Offset: 0x0000101C
		public void Awake()
		{
			QuicksilverPlortCollector component = GetComponent<QuicksilverPlortCollector>();
			bool flag = component;
			if (flag)
			{
				Destroy(component);
			}
		}

		public void ReadData(CompoundDataPiece piece)
		{
		}

		public void WriteData(CompoundDataPiece piece)
		{
		}
	}
}
