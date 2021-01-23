using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class MapPointController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public MapPointClass MapPoint;

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        GameObject prefab = GetComponentInParent<MapScript>().Prefab_MapPointTitle;
        var obj = Instantiate(prefab, GetComponentInParent<MapScript>().Destiny_WindowInfo,true);
        obj.GetComponent<MapPointTitle>().SetTitle(MapPoint.namePoint);
        Vector2 newPosition = new Vector2(transform.position.x, transform.position.y+gameObject.GetComponent<RectTransform>().rect.height);
        obj.transform.position = newPosition;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        var objs = GetComponentInParent<MapScript>().Destiny_WindowInfo.GetComponentsInChildren<MapPointTitle>();
        for(int i=0;i<objs.Length;i++)
        {
            Destroy(objs[i].gameObject);
        }
    }

    public abstract void OpenWindow();
    protected void Awake()
    {
        MapPoint.idRegion = GetComponentInParent<Regions>().regions.FindIndex(x => x == GetComponentInParent<Region>().gameObject);
    }
}