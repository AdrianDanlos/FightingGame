using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public ManageSaves manageSaves;
    
    public void GenerateData()
    {
        manageSaves.GenerateInitialValues();
    }
}
