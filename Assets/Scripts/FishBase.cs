using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVO;
public class FishBase : MonoBehaviour
{
    public int SID { get { return Sid; } }
    RVO.Vector2 Destionation;
    int Sid = -1;
    RVO.Vector2 BoxSize;
    SpriteRenderer m_spriteRenderer;
    FishBaseState m_state;
    public FishBaseState CurrentState
    {
        get { return m_state; }
        set { 
            if(m_state != null)
            {
                m_state.ExitState();
            }
            m_state = value;
            m_state.EnterState();
        }
    }
    public void Init(int sid, RVO.Vector2 dst, RVO.Vector2 boxSize)
    {
        m_spriteRenderer = GetComponent<SpriteRenderer>();
        Destionation = dst;
        Sid = sid;
        BoxSize = boxSize;
        
        SetFlip();
    }

    public bool reachDestionation()
    {
        RVO.Vector2 currentPosition =Simulator.Instance.getAgentPosition(Sid);
        if (RVOMath.absSq(currentPosition-Destionation) > 0.7f)
        {
            return false;
        }
        return true;
    }

    public void choseNewDestionation()
    {
        RVO.Vector2 currentPosition = Simulator.Instance.getAgentPosition(Sid);
        float x = Random.Range(BoxSize.x() / 2.0f, -BoxSize.x() / 2.0f);
        float y = Random.Range(BoxSize.y() / 2.0f, -BoxSize.y() / 2.0f);
        if (currentPosition.x() > 0f)
        {//˵�����ұߣ��Ǿ�������ζ�
            x -= 5.0f;
        }
        else
        {
            x += 5.0f;
        }
        Destionation = new RVO.Vector2(x, y);
        SetFlip();
        //Debug.Log("choseNewDestionation sid = " + Sid + " dx = " + x + " dy = " + y);
    }

    public void SetFlip()
    {
        RVO.Vector2 currentPosition = Simulator.Instance.getAgentPosition(Sid);
        if (Destionation.x() > currentPosition.x())
        {
            m_spriteRenderer.flipX = true;
        }
        else
        {
            m_spriteRenderer.flipX = false;
        }
    }

    public void OnHook(Vector3 hookPosition)
    {

    }

    public void Update()
    {
        if (CurrentState != null) CurrentState.OnUpdate();
        
        if (Sid < 0) return;
        RVO.Vector2 current_position = Simulator.Instance.getAgentPosition(Sid);
        transform.localPosition = new Vector3(current_position.x(), current_position.y(), 10);

        RVO.Vector2 velocity = Destionation - current_position;
        if (RVOMath.absSq(velocity) > 1.0f)
        {
            velocity = RVOMath.normalize(velocity);
        }
        Simulator.Instance.setAgentPrefVelocity(Sid, velocity);


    }
}
