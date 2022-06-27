using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FishSceneMgr : MonoBehaviour
{
    private void Awake()
    {
        EventDispatch.RegisterReceiver<int>(EventID.StartFishing, ChooseLuckyFish);
        EventDispatch.RegisterReceiver<int>(EventID.FishOnHook, FishOnHook);
        EventDispatch.RegisterReceiver<int>(EventID.CatDrawBack, CatDrawBackRob);
    }
    private void OnDestroy()
    {
        EventDispatch.UnRegisterReceiver<int>(EventID.StartFishing, ChooseLuckyFish);
        EventDispatch.UnRegisterReceiver<int>(EventID.FishOnHook, FishOnHook);
        EventDispatch.UnRegisterReceiver<int>(EventID.CatDrawBack, CatDrawBackRob);
    }
    private void OnEnable()
    {
       
    }
    private void OnDisable()
    {
    }

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
    public BoxCollider Area;

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

    float holdonstart;
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
            holdonstart = Time.realtimeSinceStartup;
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            CatObj.CurrentState.ReleaseRob();
        }

        if(Input.GetKey(KeyCode.Space) && (Time.realtimeSinceStartup - holdonstart) > 0.2f)
        {
            CatObj.CurrentState.HoldOnRob();
        }
    }
    
    Coroutine CatchFishCo;
    FishBase LuckyFish;
    public void ChooseLuckyFish(int param)
    {
        CatchFishCo = StartCoroutine(CatchRandomFish());
    }

    public IEnumerator CatchRandomFish()
    {
        int wait_time = Random.Range(1, 10);
        yield return new WaitForSeconds(wait_time);
        CatchFishCo = null;
        LuckyFish = Pool.FetchRandomFish();
        if (LuckyFish != null)
        {
            Debug.Log("�� name = " + LuckyFish.name + " �Ϲ�");
            

            //�����ε����Ӹ���
            Vector3 hookinpool = Pool.transform.InverseTransformPoint(CatObj.RobHook.position);
            Debug.Log("�������������λ�� " + hookinpool);
            LuckyFish.CurrentState = new FishBaseStateReaching(LuckyFish);
            LuckyFish.setNewDestionation(hookinpool);
        }
    }
    Coroutine CatDrawBackCo;
    public void FishOnHook(int param)
    {
        CatObj.CurrentState = new CatBaseStateOnHook(CatObj);
        //LuckyFish.transform.SetParent(CatObj.RobHook.transform);
        CatDrawBackCo = StartCoroutine(CountDownDrawBack());
    }
    IEnumerator CountDownDrawBack()
    {
        //����ʱ�ո�
        yield return new WaitForSeconds(2);
        CatDrawBackCo = null;

        CatObj.CurrentState.DrawBackRob();
        CatObj.CurrentState = new CatBaseStateReady(CatObj);
        if (LuckyFish != null)
        {//������ԥû���ո�
            Debug.Log("����ԥ ������");  
            LuckyFish.CurrentState = new FishBaseStatePatrol(LuckyFish);
            LuckyFish.transform.SetParent(this.Pool.transform);
            LuckyFish = null;
        }
    }

    public void CatDrawBackRob(int param)
    {
        if(CatchFishCo != null)
        {
            StopCoroutine(CatchFishCo);
        }

        if (CatDrawBackCo != null)
        {//˵���ڵ���ʱ����ǰ �ո���
            StopCoroutine(CatDrawBackCo);
            CatObj.CurrentState = new CatBaseStateReady(CatObj);
            LuckyFish.CurrentState = new FishBaseStateDead(LuckyFish);
            LuckyFish.transform.SetParent(CatObj.RobHook.transform);
            LuckyFish.transform.localPosition = new Vector3(0,0,0);
        }
        else
        {
            Debug.Log("�ո����� �㻹û�Ϲ�");
            if(LuckyFish != null)
            {
                LuckyFish.CurrentState = new FishBaseStatePatrol(LuckyFish);
            }
        }
        
        
    }
}
