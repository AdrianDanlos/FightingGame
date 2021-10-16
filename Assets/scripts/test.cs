using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public ManageSaves manageSaves;

    private void Start()
    {
        System.Random randomN = new System.Random();
        HashSet<int> numbers = new HashSet<int>();
        while (numbers.Count < 2)
        {
            numbers.Add(randomN.Next(1, 4));
        }

        Debug.Log(numbers.Contains(1));
        Debug.Log(numbers.Contains(2));
        Debug.Log(numbers.Contains(3));
        Debug.Log(numbers.Contains(4));
    }

    public void GenerateData()
    {
        manageSaves.GenerateInitialValues();
    }

    void LevelXPSetUp()
    {
        int[] toLevelUp = new int[30];

        for (int lv = 1; lv < toLevelUp.Length; lv++)
        {
            toLevelUp[lv] = (int)(Mathf.Floor(4 * (Mathf.Pow(lv, 0.4f))));
            Debug.Log(toLevelUp[lv]);
        }
    }
}
