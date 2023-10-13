using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTip : MonoBehaviour
{
    public string message;

    private void OnMouseEnter()
    {
        Cursor.visible = false;
        HoverTipManager.instance.SetandShowToolTip(message);
    }

    private void OnMouseExit()
    {
        Cursor.visible = true;
        HoverTipManager.instance.HideToolTip();
    }
}
