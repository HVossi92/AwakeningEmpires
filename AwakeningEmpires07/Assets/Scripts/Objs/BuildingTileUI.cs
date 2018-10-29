using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTileUI : MonoBehaviour {

    private ClickTile target;

    private void Awake()
    {
        SetUiComponents(false);
    }

    public void SetTarget(ClickTile _target)
    {
        if (_target.name == "TileConstructionPlayer1" || _target.name == "TileConstructionPlayer2")
        {
            this.target = _target;
            Vector3 newPosition = new Vector3(target.transform.position.x - 0.5f, target.transform.position.y + 0.5f, target.transform.position.z);
            this.transform.position = newPosition;

            SetUiComponents(true);
        }
    }    

    public void Hide()
    {
        SetUiComponents(false);
    }

    private void SetUiComponents(bool myBool)
    {
        Component[] renderer = this.GetComponentsInChildren<Renderer>();
        Component[] collider = this.GetComponentsInChildren<Collider>();
        foreach (Component c in renderer)
        {
            Renderer rend = (Renderer)c;
            rend.enabled = myBool;
        }
        foreach (Component d in collider)
        {
            Collider coll = (Collider)d;
            coll.enabled = myBool;
        }
    }

    private void Update()
    {
       // print(this.transform.position);
    }

}
