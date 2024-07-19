using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FixPhysics.Document
{
    public class Document : MonoBehaviour
    {
        public BoxCollider boxCollider1;
        public BoxCollider boxCollider2;

        #region FixFloat
        [ContextMenu("FixFloat")]
        public void ToFixFloat()
        {
            float f = 0.9999999f;
            FixNum ff = new FixNum(f);
            Debug.Log(
                $"最大值:{FixNum.MaxValue.ToString("#.#")} " +
                $"Float:{f.ToString("F3")} " +
                $"原始数值:{ff.rawValue.ToString("G")} " +
                $"FixFloat:{ff.Value.ToString("#.#####")} " +
                $"小数部分:{ff.FractionalPart().ToString("G")}");
        }
        #endregion FixFloat

        #region FixVector3
        [ContextMenu("FixVector3")]
        public void ToFixVector3()
        {
            FixVector3 v3 = new FixVector3(114.514f, 1919.81f, 0);
            Debug.Log(v3.ToString());
        }
        #endregion FixVector3

        #region ColliderBox
        [ContextMenu("ColliderBox:创建两个Cube")]
        public void CreateColliderBox()
        {
            boxCollider1 = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<BoxCollider>();
            boxCollider2 = GameObject.CreatePrimitive(PrimitiveType.Cube).AddComponent<BoxCollider>();
        }
        [ContextMenu("ColliderBox:碰撞检测")]
        public void ToCollisionDetect()
        {
            // 自制
            CollisionDetector detector = new CollisionDetector();
            ColliderBox box1 = new ColliderBox(boxCollider1.transform.position, boxCollider1.transform.rotation.eulerAngles, boxCollider1.transform.localScale);
            ColliderBox box2 = new ColliderBox(boxCollider2.transform.position, boxCollider2.transform.rotation.eulerAngles, boxCollider2.transform.localScale);
            bool isCollision1 = detector.DetectCollision(box1, box2);
            //// 案例
            //bool isCollision2 = CollisionDetect.DetectCollison_OptCaculate(
            //     box1.center, box1.XAxis, box1.YAxis, box1.ZAxis, box1.halfSize * 2,
            //     box2.center, box2.XAxis, box2.YAxis, box2.ZAxis, box2.halfSize * 2);
            //Debug.Log("自制碰撞检测结果:" + isCollision1 + "  案例碰撞检测结果:" + isCollision2);
        }
        #endregion ColliderBox
    }
}
