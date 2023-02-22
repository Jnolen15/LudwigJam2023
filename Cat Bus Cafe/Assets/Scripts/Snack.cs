using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snack : MonoBehaviour
{
    public enum Flavor
    {
        Nothing,
        Green, // Catnip
        Red, // Strawberry?
        Blend // Blend
    }
    public Flavor flavor;

    public bool hasMilk;
    public bool hasBoba;

    public enum SnackType
    {
        Nothing,
        Square,
        Pop,
        Ring
    }
    public SnackType snackType;
}
