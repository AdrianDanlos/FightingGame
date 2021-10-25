using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [Header("Data")]
    public SMCore sMCore;

    [Header("UI")]
    [SerializeField] private UIMainMenu uIMainMenu;

    [Header("Backgrounds")]
    [SerializeField] private Image zone1;
    [SerializeField] private Image zone2;
    [SerializeField] private Image zone3;
    [SerializeField] private Image zone4;
    [SerializeField] private Image zone1Fade;
    [SerializeField] private Image zone2Fade;
    [SerializeField] private Image zone3Fade;
    [SerializeField] private Image zone4Fade;
}
