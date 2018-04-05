using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUI : MonoBehaviour
{
    public RadialMenu menu;

    [System.Serializable]
    public class Action
    {
        public Color color;
        public Sprite sprite;
        public string title;

    }

    private RadialMenu obj;
    public Action[] options;

    void Start()
    {
        RadialMenuSpawner.instance.SpawnMenu(this);
        RadialMenuSpawner.instance.gameObject.SetActive(true);


        //RadialMenuSpawner.instance.menuRef.SetActive(true);
    }

    // void Update()
    //{



    /*

    if (Input.GetButton("SkillSelect"))
    {
        RadialMenuSpawner.instance.gameObject.SetActive(true);

    }
    else
    {
        //if (RadialMenuSpawner.instance.gameObject.GetComponent<RadialMenu>().selected)
        //{
        //    Debug.Log(RadialMenuSpawner.instance.gameObject.GetComponent<RadialMenu>().selected.title + "was selected");
        //}
        RadialMenuSpawner.instance.gameObject.SetActive(false);
    }

*/

}

