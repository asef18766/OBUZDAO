using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorList : MonoBehaviour
{
    public const int PAGE_CAPCITY = 8;
    private GunUnitUI[] gunUnitUIs;

    void Start()
    {
        // fillin gun units
        this.gunUnitUIs = GetComponentsInChildren<GunUnitUI>();
        Debug.Assert(this.gunUnitUIs.Length == ArmorList.PAGE_CAPCITY);
    }

    // TODO: refresh gun units UI
    public void Refresh()
    {

    }
}