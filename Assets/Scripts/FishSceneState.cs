using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FishSceneState
{
    protected FishSceneMgr m_mgr;
    public FishSceneState(FishSceneMgr mgr)
    {
        m_mgr = mgr;
    }
    public abstract void EnterState();

    public abstract void ExitState();
}



//public class FishSceneStateIdle : FishSceneState
//{
//    public FishSceneStateIdle(FishSceneMgr mgr) :base(mgr)
//    {
//    }

//    public override void EnterState()
//    {
//        m_mgr.CatObj.PlayAnimation(CatBase.CatAnimation.Idle);
//    }

//    public override void ExitState()
//    {
        

//    }

//    public override void DrawBackRob()
//    {

//    }

//    public override void MoveBackward()
//    {
//        m_mgr.CatObj.MoveBackward();
//    }

//    public override void MoveForward()
//    {
//        m_mgr.CatObj.MoveForward();
//    }

//    public override void StartFish()
//    {
        
//    }
//}

//public class FishSceneStateReady : FishSceneState
//{
//    public FishSceneStateReady(FishSceneMgr mgr) : base(mgr)
//    {
//    }
//    public override void EnterState()
//    {

//    }

//    public override void ExitState()
//    {


//    }
//    public override void DrawBackRob()
//    {

//    }

//    public override void MoveBackward()
//    {
//        m_mgr.CatObj.MoveBackward();
//    }

//    public override void MoveForward()
//    {
//        m_mgr.CatObj.MoveForward();
//    }

//    public override void StartFish()
//    {
//        m_mgr.CatObj.StartToFish();
//    }
//}

//public class FishSceneStateFishing : FishSceneState
//{
//    public FishSceneStateFishing(FishSceneMgr mgr) : base(mgr)
//    {
//    }
//    public override void EnterState()
//    {
//        m_mgr.CatObj.PlayAnimation(CatBase.CatAnimation.Waiting);
//    }

//    public override void ExitState()
//    {

//    }

//    public override void DrawBackRob()
//    {
//        m_mgr.CatObj.DrawbackRob();

//    }

//    public override void MoveBackward()
//    {
//        m_mgr.CatObj.MoveBackward();
//        m_mgr.CurrentState = new FishSceneStateIdle(m_mgr);
//    }

//    public override void MoveForward()
//    {
//        m_mgr.CatObj.MoveForward();
//        m_mgr.CurrentState = new FishSceneStateIdle(m_mgr);
//    }

//    public override void StartFish()
//    {
        
//    }
//}

//public class FishSceneStateOnHook : FishSceneState
//{
//    public FishSceneStateOnHook(FishSceneMgr mgr) : base(mgr)
//    {
//    }
//    public override void EnterState()
//    {
//        m_mgr.CatObj.PlayAnimation(CatBase.CatAnimation.OnHook);
//    }

//    public override void ExitState()
//    {

//    }

//    public override void DrawBackRob()
//    {
//        m_mgr.CatObj.DrawbackRob();

//    }

//    public override void MoveBackward()
//    {
//    }

//    public override void MoveForward()
//    {
//    }

//    public override void StartFish()
//    {

//    }
//}