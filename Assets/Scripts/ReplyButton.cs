using System.Data.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ReplyButton : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData ev)
    {
        ev.selectedObject = gameObject;
        DialogBox db = GetComponentInParent<DialogBox>();
        db.selected = null;
    }
    
    public void InformParentOfClick()
    {
        DialogBox db = GetComponentInParent<DialogBox>();
        db.buttonClicked(GetComponent<Button>());
        if (GameManager.instance.GetFlag("IN_BATTLE"))
            Battlefield.instance.Advance();
    }
}
