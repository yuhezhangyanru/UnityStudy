///author：Stephanie
///function：图元基类
///date：2015-9-12 23:00:11
///
namespace Assets.Assets.Scripts.Primitive
{
    class PrimitiveBase
    {
        protected float _x;
        protected float _y;

        protected float _z;


        public virtual float X { get { return _x; } }
        public virtual float Y { get { return _y; } }
        public virtual float Z { get { return _z; } }
    }
}
