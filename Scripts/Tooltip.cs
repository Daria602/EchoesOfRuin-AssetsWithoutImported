using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI m_tooltipText;
    public RectTransform m_background;
    [SerializeField]
    Camera uiCamera;

    private static Tooltip instance;

    private void Awake()
    {
        instance = this;
        m_background = transform.Find("TooltipBackground").GetComponent<RectTransform>();
        Debug.Log(m_background != null);
        m_tooltipText = transform.Find("TooltipText").GetComponent<TextMeshProUGUI>();
        Debug.Log(m_tooltipText != null);
        ShowTooltip("Some random stuff\n is going on here...");
    }
    private void ShowTooltip(string tooltipString)
    {
        gameObject.SetActive(true);
        m_tooltipText.text = tooltipString;
        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(m_tooltipText.preferredWidth + textPaddingSize * 2f, m_tooltipText.preferredHeight + textPaddingSize * 2f);
        m_background.sizeDelta = backgroundSize;
        

    }

    private void HideTooltip()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCamera, out localPoint);
        transform.localPosition = localPoint;
    }

    public static void ShowTooltip_Static(string tooltipString)
    {
        instance.ShowTooltip(tooltipString);
    }

    public static void HideTooltip_Static()
    {
        instance.HideTooltip();
    }
}
