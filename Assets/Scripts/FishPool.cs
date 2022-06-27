using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RVO;
public class FishPool : MonoBehaviour
{
    public GameObject FishPrefab;
    //public GameObject DestPrefab;
    public int FishNum;
    List<FishBase> fishList;
    public void Init()
    {
        Simulator.Instance.setTimeStep(0.25f);
        Simulator.Instance.setAgentDefaults(14.0f, 10, 5.0f, 5.0f, 0.35f, 0.01f, new RVO.Vector2(0, 0));
        //获取到box
        BoxCollider box = GetComponent<BoxCollider>();
        //Vector2 leftTop = new Vector2(-box.size.x / 2.0f,box.size.y/2.0f);
        //Vector2 rightTop = new Vector2(box.size.x / 2.0f, box.size.y / 2.0f);
        //Vector2 leftBottom = new Vector2(-box.size.x / 2.0f, -box.size.y / 2.0f);
        //Vector2 rightBottom = new Vector2(box.size.x / 2.0f, -box.size.y / 2.0f);

        fishList = new List<FishBase>();
        for (int i = 0;i < FishNum;i++)
        {
            
            float x = Random.Range(-box.size.x / 2.0f, box.size.x / 2.0f);
            float y = Random.Range(-box.size.y / 2.0f, box.size.y / 2.0f);
            GameObject t = Instantiate(FishPrefab,this.transform);
            t.transform.localPosition = new Vector3(x,y);
            

            FishBase fb = t.GetComponent<FishBase>();
            int sid = Simulator.Instance.addAgent(new RVO.Vector2(x, y));
            //Debug.Log("sid = " + sid + " position = "+t.transform.position);
            //随机目的地
            //float dx = Random.Range(-box.size.x / 2.0f, box.size.x / 2.0f);
            //float dy = Random.Range(-box.size.y / 2.0f, box.size.y / 2.0f);
            //Debug.Log("start sid = "+sid+" dx = "+dx+" dy = "+dy);
            fb.Init(sid, new RVO.Vector2(box.size.x,box.size.y));
            fb.CurrentState = new FishBaseStatePatrol(fb);
            fb.name = "fish" + sid;
            fishList.Add(fb);
            fb.SetFishSpeed(Random.Range(0.05f, 0.01f));
            //GameObject td = Instantiate(DestPrefab, this.transform);
            //td.transform.localPosition = new Vector3(dx,dy);
        }
    }

    public FishBase FetchRandomFish()
    {
        if (fishList.Count <= 0) return null;

        int index = Random.Range(0,fishList.Count);
        return fishList[index];
    }

    public void Update()
    {
        Simulator.Instance.doStep();
    }
}
