using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CatBase : MonoBehaviour
{
    public Transform RobHook;
    public Slider PowerBar;
    public enum CatAnimation { Idle, Move, HoldOn,StartFishing, Waiting, DrawbackRob, OnHook ,ThrowClose,ThrowMiddle,ThrowFar}
    protected Animator m_animator;
    protected SpriteRenderer m_spriteRenderer;
    protected FishSceneMgr m_mgr;
    public float Speed = 0.5f;

    public float Power { get { return m_power; } }
    private float m_power = 0f;
    private CatBaseState m_state;
    public CatBaseState CurrentState
    {
        get {
            return m_state;
        }
        set { 
            if(m_state != null)
            {
                m_state.ExitState();
            }
            m_state = value;
            m_state.EnterState();
        }
    }

    public delegate void AnimationCallback(string name);
    public AnimationCallback FinishCallback;
    public AnimationCallback StartCallback;

    public FishSceneMgr Manager { get { return m_mgr; } }
    
    public virtual void Init(FishSceneMgr mgr)
    {
        m_animator = GetComponent<Animator>();
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        m_mgr = mgr;
        CurrentState = new CatBaseStateIdle(this);

    }
    public void ResetPower()
    {
        m_power = 0;
        SetPowerBarValue(m_power);
    }
    public void AddPower()
    {
        m_power += 0.001f;
        if(m_power > 1.0)
        { m_power = 1; }
        SetPowerBarValue(m_power);
    }
    public void ShowPowerBar()
    {
        PowerBar.gameObject.SetActive(true);
    }
    
    public void HidePowerBar()
    {
        PowerBar.gameObject.SetActive(false);
    }

    public void SetPowerBarValue(float val)
    {
        PowerBar.value = Mathf.Clamp01(val);
    }

    public virtual void HoldOnRob()
    {
        //m_animator.speed = 0;
    }
    public virtual void AfterHoldOnRob()
    {
        //m_animator.speed = 1.0f;
        if(m_power <= 0.3f)
        {//播放抛竿比较近的动画
            PlayAnimation(CatAnimation.ThrowClose);
        }
        if(m_power > 0.3f && m_power <= 0.6f)
        {
            PlayAnimation(CatAnimation.ThrowMiddle);
        }
        if(m_power > 0.6f)
        {
            PlayAnimation(CatAnimation.ThrowFar);
        }
    }

    public virtual void PlayAnimation(CatAnimation ca)
    {
        switch(ca)
        {
            case CatAnimation.Idle:
                m_animator.Play("CatFishingIdle");
                break;
            case CatAnimation.Move:
                m_animator.Play("CatFishingMove");
                break;
            case CatAnimation.StartFishing:
                m_animator.Play("CatFishingThrow");
                break;
            case CatAnimation.ThrowClose:
                m_animator.Play("CatFishingThrowClose");
                break;
            case CatAnimation.ThrowMiddle:
                m_animator.Play("CatFishingThrowMiddle");
                break;
            case CatAnimation.ThrowFar:
                m_animator.Play("CatFishingThrowFar");
                break;
            case CatAnimation.HoldOn:
                m_animator.Play("CatFishingHoldOn");
                break;
            case CatAnimation.DrawbackRob:
                if (m_power <= 0.3f)
                {
                    m_animator.Play("CatFishingDrawBackClose");
                }
                if (m_power > 0.3f && m_power <= 0.6f)
                {
                    m_animator.Play("CatFishingDrawBackMiddle");
                }
                if (m_power > 0.6f)
                {
                    m_animator.Play("CatFishingDrawBackFar");
                }
                break;
                break;
            case CatAnimation.Waiting:
                if (m_power <= 0.3f)
                {
                    m_animator.Play("CatFishingWaitingClose");
                }
                if (m_power > 0.3f && m_power <= 0.6f)
                {
                    m_animator.Play("CatFishingWaitingMiddle");
                }
                if (m_power > 0.6f)
                {
                    m_animator.Play("CatFishingWaitingFar");
                }
                break;
            case CatAnimation.OnHook:
                m_animator.Play("CatFishingWarning");
                break;
        }
    }
    public virtual void MoveForward()
    {
        PlayAnimation(CatAnimation.Move);
        Vector3 old = transform.localPosition;
        old.x += Speed;
        float max = Manager.Area.transform.localPosition.x + Manager.Area.size.x/2.0f;
        if (old.x >= max)
        {
            return;
        }
        transform.localPosition = old;
        m_spriteRenderer.flipX = false;
    }

    public virtual void MoveBackward()
    {
        PlayAnimation(CatAnimation.Move);
        Vector3 old = transform.localPosition;
        old.x -= Speed;
        float min = Manager.Area.transform.localPosition.x - Manager.Area.size.x / 2.0f;
        if(old.x <= min)
        {
            return;
        }
        transform.localPosition = old;
        m_spriteRenderer.flipX = true;
    }

    public virtual void StartToFish()
    {
        PlayAnimation(CatAnimation.StartFishing);
    }

    public virtual void DrawbackRob()
    {
        PlayAnimation(CatAnimation.DrawbackRob);
    }

    public void StartAnimationEvent(AnimationEvent evt)
    {
        if(StartCallback != null)
        {
            StartCallback(evt.stringParameter);
        }
    }

    public void FinishAnimationEvent(AnimationEvent evt)
    {
        //if(evt.stringParameter == "through")
        //{

        //}
        if (evt.stringParameter == "drawback")
        {
            if(RobHook.childCount > 0)
            {
                for(int i = 0;i< RobHook.childCount;i++)
                {
                    Transform f = RobHook.GetChild(i);
                    Destroy(f.gameObject);
                }
            }
        }
        if (FinishCallback != null)
        {
            FinishCallback(evt.stringParameter);
        }
    }
}
