using UnityEngine;

namespace SupportPackage
{
    public class InOneRefrigerator : KMonoBehaviour
    {
        private const float SIM_TEMP = 253.15f;
        // private const float HEAT_CAPACITY = 800f;
        // private const float CONDUCITIY = 2000f;

        private Storage storage;

        protected override void OnSpawn()
        {
            Subscribe((int)GameHashes.OnStorageChange, OnStorageChanged_CB);

            var st = GetComponent<ComplexFabricator>().outStorage;
            storage = st;

            // always
            var list = st.items;
            for (int i = list.Count; --i >= 0;)
                list[i].GetComponent<PrimaryElement>().Temperature = SIM_TEMP;
        }

        private void OnStorageChanged_CB(object data)
        {
            GameObject go = (GameObject)data;

            var pic = go.GetComponent<Pickupable>();
            if (!pic)
                return;

            // reg, unreg 공통
            if (pic.storage == storage)
                go.GetComponent<PrimaryElement>().Temperature = SIM_TEMP;
        }

        protected override void OnCleanUp() =>
            Unsubscribe((int)GameHashes.OnStorageChange, OnStorageChanged_CB);
    }
}
