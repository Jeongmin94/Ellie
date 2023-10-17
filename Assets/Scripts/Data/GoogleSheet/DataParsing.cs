using UnityEngine;

public abstract class DataParsing : ScriptableObject
{
    [Header("스프레드시트 전체 주소")]
    public string url;

    [Header("스프레드시트 파싱 범위 (시작 열, 종료 열")]
    public string startRow;
    public string endRow;

    public abstract void Parse();
}
