using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowScript : MonoBehaviour
{
    Transform fighterOneTransform;
    Transform fighterTwoTransform;
    string parentFighterName;
    enum FighterObjectNames
    {
        FighterOne,
        FighterTwo
    }

    void Start()
    {
        parentFighterName = transform.parent.parent.parent.name;
        fighterOneTransform = GameObject.Find("FighterOne").transform;
        fighterTwoTransform = GameObject.Find("FighterTwo").transform;
    }

    void Update()
    {
        if (parentFighterName == FighterObjectNames.FighterOne.ToString()) setPositionOfShadows(fighterOneTransform);
        else setPositionOfShadows(fighterTwoTransform);
    }

    private void setPositionOfShadows(Transform fighterTransform)
    {
        // here we force the position of the current object to have the same x as the fighter
        transform.position = new Vector3(fighterTransform.position.x, transform.position.y, transform.position.z);
    }
}
