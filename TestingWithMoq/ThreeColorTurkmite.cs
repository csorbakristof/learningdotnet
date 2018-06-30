using OpenCvSharp;

namespace Turkmite
{
    public class ThreeColorTurkmite : TurkmiteBase
    {
        public override int PreferredIterationCount => 500000;

        readonly private Vec3b black = new Vec3b(0, 0, 0);
        readonly private Vec3b white = new Vec3b(255, 255, 255);
        readonly private Vec3b red = new Vec3b(0, 0, 255);

        public ThreeColorTurkmite(Mat image) : base(image)
        {
        }

        protected override (Vec3b newColor, int deltaDirection) GetNextColorAndUpdateDirection(Vec3b currentColor)
        {
            if (currentColor == black)
                return (white, 1);
            else if (currentColor == white)
                return (red, -1);
            else
                return (black, -1);
        }
    }
}
