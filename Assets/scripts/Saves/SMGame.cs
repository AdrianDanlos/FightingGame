using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SMGame : MonoBehaviour
{
    [Header("Data")]
    public SMCore sMCore;
    private string savePath;
    public Skills skills;
    public LevelDB levelDB;
    private GameData gameData;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/save.mame"; // it can have whatever extension name
    }


}
