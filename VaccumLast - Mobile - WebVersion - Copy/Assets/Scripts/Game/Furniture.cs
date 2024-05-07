using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{
    public enum TypeOfFutniture
    {
        Pos1by1,
        Pos2by1,
        Pos2by2
    }
    [SerializeField] private TypeOfFutniture Typefutniture;
    public TypeOfFutniture GiveTypeOfFurnip()
    {
        return Typefutniture;
    }
}
