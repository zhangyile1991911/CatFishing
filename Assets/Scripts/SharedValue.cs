using System;

public abstract class SharedValue<T> where T : new()
{
    private static T t;
    /// <summary>
    /// ʹ�þ�̬����
    /// </summary>
    public static T sData
    {
        get
        {
            if (t == null)
            {
                t = new T();
            }
            return t;
        }
    }
}