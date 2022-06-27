using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CatBase : MonoBehaviour
{
    public Transform RobHook;
    public Slider PowerBar;
    public enum CatAnimation { Idle, Move, HoldOn,StartFishing, Waiting, DrawbackRob, OnHook }
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

    public virtual void StopAnimation()
    {
        m_animator.speed = 0;
    }
    public virtual void ResumeAnimation()
    {
        m_animator.speed = 1.0f;
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
            case CatAnimation.HoldOn:
                m_animator.Play("CatFishingHoldOn");
                break;
            case CatAnimation.DrawbackRob:
                m_animator.Play("CatFishingDrawBack");
                break;
            case CatAnimation.Waiting:
                m_animator.Play("CatFishingWaiting");
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
