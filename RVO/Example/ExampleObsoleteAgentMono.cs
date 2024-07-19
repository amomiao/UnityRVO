using FixPhysics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RVO.Example
{
    public class ExampleObsoleteAgentMono : ExampleAgentMono
    {
        protected override void Start()
        {
            targetPos = transform.position;
            agent = new RectObsoleteAgentRVO(transform.position, StandardSpeed, transform.forward, new RectObsoleteAgentRVO.Rect(10, 10), 0);
            ExampleEntitySystem.GetInstance().AddEntity(this);
        }

        public override bool Avoid(ExampleAgentMono b, FixNum deltaTime, ref AgentLineAreaRVO line) => false;

        protected override void Update() { }

        protected override void OnDrawGizmos() { }
    }
}