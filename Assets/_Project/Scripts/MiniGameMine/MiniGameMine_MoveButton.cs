using UnityEngine;
using UnityEngine.EventSystems;

public class MiniGameMine_MoveButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public MiniGameMine miniGameMine;

    public void OnPointerDown(PointerEventData pointerEventData)
    {
        miniGameMine.Moving = true;
    }

    public void OnPointerUp(PointerEventData pointerEventData)
    {
        miniGameMine.Moving = false;
    }
}