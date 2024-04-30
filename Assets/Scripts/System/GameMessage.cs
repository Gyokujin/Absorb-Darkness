using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Game Message/Message")]
public class GameMessage : ScriptableObject
{
    public Text text;
    public string message;
}