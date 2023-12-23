using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoadingTipsData", menuName = "GameData List/LoadingTipsData")]
public class LoadingTipsData : ScriptableObject
{
    public List<string> tips;
}