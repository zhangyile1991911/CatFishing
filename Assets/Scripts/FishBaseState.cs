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

//随便乱游
public class FishBaseStatePatrol : FishBaseState
{
    public FishBaseStatePatrol(FishBase cb):base(cb)
    {
        FishObj = cb;
    }
    public override void EnterState()
    {
        Debug.Log("sid = " + FishObj.SID + " 进入乱游的状态");
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.SID + " 退出乱游的状态");
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
        Debug.Log("sid = "+FishObj.SID+" 进入靠近鱼钩的状态");
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.SID + " 退出靠近鱼钩的状态");
    }

    public override void OnUpdate()
    {
        if(FishObj.reachDestionation())
        {//靠近钩子了
            FishObj.CurrentState = new FishBaseStateOnHook(FishObj);
        }
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
        Debug.Log("sid = " + FishObj.SID + " 进入上钩的状态");
        //todo 播放挣扎动画
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.SID + " 退出上钩的状态");
    }

    public override void OnUpdate()
    {

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
    }

    public override void ExitState()
    {
        Debug.Log("sid = " + FishObj.SID + " 退出死亡的状态");
    }

    public override void OnUpdate()
    {

    }
}