using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RVO.Example
{
    [Obsolete("Agent为Mono时的方案,现已废弃", true)]
    public class ExampleRVOForward : MonoBehaviour
    {
        //public bool isDrawGizoms;
        //public Camera mainCamera;
        //public List<AgentRVO> agents;
        //// 用于Gizmos绘制
        //private List<List<AgentRVO>> debugCollisionAgents = new List<List<AgentRVO>>();
        //// 用于镜头跟随
        //private Vector3 offset;

        //public void Awake()
        //{
        //    if (mainCamera == null)
        //        mainCamera = Camera.main;
        //    if (agents.Count > 0)
        //        offset = mainCamera.transform.position - agents[0].transform.position;
        //}

        //private void Start()
        //{
        //    StartCoroutine(IE_Avoid());
        //    foreach (AgentRVO agent in agents)
        //        debugCollisionAgents.Add(new List<AgentRVO>());
        //}

        //private void Update()
        //{
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        Vector3 targetPos = mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
        //        foreach (AgentRVO agent in agents)
        //            agent.transform.LookAt(targetPos);
        //    }

        //    //mainCamera.transform.position = agents[0].transform.position + offset;
        //    if (agents.Count > 0)
        //    {
        //        foreach (AgentRVO agent in agents)
        //            agent.transform.position += agent.directionVector.Value * agent.velocity * Time.deltaTime;
        //    }
        //}

        //private IEnumerator IE_Avoid()
        //{
        //    while (true)
        //    {
        //        //yield return new WaitForSeconds(0.1f);
        //        yield return 0;
        //        foreach (AgentRVO agent in agents)
        //        {
        //            agent.directionVector = agent.transform.forward;
        //            agent.velocity = 1;
        //        }
        //        // 全体避障
        //        for (int i = 0; i < agents.Count; i++)
        //            ToRVO(i);
        //        // 单体避障
        //        //ToRVO(1);
        //    }
        //}

        //private void ToRVO(int index)
        //{
        //    debugCollisionAgents[index].Clear();
        //    List<AgentLineAreaRVO> lines = new List<AgentLineAreaRVO>();
        //    AgentLineAreaRVO line = new AgentLineAreaRVO();
        //    if (agents.Count > 1)
        //    {
        //        for (int i = 0; i < agents.Count; i++)
        //        {
        //            if (i == index)
        //                continue;
        //            if (agents[index].Avoid(agents[i], Time.deltaTime, ref line))
        //            {
        //                lines.Add(line);
        //                debugCollisionAgents[index].Add(agents[i]);
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
        //                    //if (lines.Count > 1)
        //                    //    Debug.Log($"复合避障:数量 {lines.Count},成功于{j}");
        //                    getPoint = true;
        //                    usedLine = lines[i];
        //                    agents[index].SetDirectionVector(Time.deltaTime, usedLine);
        //                    // Debug.Log($"{nameof(IE_Avoid)}:成功避障,障碍对象物有 {lines.Count}个。");
        //                }
        //            }
        //            else
        //            {
        //                //if (lines.Count > 1)
        //                //    Debug.Log($"复合避障:数量 {lines.Count},失败于{j}");
        //                break;
        //            }
        //        }
        //        if (getPoint)
        //            break;
        //    }
        //    // 不能避障
        //    if (!getPoint && lines.Count > 0)
        //    {
        //        agents[index].velocity = 0;
        //        //Debug.Log($"不能找到一个避障路径,暂停");
        //    }
        //    lines.Clear();
        //}

        //private void OnDrawGizmos()
        //{
        //    if (!isDrawGizoms)
        //        return;
        //    if (agents.Count > 0)
        //    {
        //        foreach (AgentRVO agent in agents)
        //        {
        //            if (agent.velocity == 0)
        //                Gizmos.color = Color.yellow;
        //            Gizmos.DrawWireSphere(agent.position, agent.radius);
        //            // forward
        //            Gizmos.color = Color.yellow;
        //            Gizmos.DrawLine(agent.position, agent.position + agent.directionVector * 2);
        //            Gizmos.color = Color.white;
        //        }
        //        GizmosFocusTo(0, 1);
        //        GizmosFocusTo(1, 0);
        //        Gizmos.color = Color.red;
        //        for (int i = 0; i < debugCollisionAgents.Count; i++)
        //            for (int j = 0; j < debugCollisionAgents[i].Count; j++)
        //                Gizmos.DrawLine(agents[i].position, debugCollisionAgents[i][j].position);
        //        Gizmos.color = Color.white;
        //    }
        //}

        //private void GizmosFocusTo(int from, int to)
        //{
        //    AgentRVO agent1 = agents[from];
        //    AgentRVO agent2 = agents[to];
        //    Gizmos.DrawLine(agent1.position, agent2.position);
        //    float r = agent1.radius + agent2.radius;
        //    float angle = agent1.GetHalfAngleToTangleOtherRound(out _, r, agent2);
        //    Gizmos.DrawLine(agent1.position, (Vector3)agent1.position + Quaternion.Euler(0, angle, 0) * (agent2.position - agent1.position));
        //    Gizmos.DrawLine(agent1.position, (Vector3)agent1.position + Quaternion.Euler(0, -angle, 0) * (agent2.position - agent1.position));
        //}
    }
}
