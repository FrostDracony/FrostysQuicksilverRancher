using System.IO;
using MonomiPark.SlimeRancher.DataModel;
using SRML.SR.SaveSystem;
using UnityEngine;

namespace FrostysQuicksilverRancher.Components
{
	public class ElectricStorageModel : GadgetModel, ISerializableModel
	{
		int Version
		{
			get
			{
				return 0;
			}
		}

		public ElectricStorageModel(Gadget.Id ident, string siteId, Transform transform) : base(ident, siteId, transform)
		{
		}

		public void LoadData(BinaryReader reader)
		{
			basicChargeAmount = reader.ReadSingle();
			advancedChargeAmount = reader.ReadSingle();
		}

		public void WriteData(BinaryWriter writer)
		{
			writer.Write(Version);
			writer.Write(basicChargeAmount);
			writer.Write(advancedChargeAmount);
		}

		public float basicChargeAmount;

		public float advancedChargeAmount;
	}
}
