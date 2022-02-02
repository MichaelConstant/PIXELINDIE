using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter_Base : MonoBehaviour
{
    protected PlayerControl Player;
    protected void Awake()
    {
        Player = GetComponentInParent<PlayerControl>();
    }
}