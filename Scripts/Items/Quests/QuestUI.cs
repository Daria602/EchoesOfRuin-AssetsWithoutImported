using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public TextMeshProUGUI questText;
    public SimpleTooltip tooltip;
    private string tooltipText;

    public void SetText(string text)
    {
        questText.text = text;
    }

    public void SetTooltip(string title, string description)
    {
        tooltipText = "$" + title + "`\n" + description;
    }

    private void OnEnable()
    {
        tooltip = GetComponent<SimpleTooltip>();
        tooltip.infoLeft = tooltipText;
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }

}
