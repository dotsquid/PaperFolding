using UnityEngine;
using UnityEngine.EventSystems;

public class HeroTapController : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Hero _hero;

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        _hero.SetTarget(eventData.pointerCurrentRaycast.worldPosition);
    }
}
