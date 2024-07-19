using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FixPhysics
{
    public struct ColliderBox : ICollider
    {
        public readonly FixVector3 center;
        public readonly FixVector3 eulerAngle;
        public readonly FixVector3 halfSize;

        public readonly FixVector3 Size => halfSize * 2;
        public readonly FixVector3 XAxis => Quaternion.Euler(eulerAngle) * Vector3.forward;
        public readonly FixVector3 YAxis => Quaternion.Euler(eulerAngle) * Vector3.up;
        public readonly FixVector3 ZAxis => Quaternion.Euler(eulerAngle) * Vector3.right;

        /// <summary> 碰撞盒 </summary>
        /// <param name="center"> 中心 </param>
        /// <param name="eulerAngle"> 负的Transform的欧拉角 </param>
        /// <param name="size"> 尺寸 </param>
        public ColliderBox(Vector3 center, Vector3 eulerAngle, Vector3 size)
        {
            this.center = center;
            this.eulerAngle = -eulerAngle;
            this.halfSize = size / 2;
        }
    }
}
