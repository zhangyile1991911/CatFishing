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

//随便乱游
public class FishBaseStatePatrol : FishBaseState
{
    public FishBaseStatePatrol(FishBase cb):base(cb)
    {
        FishObj = cb;
    }
    public override void EnterState()
    {
        Debug.Log("sid = " + FishObj.name + " 进入乱游的状态");
        FishObj.PlayAnimation(FishBase.FishAnimation.Move);
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.name + " 退出乱游的状态");
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

//准备上钩
public class FishBaseStateReaching :FishBaseState
{
    public FishBaseStateReaching(FishBase cb) : base(cb)
    {
        FishObj = cb;
    }
    public override void EnterState()
    {
        Debug.Log("sid = "+FishObj.name + " 进入靠近鱼钩的状态");
        FishObj.PlayAnimation(FishBase.FishAnimation.Move);
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.name + " 退出靠近鱼钩的状态");
    }

    public override void OnUpdate()
    {
        if(FishObj.reachDestionation())
        {//靠近钩子了
            FishObj.CurrentState = new FishBaseStateOnHook(FishObj);
        }
    }
    public override bool CanApply(FishBaseState new_state)
    {
        return true;
    }
}
//咬钩
public class FishBaseStateOnHook : FishBaseState
{
    public FishBaseStateOnHook(FishBase cb) : base(cb)
    {
        FishObj = cb;
    }
    public override void EnterState()
    {
        Debug.Log("sid = " + FishObj.name + " 进入上钩的状态");
        EventDispatch.Dispatch(EventID.FishOnHook, 0);
        //todo 播放挣扎动画
        FishObj.PlayAnimation(FishBase.FishAnimation.Fighting);
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.name + " 退出上钩的状态");
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

//死亡
public class FishBaseStateDead:FishBaseState
{
    public FishBaseStateDead(FishBase cb) : base(cb)
    {
        FishObj = cb;
    }
    public override void EnterState()
    {
        Debug.Log("sid = " + FishObj.SID + " 进入死亡的状态");
        FishObj.PlayAnimation(FishBase.FishAnimation.Dead);
        FishObj.RemoveFromRVO();
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.SID + " 退出死亡的状态");
    }

    public override void OnUpdate()
    {

    }
    public override bool CanApply(FishBaseState new_state)
    {
        return false;
    }
}