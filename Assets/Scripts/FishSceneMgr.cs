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
    FishBase ChooseFish;
    public void ChooseLuckyFish(int param)
    {
        CatchFishCo = StartCoroutine(CatchRandomFish());
    }

    public IEnumerator CatchRandomFish()
    {
        int wait_time = Random.Range(1, 10);
        yield return new WaitForSeconds(wait_time);
        ChooseFish = Pool.FetchRandomFish();
        if (ChooseFish != null)
        {
            Debug.Log("�� name = " + ChooseFish.name + " �Ϲ�");
            

            //�����ε����Ӹ���
            Vector3 hookinpool = Pool.transform.InverseTransformPoint(CatObj.RobHook.position);
            Debug.Log("�������������λ�� " + hookinpool);
            ChooseFish.CurrentState = new FishBaseStateReaching(ChooseFish);
            ChooseFish.setNewDestionation(hookinpool);
            CatchFishCo = null;
        }
    }
    Coroutine CatDrawBackCo;
    public void FishOnHook(int param)
    {
        CatObj.CurrentState = new CatBaseStateOnHook(CatObj);
        CatDrawBackCo = StartCoroutine(CountDownDrawBack());
    }
    IEnumerator CountDownDrawBack()
    {
        //����ʱ�ո�
        yield return new WaitForSeconds(2);
        CatDrawBackCo = null;
        if (ChooseFish != null && ChooseFish.transform.parent != CatObj.RobHook)
        {//������ԥû���ո�
            Debug.Log("����ԥ ������");
            CatObj.CurrentState.DrawBackRob();
            CatObj.CurrentState = new CatBaseStateReady(CatObj);
            ChooseFish.CurrentState = new FishBaseStatePatrol(ChooseFish);
        }
    }

    public void CatDrawBackRob(int param)
    {
        if(CatDrawBackCo != null&& ChooseFish != null)
        {//˵���ڵ���ʱ����ǰ �ո���
            StopCoroutine(CatDrawBackCo);
            CatObj.CurrentState = new CatBaseStateReady(CatObj);
            ChooseFish.CurrentState = new FishBaseStateDead(ChooseFish);
            ChooseFish.transform.SetParent(CatObj.RobHook);
            ChooseFish.transform.localPosition = new Vector3(0, 0, 0);
        }
        else
        {
            Debug.Log("�ո����ˣ�������");
        }
        
    }
}
