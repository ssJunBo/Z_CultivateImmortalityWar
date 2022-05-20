using Common;

namespace Data
{
    /// <summary>
    /// 个人用户数据
    /// </summary>
    public class PersonDetailData
    {
        // 玩家id
        public int id;
        public string name;
        public int level;
        public Profession profession; // 职业
        public EquipData equipData; // 身上装备信息
    }
}