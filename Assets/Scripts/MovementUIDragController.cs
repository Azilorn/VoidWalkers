using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum ButtonType {Up, Right, Down, Left, A, B }
public class MovementUIDragController : MonoBehaviour
{
    public ButtonType buttonType;
    private bool clicked;

    //private void Update()
    //{
    //    if (clicked) {
    //        GetInput();
    //    }
    //}

    //public void GetInput()
    //{
    //    switch (buttonType)
    //    {
    //        case ButtonType.Up:
    //            CharacterMovement.MoveUp();
    //            break;
    //        case ButtonType.Right:
    //            CharacterMovement.MoveRight();
    //            break;
    //        case ButtonType.Down:
    //            CharacterMovement.MoveDown();
    //            break;
    //        case ButtonType.Left:
    //            CharacterMovement.MoveLeft();
    //            break;
    //        case ButtonType.A:
    //            break;
    //        case ButtonType.B:
    //            break;
    //    }
    //}
    
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    clicked = true;
    //}

    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    clicked = false;
    //}
}