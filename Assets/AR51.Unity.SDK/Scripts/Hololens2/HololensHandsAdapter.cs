#if AR51_Hololens2
namespace AR51.Unity.SDK.HandsAdapters
{
    public class HololensHandsAdapter
        : IHandsAdapter
    {
        public HandJointInfo[] GetRightJointInfos()
        {
            throw new System.NotImplementedException();
        }

        public HandJointInfo[] GetLeftJointInfos()
        {
            throw new System.NotImplementedException();
        }
    }
}
#endif