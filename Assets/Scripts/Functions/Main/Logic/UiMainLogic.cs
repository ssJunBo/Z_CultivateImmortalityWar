using bFrameWork.Game.UIFrame.Base;
using Common;
using Data;
using Managers.Model;

namespace Functions.Main.Logic
{
    public class UiMainLogic : UiLogicBase
    {
        protected override string Path => "Prefabs/Dialogs/MainDialog";
        protected override EUiID UiId => EUiID.Main;
        protected override EUiLayer UiLayer => EUiLayer.Low_2D;

        public CModelPlay model { get; }

        public PersonDetailData PersonDetailInfo { get; private set; }
        public UiMainLogic(CModelPlay model)
        {
            this.model = model;
        }

        public override void Open()
        {
            base.Open();
            GeneratePersonDetailData();
        }

        // TODO 后面换成网络请求 获得数据
        private void GeneratePersonDetailData()
        {
            PersonDetailInfo = new PersonDetailData
            {
                id = 1,
                name = "good study",
                equipData = new EquipData(),
                level = 10,
                profession = Profession.Swordsman,// 剑客
            };
        }
    }
}