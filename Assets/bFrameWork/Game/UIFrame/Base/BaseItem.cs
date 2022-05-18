using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseItem : MonoBehaviour {

    //所有的Button 
    protected List<Button> m_AllButton = new List<Button>();

    public virtual void OnAwake(params object[] paraList) { }

    public virtual void OnStart(params object[] paraList) { }

    public virtual void OnDisable() { }

    public virtual void OnUpdate() { }

    /// <summary>
    /// 添加Button事件监听 
    /// </summary>
    /// <param name="btn"></param>
    /// <param name="action"></param>
    public void AddButtonClickListener(Button btn, UnityEngine.Events.UnityAction action)
    {
        if (btn != null)
        {
            if (!m_AllButton.Contains(btn))
            {
                m_AllButton.Add(btn);
            }
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(action);
            btn.onClick.AddListener(BtnPlaySound);
        }
    }

    public virtual void OnClose()
    {
        RemoveAllButtonListener();
        m_AllButton.Clear();
    }


    /// <summary>
    /// 播放btn声音
    /// </summary>
    void BtnPlaySound() { }

    /// <summary>
    /// 移除所有的Button事件
    /// </summary>
    public void RemoveAllButtonListener()
    {
        foreach (Button btn in m_AllButton)
        {
            btn.onClick.RemoveAllListeners();
        }
    }

}
