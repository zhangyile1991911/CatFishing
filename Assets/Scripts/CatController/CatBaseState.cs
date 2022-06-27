using UnityEngine;

public abstract class CatBaseState
{
    protected CatBase CatObj;
    public CatBaseState(CatBase cb)
    {
        CatObj = cb;
    }
    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void DrawBackRob();

    public abstract void ThrowRob();

    public abstract void HoldOnRob();

    public abstract void ReleaseRob();

    public abstract void MoveForward();

    public abstract void MoveBackward();
}


public class CatBaseStateIdle : CatBaseState
{
    public CatBaseStateIdle(CatBase cb) : base(cb)
    {

    }
    public override void EnterState()
    {
        Debug.Log("进入CatBaseStateIdle");
        CatObj.PlayAnimation(CatBase.CatAnimation.Idle);
    }
    public override void ExitState()
    {
        Debug.Log("退出CatBaseStateIdle");
    }

    public override void ThrowRob()
    {

    }
    public override void HoldOnRob()
    {

    }
    public override void ReleaseRob()
    {
        
    }

    public override void DrawBackRob()
    {

    }

    public override void MoveBackward()
    {
        CatObj.MoveBackward();
    }

    public override void MoveForward()
    {
        CatObj.MoveForward();
    }
}

public class CatBaseStateReady : CatBaseState
{
    bool isholdon = false;
    public CatBaseStateReady(CatBase cb) : base(cb)
    {

    }
    public override void EnterState()
    {
        Debug.Log("进入CatBaseStateReady");
        isholdon = false;
        //CatObj.PlayAnimation(CatBase.CatAnimation.Idle);
    }

    public override void ExitState()
    {
        Debug.Log("退出CatBaseStateReady");
        CatObj.FinishCallback = null;
        CatObj.HidePowerBar();
    }

    public override void ThrowRob()
    {
        CatObj.PlayAnimation(CatBase.CatAnimation.StartFishing);
        CatObj.ShowPowerBar();
        CatObj.ResetPower();
        CatObj.FinishCallback = (string name) =>
        {
            if(name == "holdon"&&isholdon)
            {
                CatObj.StopAnimation();
            }
            if (name == "throw")
            {
                CatObj.CurrentState = new CatBaseStateFishing(CatObj);
            }
        };
    }

    public override void HoldOnRob()
    {
        isholdon = true;
        CatObj.AddPower();
    }

    public override void ReleaseRob()
    {
        CatObj.ResumeAnimation();
    }

    public override void DrawBackRob()
    {

    }

    public override void MoveBackward()
    {
        CatObj.MoveBackward();
    }

    public override void MoveForward()
    {
        CatObj.MoveForward();
    }
}

public class CatBaseStateFishing : CatBaseState
{
    public CatBaseStateFishing(CatBase cb) : base(cb)
    {

    }
    public override void EnterState()
    {
        Debug.Log("进入CatBaseStateFishing");
        CatObj.PlayAnimation(CatBase.CatAnimation.Waiting);
        EventDispatch.Dispatch(EventID.StartFishing, 0);
    }

    public override void ExitState()
    {
        Debug.Log("退入CatBaseStateFishing");
        CatObj.FinishCallback = null;
    }

    public override void ThrowRob()
    {

    }
    public override void HoldOnRob()
    {

    }
    public override void ReleaseRob()
    {

    }

    public override void DrawBackRob()
    {
        EventDispatch.Dispatch(EventID.CatDrawBack, 0);
        CatObj.PlayAnimation(CatBase.CatAnimation.DrawbackRob);
        CatObj.FinishCallback = (string name) =>
        {
            if (name == "drawback")
            {
                CatObj.CurrentState = new CatBaseStateReady(CatObj);
            }
        };
        
    }

    public override void MoveBackward()
    {

    }

    public override void MoveForward()
    {

    }
}

public class CatBaseStateOnHook : CatBaseState
{
    public CatBaseStateOnHook(CatBase cb) : base(cb)
    {
    }
    public override void EnterState()
    {
        Debug.Log("进入CatBaseStateOnHook");
        CatObj.PlayAnimation(CatBase.CatAnimation.OnHook);
    }

    public override void ExitState()
    {
        Debug.Log("退出CatBaseStateOnHook");
    }

    public override void ThrowRob()
    {

    }

    public override void HoldOnRob()
    {

    }

    public override void ReleaseRob()
    {

    }

    public override void DrawBackRob()
    {
        CatObj.DrawbackRob();
        EventDispatch.Dispatch(EventID.CatDrawBack, 0);
    }

    public override void MoveBackward()
    {
    }

    public override void MoveForward()
    {
    }

}
