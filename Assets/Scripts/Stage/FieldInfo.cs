using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInfo : ScriptableObject
{
    public enum FieldType
    {
        Town, Field, BossField
    }

    public FieldType fieldType;
}