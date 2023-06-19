using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public TextMeshProUGUI questText;
    public SimpleTooltip tooltip;

    public void SetText(string text)
    {
        questText.text = text;
    }

    public void SetTooltip(string title, string description)
    {
        tooltip.infoLeft = "$" + title + "`\n" + description;
    }

    private void OnEnable()
    {
        tooltip = GetComponent<SimpleTooltip>();
    }

}
