using SRML.SR.SaveSystem;
using SRML.SR.SaveSystem.Data;

namespace FrostysQuicksilverRancher.Components
{
    class PlortUndisappearifier : SRBehaviour, ExtendedData.Participant
    {
        public void Awake()
        {
            QuicksilverPlortCollector component = base.GetComponent<QuicksilverPlortCollector>();
            if (component)
            {
                UnityEngine.Object.Destroy(component);
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
