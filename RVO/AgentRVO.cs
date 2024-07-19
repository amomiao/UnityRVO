using FixPhysics;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RVO
{
    public class AgentRVO
    {
        protected enum E_State
        {
            No,
            Inner,
            In,
            OutRight,
            OutLeft
        }

        private FixVector3 position;
        /// <summary> 运行速度 </summary>
        private FixNum velocity;
        /// <summary> 运行方向 </summary>
        private FixVector3 directionVector;
        /// <summary> 代理半径 </summary>
        private FixNum radius = 0.5f;
        /// <summary> 
        /// 代理检查范围:检测运算中都是半径+检测范围,所以默认值小于半径 
        /// [已废弃]无用参数仅保留
        /// </summary>
        [Obsolete("无用参数仅保留", true)]
        private FixNum checkLength = 0.25f;

        public FixVector3 Position => position;
        public FixNum Velocity => velocity;
        public FixVector3 DirectionVector => directionVector;
        public FixNum Radius => radius;
        [Obsolete("无用参数仅保留", true)]
        public FixNum CheckLength { get => checkLength; set => checkLength = value; }

        public AgentRVO(FixVector3 startPos, FixNum velocity, FixVector3 directionVector, float radius = 0.5f)
        {
            UpdatePos(startPos);
            UpdateSpeed(velocity);
            UpdateDirectionVector(directionVector);
            this.radius = radius;
        }

        public void UpdatePos(FixVector3 position)
        {
            this.position = position;
        }
        public void UpdateSpeed(FixNum velocity)
        {
            this.velocity = velocity;
        }
        public void UpdateDirectionVector(FixVector3 directionVector)
        {
            this.directionVector = directionVector;
        }
        public void Stop() // 使用UpdateSpeed重启移动
        {
            this.velocity = 0;
        }

        /// <summary> 避障导航 </summary>
        /// <param name="b"> 另一个代理 </param>
        /// <param name="deltaTime"> 时间区间 </param>
        /// <param name="line"> 线性区域 </param>
        /// <returns> 是否需要避障(返回true将进行避障,需要时会为<paramref name="line"/>赋值) </returns>
        public bool Avoid(AgentRVO b, FixNum deltaTime, ref AgentLineAreaRVO line)
        {
            // expand B Radius
            FixNum ebr = GetExapndOtherRadius(b);
            FixVector3 vo = GetRelativeMotionIncre(deltaTime, b);
            // 修正检测向量
            // vo = checkLength <= 0 ? vo : vo.Value.normalized * checkLength;
            FixNum halfAngle = GetHalfAngleToTangleOtherRound(out FixNum tangleLength, ebr, b);
            E_State state = IsCollision(vo, ebr, tangleLength, halfAngle, b);
            // 第六步: 返回直线进行线性规划
            if (state != E_State.No)
            {
                FixVector3 correct = FixVector3.zero;
                switch (state)
                {
                    case E_State.No:
                    default:
                        break;
                    case E_State.Inner: // 已经相交时,结束相交状态是最优先的事宜
                        correct = -ThisToOtherVector(b).Value;
                        break;
                    case E_State.In:
                        correct = InMinCorrectDirection(deltaTime, vo, ebr, b);
                        break;
                    case E_State.OutRight:
                        correct = OutMinCorrectDirection(deltaTime, halfAngle, vo, b);
                        break;
                    case E_State.OutLeft: // 左给负半角
                        correct = OutMinCorrectDirection(deltaTime, -halfAngle, vo, b);
                        break;
                }
                // Debug.Log($"{directionVector.Value.normalized}  {correct.Value.normalized}");
                // 最短修正向量既可以直接使用也可以写为公式给予集体使用
                // (1)直接使用最短修正向量:朝向偏移过去
                // directionVector = (GetMotionIncre(deltaTime) + correct).Value.normalized;
                // (2)转为:AX+BY+C<=0的区域,求合法集合
                line = new AgentLineAreaRVO(correct, (GetMotionIncre(deltaTime) + correct));
                return true;
            }
            return false;
        }

        public bool Bvoid(AgentRVO b, FixNum deltaTime, ref AgentLineAreaRVO line)
        {
            // expand B Radius
            FixNum ebr = GetExapndOtherRadius(b);
            FixVector3 vo = GetRelativeMotionIncre(deltaTime, b);
            // 修正检测向量
            // vo = checkLength <= 0 ? vo : vo.Value.normalized * checkLength;
            FixNum halfAngle = GetHalfAngleToTangleOtherRound(out FixNum tangleLength, ebr, b);
            E_State state = b.IsSureOtherCollisionSelf(vo, ebr, tangleLength, halfAngle, this);
            // 第六步: 返回直线进行线性规划
            if (state != E_State.No)
            {
                FixVector3 correct = FixVector3.zero;
                switch (state)
                {
                    case E_State.No:
                    default:
                        break;
                    case E_State.Inner: // 已经相交时,结束相交状态是最优先的事宜
                        // 向相撞方向的反方向
                        correct = -ThisToOtherVector(b).Value;
                        break;
                    case E_State.In:
                        correct = InMinCorrectDirection(deltaTime, vo, ebr, b);
                        break;
                    case E_State.OutRight:
                        correct = OutMinCorrectDirection(deltaTime, halfAngle, vo, b);
                        break;
                    case E_State.OutLeft: // 左给负半角
                        correct = OutMinCorrectDirection(deltaTime, -halfAngle, vo, b);
                        break;
                }
                // Debug.Log($"{directionVector.Value.normalized}  {correct.Value.normalized}");
                // 最短修正向量既可以直接使用也可以写为公式给予集体使用
                // (1)直接使用最短修正向量:朝向偏移过去
                // directionVector = (GetMotionIncre(deltaTime) + correct).Value.normalized;
                // (2)转为:AX+BY+C<=0的区域,求合法集合
                line = new AgentLineAreaRVO(correct, (GetMotionIncre(deltaTime) + correct));
                return true;
            }
            return false;
        }

        /// <summary> 
        /// 修正移动朝向:
        /// 一种设定逻辑,一个线性规划方案的originalPoint能正确避障,则使用他的originalVector修正
        /// </summary>
        public void CorrectDirectionVector(FixNum deltaTime, AgentLineAreaRVO line) => CorrectDirectionVector(deltaTime, line.originalVector);
        /// <summary> 修正移动朝向 </summary>
        public void CorrectDirectionVector(FixNum deltaTime, FixVector3 correct)
        {
            // 三角形法则: ↖+→ => ↑
            directionVector = (GetMotionIncre(deltaTime) + correct).Value.normalized;
        }

        #region 私有数学运算
        /// <summary> 从this指向other的向量:other-this </summary>
        protected FixVector3 ThisToOtherVector(AgentRVO other) => other.position - this.position;
        /// <summary> 从other指向this的向量:this-other </summary>
        protected FixVector3 OtherToThisVector(AgentRVO other) =>  this.position - other.position;
        /// <summary> 获取代理移动增量 </summary>
        /// <param name="deltaTime"> 移动微分时长 </param>
        /// <returns> 秒速*移动微分 </returns>
        protected FixVector3 GetMotionIncre(FixNum deltaTime)
            => directionVector * velocity * deltaTime;

        // 第一步:ra + rb
        /// <summary>
        /// (Expand B radius)b的半径扩大a的半径,使a作为一个点。
        /// 非两圆时,则为ab的闵可夫斯基和。
        /// </summary>
        protected virtual FixNum GetExapndOtherRadius(AgentRVO other)
         => this.radius + other.radius;

        // 第二步:求相对位移量
        /// <summary> 获取两个移动代理的相对移动量:Vo=Va-Vb </summary>
        protected FixVector3 GetRelativeMotionIncre(FixNum deltaTime, AgentRVO other)
            => this.GetMotionIncre(deltaTime) - other.GetMotionIncre(deltaTime);

        // 第三步:求夹角
        /// <summary> 求过a点,切b圆的线段与向量ab的夹角 </summary>
        internal FixNum GetHalfAngleToTangleOtherRound(out FixNum tangleLength, FixNum r, AgentRVO other)
        {
            // 向量ab
            FixVector3 ab = ThisToOtherVector(other);
            // 模长(斜边)
            FixNum modLength = ab.Value.magnitude;
            // 模长与半径通过 勾股定理求切点与a点(无半径圆)距离
            tangleLength = Mathf.Sqrt(Mathf.Abs(modLength * modLength - r * r));
            // arcCos(直角边/斜边) = 半角度
            FixNum halfAngle = Mathf.Acos(tangleLength / modLength) * Mathf.Rad2Deg;
            return halfAngle;
        }

        // 第四步:确认是否相撞
        /// <summary>
        /// (1)处于小圆内，必定相撞
        /// (2)处于三角区域并在大圆外，必定相撞
        /// </summary>
        /// <param name="relativeIncre"> 相对位移量 </param>
        /// <param name="exapndR"> b为圆心 ar+br为半径 </param>
        /// <param name="tangleLength"> a为圆心 a到小圆切点为半径 </param>
        /// <param name="halfAngle"> a到两条切小圆的切线之间的夹角 </param>
        /// <param name="other"> b </param>
        /// <returns> 是否可能相撞 </returns>
        protected E_State IsCollision(FixVector3 relativeIncre, FixNum exapndR, FixNum tangleLength, FixNum halfAngle, AgentRVO other)
        {
            FixVector3 point = this.position + relativeIncre;
            // 极近已碰撞:无关移动量,检测两实体
            if (ThisToOtherVector(other).Value.sqrMagnitude <= exapndR * exapndR)
                return E_State.Inner;
            // 增量处于小圆内,必定相撞
            // relativeIncre以a为原地,此时需要以b为原点
            else if ((point - other.position).Value.sqrMagnitude <= exapndR * exapndR)
                return E_State.In;
            else
            {
                // 在大圆(垂线圆)外继续,在内则不碰撞
                /// relativeIncre 与 tangleLength 都是已a为原点的,无需偏移
                if (relativeIncre.Value.sqrMagnitude > tangleLength * tangleLength)
                {
                    // 处于三角形内
                    FixNum angle = Vector3.SignedAngle(ThisToOtherVector(other), point, Vector3.up);
                    if (Mathf.Abs(angle) <= halfAngle)
                    {
                        if (angle <= 0)
                            return E_State.OutRight;
                        else
                            return E_State.OutLeft;
                    }
                }
            }
            // 1.三角外
            // 2.小圆外,大圆(垂线圆)内
            // 不会发生碰撞
            return E_State.No;
        }

        // 第四步:由可能被撞方来 确认是否相撞
        // this代表小圆圆心
        // requester.position代表射线起点
        protected virtual E_State IsSureOtherCollisionSelf(FixVector3 relativeIncre, FixNum exapndR, FixNum tangleLength, FixNum halfAngle, AgentRVO requester)
        {
            // 增量点
            FixVector3 point = requester.position + relativeIncre;
            // 极近已碰撞
            if (OtherToThisVector(requester).Value.sqrMagnitude <= exapndR * exapndR)
                return E_State.Inner;
            // 增量处于大圆内,必定相撞
            // relativeIncre以a为原地,此时需要以b为原点
            else if ((point - this.position).Value.sqrMagnitude <= exapndR * exapndR)
                return E_State.In;
            else
            {
                // 在大圆外
                /// relativeIncre 与 tangleLength 都是已a为原点的,无需偏移
                if (relativeIncre.Value.sqrMagnitude > tangleLength * tangleLength)
                {
                    // 处于三角形内
                    FixNum angle = Vector3.SignedAngle(OtherToThisVector(requester), point, Vector3.up);
                    if (Mathf.Abs(angle) <= halfAngle)
                    {
                        if (angle <= 0)
                            return E_State.OutRight;
                        else
                            return E_State.OutLeft;
                    }
                }
            }
            return E_State.No;
        }

        // 第五步:若要相撞,修改移动方向
        /// <summary> (1)相对移动量在小圆内时,修正移动量为:(A+相对移动量-B)归一化*(小圆半径 - 圆心与修正量点距离) </summary>
        protected FixVector3 InMinCorrectDirection(FixNum deltaTime, FixVector3 relativeIncre, FixNum exapndR, AgentRVO other)
            => (this.position + relativeIncre - other.position).Value.normalized * (exapndR - Vector3.Distance(other.position, this.position + relativeIncre)) * deltaTime;

        /// <summary> (2)相对移动量在三角区域并在大圆外,修正移动量为:偏移投影到切线,投影<-偏移 </summary>
        protected FixVector3 OutMinCorrectDirection(FixNum deltaTime, FixNum halfAngle, FixVector3 relativeIncre, AgentRVO other)
        {
            // 切线
            FixVector3 dir = Quaternion.Euler(0, halfAngle, 0) * ThisToOtherVector(other).Value.normalized;
            // 偏移投影到切线
            // FixNum length = (this.position + relativeIncre - other.position).Dot(dir);
            FixNum length = (this.position + relativeIncre).Dot(dir);
            // 计算偏移点到切线投影的向量
            // return (dir * length - ThisToOtherVector(other)).Value.normalized * deltaTime;
            return (dir * length - (this.position + relativeIncre)).Value.normalized * deltaTime;
        }
        #endregion 私有数学运算
    }
}
