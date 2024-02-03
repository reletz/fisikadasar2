using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public abstract class BaseDialogue : ScriptableObject
{
    public abstract void Enter();
}
