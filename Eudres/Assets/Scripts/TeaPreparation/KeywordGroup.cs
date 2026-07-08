using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KeywordGroup", menuName = "Scriptable Objects/KeywordGroup")]
public class KeywordGroup : ScriptableObject
{
    public List<Keyword> keywords;
}
