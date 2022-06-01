using UnityEngine;

public class StatModifier
{
    public enum Type
    {
        BaseFlat,
        BaseMult,
        TotalFlat,
        TotalMultA,
        TotalMultB,
        TotalMultC,
        TotalMultD,
    }

    public string name;
    public Type type;

    public float value;

    public GameObject source;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="value">곱연산의 경우 30% 추가 -> 0.3으로 할 것.</param>
    public StatModifier(string name, Type type, float value, GameObject source = null)
    {
        this.name = name;
        this.type = type;
        this.value = value;
        this.source = source;
    }
}
