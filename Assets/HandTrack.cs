using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class HandTrack : MonoBehaviour
{
    public TwoBoneIKConstraint leftHandIk;
    public TwoBoneIKConstraint rightHandIK;
    public RigBuilder rigBuilder;
    public Transform rightHandTarget;
    public Transform leftHandTarget;

    public Vector3 weaponOriginalPosition;
    public void Awake()
    {
        weaponOriginalPosition = transform.localPosition;
    }
    /*public void ResetHand()
    {
        leftHandIk.data.target = rightHandTarget;
        rightHandIK.data.target = leftHandTarget;
        rigBuilder.Build();
    }*/


}
