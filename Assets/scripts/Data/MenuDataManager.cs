using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDataManager : MonoBehaviour
{
    // Data management
    public ManageSaves manageSaves;
    // loadear data aqui en vez de en el boton
    public void Awake()
    {
        manageSaves.LoadMenuData();
    }

}
