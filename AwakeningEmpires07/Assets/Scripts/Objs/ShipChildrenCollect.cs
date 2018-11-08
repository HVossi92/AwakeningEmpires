using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipChildrenCollect : MonoBehaviour
{

    public List<GameObject> ShipChildrenList(GameObject shipHolder)
    {
        GameObject shipChild;
        List<GameObject> shipChildren = new List<GameObject>();

        // loop trhough Children and put them into a List (direclty moving them into another parent will change the childCount and fuck up
        for (int i = 0; i < shipHolder.transform.childCount; i++)
        {
            shipChild = shipHolder.transform.GetChild(i).gameObject;
            shipChildren.Add(shipChild);
        }
        return shipChildren;
    }

}