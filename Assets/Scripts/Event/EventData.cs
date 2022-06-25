using System;
using UnityEngine;
/*
where T : struct �������Ͳ���T����̳���System.ValueType��
where T : class �������Ͳ���T�������������ͣ�Ҳ���ǲ��ܼ̳���System.ValueType��
where T : new() �������Ͳ���T������һ��ȱʡ�Ĺ��캯��
where T : NameOfClass �������Ͳ���T����̳���ĳ�����ʵ��ĳ���ӿڡ�
������Щ�޶��������ʹ�ã����磺 public class Point where T : class, IComparable, new()
*/
public abstract class EventData<T> : SharedValue<T> where T : class, new()
{
    public virtual void Clear()
    {

    }
}

public class StartFishingEvent : EventData<StartFishingEvent>
{

    public override void Clear()
    {
        base.Clear();
    }
}

public class FishOnHookEvent : EventData<FishOnHookEvent>
{
    public FishBase FishObject;
    public override void Clear()
    {
        FishObject = null;
    }
}
