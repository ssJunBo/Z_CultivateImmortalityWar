using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 超链接text
/// </summary>
[ExecuteInEditMode]
public class HyperlinkText : Text, IPointerClickHandler
{
    /// <summary>
    /// 超链接正则
    /// </summary>
    public static readonly Regex SHrefRegex = new Regex(@"<a[\u00A0 ]href=([^>\n\u00A0\s]+)>(.*?)(</a>)", RegexOptions.Singleline);

    /// <summary>
    /// 超链接需要的参数
    /// </summary>
    private readonly List<HyperlinkInfo> mLinkInfos = new List<HyperlinkInfo>();

    /// <summary>
    /// 文本构造器
    /// </summary>
    private readonly StringBuilder textBuilder = new StringBuilder();

    /// <summary>
    /// 解析完最终的文本
    /// </summary>
    private string outputText;

    public override string text
    {
        get => base.text;

        set
        {
            outputText = GetOutputText(value);
            base.text = value;
        }
    }

    /// <summary>
    /// 绘制模型，文字的顶点排列顺序  0   1 
    ///                          3   2  从左上角顺时针排列
    /// </summary>
    /// <param name="toFill"></param>
    protected override void OnPopulateMesh(VertexHelper toFill)
    {
        base.OnPopulateMesh(toFill);
        UIVertex vert = new UIVertex();

        // 处理超链接包围框
        foreach (var hrefInfo in mLinkInfos)
        {
            hrefInfo.boxes.Clear();
            if (hrefInfo.StartIndex >= toFill.currentVertCount)
            {
                continue;
            }

            // 将超链接里面的文本顶点索引坐标加入到包围框
            toFill.PopulateUIVertex(ref vert, hrefInfo.StartIndex);
            var pos = vert.position;
            var bounds = new Bounds(pos, Vector3.zero);
            for (int i = hrefInfo.StartIndex, m = hrefInfo.EndIndex; i < m; i++)
            {
                if (i >= toFill.currentVertCount)
                {
                    break;
                }

                toFill.PopulateUIVertex(ref vert, i);
                pos = vert.position;
                if (pos.x < bounds.min.x) // 换行重新添加包围框
                {
                    hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
                    bounds = new Bounds(pos, Vector3.zero);
                }
                else
                {
                    bounds.Encapsulate(pos); // 扩展包围框
                }
            }

            hrefInfo.boxes.Add(new Rect(bounds.min, bounds.size));
        }
    }

    private string GetOutputText(string outputText)
    {
        textBuilder.Length = 0;
        mLinkInfos.Clear();

        var indexText = 0;
        var matches = SHrefRegex.Matches(outputText);

        foreach (Match match in matches)
        {
            textBuilder.Append(outputText.Substring(indexText, match.Index - indexText));

            var group = match.Groups[1];
            var hrefInfo = new HyperlinkInfo
            {
                StartIndex = textBuilder.Length * 4, // 超链接里的文本起始顶点索引
                EndIndex = (textBuilder.Length + match.Groups[2].Length - 1) * 4 + 3,
                StrInfo = group.Value
            };
            mLinkInfos.Add(hrefInfo);

            textBuilder.Append(match.Groups[2].Value);
            indexText = match.Index + match.Length;
        }

        textBuilder.Append(outputText.Substring(indexText, outputText.Length - indexText));
        return textBuilder.ToString();
    }

    /// <summary>
    /// 超链接点击 事件
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (mLinkInfos == null || mLinkInfos.Count == 0) return;

        RectTransformUtility
            .ScreenPointToLocalPointInRectangle(rectTransform, eventData.position
                , eventData.pressEventCamera, out var lp);

        foreach (var hyperLinkObj in mLinkInfos)
        {
            if (hyperLinkObj.boxes == null || hyperLinkObj.boxes.Count == 0) continue;

            for (var i = 0; i < hyperLinkObj.boxes.Count; ++i)
            {
                if (hyperLinkObj.boxes[i].Contains(lp))
                {
                    Application.OpenURL(hyperLinkObj.StrInfo);
                    return;
                }
            }
        }
    }

    protected override void OnDestroy()
    {
        mLinkInfos.Clear();
    }

    private class HyperlinkInfo
    {
        /// <summary>
        /// 点击超链接 需要的信息
        /// </summary>
        public string StrInfo;

        /// <summary>
        /// 包围盒
        /// </summary>
        public readonly List<Rect> boxes = new List<Rect>();

        /// <summary>
        /// 开始索引
        /// </summary>
        public int StartIndex;

        /// <summary>
        /// 结束索引
        /// </summary>
        public int EndIndex;
    }
}