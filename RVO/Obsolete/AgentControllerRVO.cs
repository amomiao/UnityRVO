using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RVO
{
    [Obsolete("多线程方案,现已废弃",true)]
    public class AgentControllerRVO : MonoBehaviour
    {
        //public List<AgentRVO> agents;
        //public bool isRun;
        //public float deltaTime;
        //public int nowIndex;

        //private void Start()
        //{
        //    isRun = true;
        //    Run();
        //}

        //private void OnDestroy()
        //{
        //    isRun = false;
        //}

        //private void Update()
        //{
        //    Debug.Log($"当前线程运算: {nowIndex} / {agents.Count}");
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
        //        foreach (AgentRVO agent in agents)
        //            agent.transform.LookAt(targetPos);
        //    }

        //    deltaTime = Time.deltaTime;
        //    if (agents.Count > 0)
        //    {
        //        foreach (AgentRVO agent in agents)
        //        {
        //            agent.transform.position += agent.directionVector.Value * agent.velocity * Time.deltaTime;
        //            agent.directionVector = agent.transform.forward;
        //            agent.velocity = 1;
        //        }
        //    }
        //}

        //private async Task Thread_ToRVO()
        //{
        //    ToRVO();
        //    await Task.Delay(1);
        //}
        //private void ToRVO()
        //{
        //    for (int i = 0; i < agents.Count; i++)
        //    {
        //        nowIndex = i;
        //        ToRVO(i);
        //    }
        //}

        //private void Run()
        //{
        //    int taskCount = 8;
        //    Task.Run(/*async */() =>
        //    {
        //        try
        //        {
        //            while (isRun)
        //            {
        //                //await Thread_ToRVO();
        //                ToRVO();
        //            }
        //            Debug.Log("线程退出");
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.Log("RVO线程报错: " + e);
        //        }
        //    });
        //}

        //private void ToRVO(int index)
        //{
        //    List<AgentLineAreaRVO> lines = new List<AgentLineAreaRVO>();
        //    AgentLineAreaRVO line = new AgentLineAreaRVO();
        //    if (agents.Count > 1)
        //    {
        //        for (int i = 0; i < agents.Count; i++)
        //        {
        //            // 自身不检测
        //            if (i == index)
        //                continue;
        //            // 距离过远不检测
        //            if ((agents[index].position - agents[i].position).Value.magnitude >
        //                agents[index].radius + agents[i].radius + agents[index].checkLength)
        //                continue;
        //            if (agents[index].Avoid(agents[i], deltaTime, ref line))
        //            {
        //                lines.Add(line);
        //            }
        //        }
        //    }
        //    bool getPoint = false;
        //    AgentLineAreaRVO usedLine;
        //    for (int i = 0; i < lines.Count; i++)
        //    {
        //        for (int j = 0; j < lines.Count; j++)
        //        {
        //            // 双向遍历 i=j时检测的是自己,自己的originalPoint必然合法
        //            if (i == j || lines[j].Legal(lines[i].originalPoint))
        //            {
        //                if (j + 1 == lines.Count)
        //                {
        //                    getPoint = true;
        //                    usedLine = lines[i];
        //                    agents[index].SetDirectionVector(deltaTime, usedLine);
        //                    // Debug.Log($"{nameof(IE_Avoid)}:成功避障,障碍对象物有 {lines.Count}个。");
        //                }
        //            }
        //            else
        //                break;
        //        }
        //        if (getPoint)
        //            break;
        //    }
        //    // 不能避障
        //    if (!getPoint && lines.Count > 0)
        //        agents[index].velocity = 0;
        //    lines.Clear();
        //}
    }
}
