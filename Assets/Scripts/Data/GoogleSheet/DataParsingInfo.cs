using UnityEngine;

public abstract class DataParsingInfo : ScriptableObject
{
    [Header("Google SpreadSheets URL")]
    public string url;

    [Header("Google SpreadSheets Parsing Range")]
    public string startRow;
    public string endRow;

    public abstract void Parse();
}
