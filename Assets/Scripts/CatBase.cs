using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CatBase : MonoBehaviour
{
    public Transform RobHook;
    public enum CatAnimation { Idle, Move, StartFishing, Waiting, DrawbackRob, OnHook }
    protected Animator m_animator;
    protected SpriteRenderer m_spriteRenderer;
    protected FishSceneMgr m_mgr;

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
                m_animator.Play("CatFishingThrough");
                break;
            case CatAnimation.DrawbackRob:
                m_animator.Play("CatFishingDrawBack");
                break;
            case CatAnimation.Waiting:
                m_animator.Play("CatFishingWaiting");
                break;
        }
    }
    public virtual void MoveForward()
    {
        PlayAnimation(CatAnimation.Move);
        Vector3 old = transform.position;
        old.x += 1.0f;
        transform.position = old;
        m_spriteRenderer.flipX = false;
    }

    public virtual void MoveBackward()
    {
        PlayAnimation(CatAnimation.Move);
        Vector3 old = transform.position;
        old.x -= 1.0f;
        transform.position = old;
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
        //if (evt.stringParameter == "drawback")
        //{
            
        //}
        if(FinishCallback != null)
        {
            FinishCallback(evt.stringParameter);
        }
    }
}
