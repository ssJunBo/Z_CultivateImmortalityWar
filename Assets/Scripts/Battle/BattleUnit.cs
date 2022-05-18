using System;
using UnityEngine;

namespace Battle
{
    public abstract class BattleUnit : MonoBehaviour
    {
        #region Attributes
        [NonSerialized] public int Hp;
        [NonSerialized] public int Attack;
        [NonSerialized] public int Def;
        [NonSerialized] public int Name;
        #endregion
    }
}
