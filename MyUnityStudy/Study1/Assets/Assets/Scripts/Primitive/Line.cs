///author:Stephanie
///function:图元类：直线
///date:2015-9-5 17:47:21
namespace Assets.Assets.Scripts.Primitive
{
    class Line : PrimitiveBase
    {
        private float _a;
        private float _b;

        //构造直线：y=ax+b的个数
        public Line(float a, float b)
        {
            this._a = a;
            this._b = b;
        }

        //直线的Y值
        public override float Y
        {
            get
            {
                return GetY();
            }
        }

        private float GetY()
        {
            return _a * _x + _b;
        }
    }
}
