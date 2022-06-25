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

    public abstract bool CanApply(FishBaseState new_state);
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
        Debug.Log("sid = " + FishObj.name + " �������ε�״̬");
        FishObj.PlayAnimation(FishBase.FishAnimation.Move);
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.name + " �˳����ε�״̬");
    }

    public override bool CanApply(FishBaseState new_state)
    {
        return true;
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
        Debug.Log("sid = "+FishObj.name + " ���뿿���㹳��״̬");
        FishObj.PlayAnimation(FishBase.FishAnimation.Move);
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.name + " �˳������㹳��״̬");
    }

    public override void OnUpdate()
    {
        if(FishObj.reachDestionation())
        {//����������
            FishObj.CurrentState = new FishBaseStateOnHook(FishObj);
        }
    }
    public override bool CanApply(FishBaseState new_state)
    {
        return true;
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
        Debug.Log("sid = " + FishObj.name + " �����Ϲ���״̬");
        EventDispatch.Dispatch(EventID.FishOnHook, 0);
        //todo ������������
        FishObj.PlayAnimation(FishBase.FishAnimation.Fighting);
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.name + " �˳��Ϲ���״̬");
    }

    public override void OnUpdate()
    {

    }
    public override bool CanApply(FishBaseState new_state)
    {
        if(new_state.GetType() ==typeof(FishBaseStatePatrol))
        {
            return true;
        }
        if (new_state.GetType() == typeof(FishBaseStateDead))
        {
            return true;
        }

        return false;
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
        FishObj.PlayAnimation(FishBase.FishAnimation.Dead);
        FishObj.RemoveFromRVO();
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.SID + " �˳�������״̬");
    }

    public override void OnUpdate()
    {

    }
    public override bool CanApply(FishBaseState new_state)
    {
        return false;
    }
}