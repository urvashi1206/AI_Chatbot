using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTip : MonoBehaviour
{
    public string message;

    private void OnMouseEnter()
    {
        HoverTipManager.instance.SetandShowToolTip(message);
    }

    private void OnMouseExit()
    {
        HoverTipManager.instance.HideToolTip();
    }
}
