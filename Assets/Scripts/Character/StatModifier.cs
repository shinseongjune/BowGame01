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

    public Type type;

    public float value;

    public object source;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <param name="value">곱연산의 경우 30% 추가 -> 0.3으로 할 것.</param>
    public StatModifier(Type type, float value, object source = null)
    {
        this.type = type;
        this.value = value;
        this.source = source;
    }
}
