using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuSpawner : MonoBehaviour {

    public static RadialMenuSpawner instance;
    public RadialMenu menuPrefab;
    //public GameObject menuRef;

    void Awake()
    {
        instance = this;
    }

    public void SpawnMenu(InteractUI obj)
    {
        RadialMenu newMenu = Instantiate(menuPrefab);
        newMenu.transform.SetParent(transform, false);
        newMenu.SpawnButtons(obj);
       // menuRef = newMenu.gameObject;
    }
}
