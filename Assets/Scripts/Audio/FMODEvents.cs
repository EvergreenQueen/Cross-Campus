using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Button sound")]
    [field: SerializeField] public EventReference buttonSound { get; private set; }

    /*[field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; private set; }*/

    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }
}
