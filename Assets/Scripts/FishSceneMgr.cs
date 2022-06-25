using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FishSceneMgr : MonoBehaviour
{
    public FishSceneState CurrentState { get { return m_state; }set 
        {
            if (m_state != null)
                m_state.ExitState();
            m_state = value;
            m_state.EnterState();
        } }
    FishSceneState m_state;

    public CatBase CatObj;
    public FishPool Pool;

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        //m_state = new FishSceneStateIdle(this);
        CatObj.Init(this);
    }
    #region
    public void StartFish()
    {
        if (CatObj.CurrentState != null) CatObj.CurrentState.ThrowRob();
    }
    public void DrawbackRob()
    {
        if (CatObj.CurrentState != null) CatObj.CurrentState.DrawBackRob();
    }

    public void MoveForward()
    {
        if (CatObj.CurrentState != null) CatObj.CurrentState.MoveForward();
    }

    public void MoveBackward()
    {
        if (CatObj.CurrentState != null) CatObj.CurrentState.MoveBackward();
    }
    #endregion

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            MoveForward();
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveBackward();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartFish();
            DrawbackRob();
        }
    }
    Coroutine CatchFishCo;
    public void WaitingFishOnHook()
    {
        CatchFishCo = StartCoroutine(CatchRandomFish());
    }
    public IEnumerator CatchRandomFish()
    {
        yield return new WaitForSeconds(5);
        FishBase fb = Pool.FetchRandomFish();
        Debug.Log("鱼 sid = "+fb.name+" 上钩");
        
        //让鱼游到钩子附近
        Vector3 hookinpool = Pool.transform.InverseTransformPoint(CatObj.RobHook.position);
        Debug.Log("钩子在鱼塘里的位置 "+hookinpool);
    }
}
