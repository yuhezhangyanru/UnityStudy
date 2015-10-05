///author：张燕茹
///function：矩形
///date：2015-9-13 11:46:46

using UnityEngine;
namespace Assets.Assets.Scripts.Primitive
{
    class Rectangle : PrimitiveBase
    {
        public override float X
        {
            get
            {
                return base.X;
            }
        }

        public override float Y
        {
            get
            {
                return base.Y;
            }
        }

        public override float Z
        {
            get
            {
                return base.Z;
            }
        }

        public Vector3 min;  //左上角
        public Vector3 max;  //右下角坐标

        public Vector3 center//中心点
        {
            get
            {
                return (min + max) / 2f;
            }
        }
    }
}
