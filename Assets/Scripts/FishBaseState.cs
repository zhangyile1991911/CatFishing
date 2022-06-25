using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FishBaseState
{
    protected FishBase FishObj;
    public FishBaseState(FishBase cb)
    {
        FishObj = cb;
    }
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void OnUpdate();
}

//�������
public class FishBaseStatePatrol : FishBaseState
{
    public FishBaseStatePatrol(FishBase cb):base(cb)
    {
        FishObj = cb;
    }
    public override void EnterState()
    {
        Debug.Log("sid = " + FishObj.SID + " �������ε�״̬");
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.SID + " �˳����ε�״̬");
    }

    public override void OnUpdate()
    {
        if (FishObj.reachDestionation())
        {
            FishObj.choseNewDestionation();
        }
    }
}

//׼���Ϲ�
public class FishBaseStateReaching :FishBaseState
{
    public FishBaseStateReaching(FishBase cb) : base(cb)
    {
        FishObj = cb;
    }
    public override void EnterState()
    {
        Debug.Log("sid = "+FishObj.SID+" ���뿿���㹳��״̬");
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.SID + " �˳������㹳��״̬");
    }

    public override void OnUpdate()
    {
        if(FishObj.reachDestionation())
        {//����������
            FishObj.CurrentState = new FishBaseStateOnHook(FishObj);
        }
    }
}
//ҧ��
public class FishBaseStateOnHook : FishBaseState
{
    public FishBaseStateOnHook(FishBase cb) : base(cb)
    {
        FishObj = cb;
    }
    public override void EnterState()
    {
        Debug.Log("sid = " + FishObj.SID + " �����Ϲ���״̬");
        //todo ������������
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.SID + " �˳��Ϲ���״̬");
    }

    public override void OnUpdate()
    {

    }
}

//����
public class FishBaseStateDead:FishBaseState
{
    public FishBaseStateDead(FishBase cb) : base(cb)
    {
        FishObj = cb;
    }
    public override void EnterState()
    {
        Debug.Log("sid = " + FishObj.SID + " ����������״̬");
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.SID + " �˳�������״̬");
    }

    public override void OnUpdate()
    {

    }
}