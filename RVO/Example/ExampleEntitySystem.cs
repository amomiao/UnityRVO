using FixPhysics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RVO.Example
{
    public class ExampleEntitySystem : MonoBehaviour
    {
        private static ExampleEntitySystem _instance;
        public static ExampleEntitySystem GetInstance() => _instance;

        [Header("逻辑帧率")]
        public int logicFrame = 15;
        /// <summary> 主摄计算点击位置 </summary>
        public Camera mainCamera;
        /// <summary> 代理集合 </summary>
        private List<ExampleAgentMono> agents;
        /// <summary> 索引按下shift后的逐个单独移动 </summary>
        private int operIndex;

        private void Awake()
        {
            _instance = this;
            agents = new List<ExampleAgentMono>();
            operIndex = 0;
        }

        private void Start()
        {
            StartCoroutine(IE_Avoid());
        }

        /// <summary> 键鼠操作 </summary>
        private void Update()
        {
            if (agents.Count == 0)
                return;
            // 逐个给代理一个目标位置
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButtonDown(0))
            {
                // 保证相机向前10长度是地面,或改用其他获取目的点的方式
                Vector3 targetPos = mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
                agents[operIndex].SetTargetPos(targetPos);
                if (++operIndex >= agents.Count)
                    operIndex = 0;
            }
            // 给所有代理一个目标位置
            else if (Input.GetMouseButtonDown(0))
            {
                // 保证相机向前10长度是地面,或改用其他获取目的点的方式
                Vector3 targetPos = mainCamera.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10);
                foreach (ExampleAgentMono agent in agents)
                    agent.SetTargetPos(targetPos);
            }
            // 重置
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                operIndex = 0;
            }
        }

        /// <summary> 代理集合中添加实体 </summary>
        public void AddEntity(ExampleAgentMono agentMono) => agents.Add(agentMono);

        /// <summary> 所有代理实体计算一次避障 </summary>
        private IEnumerator IE_Avoid()
        {
            //int count = 0;
            FixNum span = 1f / logicFrame;
            List<AgentLineAreaRVO> lines;
            while (true)
            {
                yield return new WaitForSeconds(span);
                //Debug.Log(count++);
                //yield return 0;
                // 要先统一取刷新代理的属性
                foreach(ExampleAgentMono am in agents)
                    am.RefrushAgent();
                for (int i = 0; i < agents.Count; i++)
                {
                    ToRVO(i,span, out lines);
                    ToCorrectDirection(i, span, lines);
                }
            }
        }
        /// <summary> 明确是否需要避障,需要会构造避障公式 </summary>
        private void ToRVO(int index,FixNum deltaTime, out List<AgentLineAreaRVO> lines)
        {
            lines = new List<AgentLineAreaRVO>();
            AgentLineAreaRVO line = new AgentLineAreaRVO();
            if (agents.Count > 1)
            {
                for (int i = 0; i < agents.Count; i++)
                {
                    if (i == index)
                        continue;
                    if (agents[index].Avoid(agents[i], deltaTime, ref line))
                        lines.Add(line);
                }
            }
        }
        /// <summary> 解避障公式 </summary>
        private void ToCorrectDirection(int index, FixNum deltaTime, List<AgentLineAreaRVO> lines)
        {
            bool getPoint = false;
            AgentLineAreaRVO usedLine;
            for (int i = 0; i < lines.Count; i++)
            {
                for (int j = 0; j < lines.Count; j++)
                {
                    // 双向遍历 i=j时检测的是自己,自己的originalPoint必然合法
                    if (i == j || lines[j].IsLegal(lines[i].originalPoint))
                    {
                        if (j + 1 == lines.Count)
                        {
                            //if (lines.Count > 1)
                            //    Debug.Log($"复合避障:数量 {lines.Count},成功于{j}");
                            getPoint = true;
                            usedLine = lines[i];
                            agents[index].CorrectDirectionVector(deltaTime, usedLine);
                            // Debug.Log($"{nameof(IE_Avoid)}:成功避障,障碍对象物有 {lines.Count}个。");
                        }
                    }
                    else
                    {
                        //if (lines.Count > 1)
                        //    Debug.Log($"复合避障:数量 {lines.Count},失败于{j}");
                        break;
                    }
                }
                if (getPoint)
                    break;
            }
            // 不能避障
            if (!getPoint && lines.Count > 0)
            {
                agents[index].Stop();
                //Debug.Log($"不能找到一个避障路径,暂停");
            }
            lines.Clear();
        }
    }
}