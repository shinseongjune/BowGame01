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
    /// <param name="value">�������� ��� 30% �߰� -> 0.3���� �� ��.</param>
    public StatModifier(Type type, float value, object source = null)
    {
        this.type = type;
        this.value = value;
        this.source = source;
    }
}
