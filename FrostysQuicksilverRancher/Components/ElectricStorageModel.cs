using System.IO;
using MonomiPark.SlimeRancher.DataModel;
using SRML.SR.SaveSystem;
using UnityEngine;

namespace FrostysQuicksilverRancher.Components
{
	public class ElectricStorageModel : GadgetModel, ISerializableModel
	{
		private int Version => 0;

		public ElectricStorageModel(Gadget.Id ident, string siteId, Transform transform) : base(ident, siteId, transform)
		{
		}

		public void LoadData(BinaryReader reader)
		{
			reader.ReadInt32();
			chargeAmount = reader.ReadSingle();
		}

		public void WriteData(BinaryWriter writer)
		{
			writer.Write(Version);
			writer.Write(chargeAmount);
		}

		public float chargeAmount;
	}
}
