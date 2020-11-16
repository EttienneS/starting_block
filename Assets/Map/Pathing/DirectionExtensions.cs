namespace Assets.Map.Pathing
{
    public static class DirectionExtensions
    {
        public static Direction Opposite(this Direction direction)
        {
            return Rotate(direction, 4);
        }

        public static Direction Rotate(this Direction rotation, int amount)
        {
            var value = (int)rotation;

            value += amount;

            if (value < 0)
            {
                value += 8;
            }
            else if (value > 7)
            {
                value -= 8;
            }

            return (Direction)value;
        }

        public static Direction Rotate90CCW(this Direction rotation)
        {
            return Rotate(rotation, 2);
        }

        public static Direction Rotate90CW(this Direction rotation)
        {
            return Rotate(rotation, -2);
        }

        public static Direction RotateCCW(this Direction rotation)
        {
            return Rotate(rotation, 1);
        }

        public static Direction RotateCW(this Direction rotation)
        {
            return Rotate(rotation, -1);
        }

        public static int ToDegrees(this Direction rotation)
        {
            return (int)rotation * 45;
        }
    }
}