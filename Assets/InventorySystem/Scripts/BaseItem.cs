using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "baseItem", menuName = "Items/BaseItem")]
public class BaseItem : ScriptableObject
{
    public string id = Guid.NewGuid().ToString();
    public string description = string.Empty;
    public int maxStack = 50;
}
