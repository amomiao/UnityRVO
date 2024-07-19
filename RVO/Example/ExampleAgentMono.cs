using FixPhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RVO.Example
{
    /// <summary> 实现代理一个的案例 </summary>
    public class ExampleAgentMono : MonoBehaviour
    {
        /// <summary> 标准速度 </summary>
        public static FixNum StandardSpeed = 0.5f;
        /// <summary> 移向目标 </summary>
        public FixVector3 targetPos;
        /// <summary> 代理寻路器 </summary>
        protected AgentRVO agent;
        /// <summary> 寻路器指向 </summary>
        private FixVector3 dir;

        /// <summary> 移向目标的方向 </summary>
        private FixVector3 ToTargetDir
        {
            get
            {
                if (targetPos == transform.position)
                    targetPos += new FixVector3(Random.Range(0, 0.0001f), 0, Random.Range(0, 0.0001f));
                return (targetPos.Value - transform.position).normalized;
            }
        }

        protected virtual void Start()
        {
            targetPos = transform.position;
            agent = new AgentRVO(transform.position, StandardSpeed, transform.forward, 0.5f);
            ExampleEntitySystem.GetInstance().AddEntity(this);
        }

        /// <summary> 设置移动目标点 </summary>
        public void SetTargetPos(FixVector3 targetPos)
        { 
            this.targetPos = targetPos;
        }

        /// <summary> 
        /// 刷新代理器数据:
        /// 实际运行不会总按代理的期望进行,代理的运算需要真实的运行数据。
        /// </summary>
        public void RefrushAgent()
        {
            agent.UpdatePos(transform.position);
            agent.UpdateSpeed(StandardSpeed);
            agent.UpdateDirectionVector(ToTargetDir);
        }

        /// <summary> 进行一次避障 </summary>
        public virtual bool Avoid(ExampleAgentMono b, FixNum deltaTime, ref AgentLineAreaRVO line)
            => agent.Bvoid(b.agent, deltaTime, ref line);

        /// <summary> 停止寻路 </summary>
        public void Stop()=> agent.Stop();

        /// <summary> 代理修正移动方向 </summary>
        public void CorrectDirectionVector(FixNum deltaTime, AgentLineAreaRVO line) => agent.CorrectDirectionVector(deltaTime, line);

        /// <summary> 获取代理步长 </summary>
        private Vector3 GetMoveStep()
        {
            dir = agent.DirectionVector;
            // 可以做一些操作，如转向
            transform.forward = dir;
            return dir * agent.Velocity * Time.deltaTime;
        }


        protected virtual void Update()
        {
            transform.position += GetMoveStep();
        }

        protected virtual void OnDrawGizmos()
        {
            if (agent == null)
                return;
            Color color = Gizmos.color;
            // 代理范围
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, agent.Radius);
            // 检测射线
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + agent.DirectionVector.Value * (agent.Radius + 1));
            // 目标点
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(targetPos, agent.Radius);
            Gizmos.DrawLine(transform.position, targetPos);
            Gizmos.color = color;
        }
    }
}