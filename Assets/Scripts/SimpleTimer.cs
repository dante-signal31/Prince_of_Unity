namespace Prince
{
    /// <summary>
    /// <p>Measures time.</p>
    ///
    /// <P>It is intended to be updated inside an Unity's Update or FixedUpdate method's delta or fixedDelta
    /// times.</p>
    /// </summary>
    public class SimpleTimer
    {
        public float ElapsedTime { get; private set; }
        public bool IsMeasuringTime { get; private set; }

        public SimpleTimer()
        {
            ResetTimer();
        }

        /// <summary>
        /// Reset ElapsedTime to zero.
        /// </summary>
        private void ResetTimer()
        {
            ElapsedTime = 0;
            IsMeasuringTime = false;
        }

        /// <summary>
        /// Reset timer and start measuring time.
        /// </summary>
        public void StartTimeMeasure()
        {
            ResetTimer();
            ResumeTimeMeasure();
        }

        /// <summary>
        /// Start measuring time but from the point it was stopped.
        /// </summary>
        public void ResumeTimeMeasure()
        {
            IsMeasuringTime = true;
        }

        /// <summary>
        /// Stop measuring time.
        /// </summary>
        public void StopTimeMeasure()
        {
            IsMeasuringTime = false;
        }

        /// <summary>
        /// Increment elapsed time only if IsMeasuringTime is True.
        /// </summary>
        /// <param name="additionalElapsedTime"></param>
        public void UpdateElapsedTime(float additionalElapsedTime)
        {
            if (IsMeasuringTime) ElapsedTime += additionalElapsedTime;
        }
        
        
    }
}