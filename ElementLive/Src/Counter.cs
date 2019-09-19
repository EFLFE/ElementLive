namespace ElementLive.Src
{
    /// <summary> Счётчик. </summary>
#if DEBUG
    [System.Diagnostics.DebuggerDisplay("Tick: {CurrentTick}:{TargetTick}")]
#endif
    struct Counter
    {
        public int CurrentTick;
        public int TargetTick;

        public Counter(int targetTick, int currentTick = 0)
        {
            TargetTick = targetTick;
            CurrentTick = currentTick;
        }

        public bool Tick()
        {
            if (++CurrentTick >= TargetTick)
            {
                CurrentTick = 0;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            return $"{CurrentTick.ToString()}/{TargetTick.ToString()}";
        }

    }
}
