using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
    public class DataManager : Singleton<DataManager>
    {
        public List<EnemyData> gameDatas;
        private Dictionary<int, EnemyData> dataDictionary = new Dictionary<int, EnemyData>();

        protected override void Awake()
        {
            base.Awake();

            foreach (EnemyData data in gameDatas)
            if (!dataDictionary.ContainsKey(data._id))
            {
                dataDictionary.Add(data._id, data);
            }
        }

        public EnemyData GetData(int id)
        {
            if (dataDictionary.TryGetValue(id, out EnemyData data))
            {
                return data;
            }
            else
            {
                return null;
            }
        }
    }
}