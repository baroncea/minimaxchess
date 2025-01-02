namespace projectIA
{
    public class Move
    {
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public bool IsCapture { get; set; }

        public Move(int startX, int startY, int endX, int endY, bool isCapture)
        {
            StartX = startX;
            StartY = startY;
            EndX = endX;
            EndY = endY;
            IsCapture = isCapture;
        }
    }
}
