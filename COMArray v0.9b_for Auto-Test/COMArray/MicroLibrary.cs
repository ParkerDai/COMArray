using System;
using System.Threading;

namespace MicroLibrary
{
    public class MicroStopwatch : System.Diagnostics.Stopwatch
    {
        double m_dMicroSecPerTick = 1000000D / Frequency;
        private short _socketID;

        public short Tag
        {
            get { return _socketID; }
            set { _socketID = value; }
        }

        public MicroStopwatch()
        {
            if (!System.Diagnostics.Stopwatch.IsHighResolution)
                throw new Exception("On this system the high-resolution performance counter is not available");
        }

        public long ElapsedMicroseconds
        {
            get{ return (long)(ElapsedTicks * m_dMicroSecPerTick); }
        }
    }

    public class MicroTimer
    {
        public delegate void MicroTimerElapsedEventHandler(object sender, MicroTimerEventArgs timerEventArgs);
        public event MicroTimerElapsedEventHandler MicroTimerElapsed;

        Thread m_threadTimer = null;
        long m_lIgnoreEventIfLateBy = long.MaxValue;
        long m_lTimerIntervalInMicroSec = 0;
        bool m_bStopTimer = true;
        private short _socketID;

        public short Tag
        {
            get { return _socketID; }
            set { _socketID = value; }
        }

        public MicroTimer()
        {
        }

        public MicroTimer(long lTimerIntervalInMicroseconds)
        {
            Interval = lTimerIntervalInMicroseconds;
        }

        public long Interval
        {
            get { return m_lTimerIntervalInMicroSec; }
            set { m_lTimerIntervalInMicroSec = value; }
        }

        public long IgnoreEventIfLateBy
        {
            get
            {
                return m_lIgnoreEventIfLateBy;
            }
            set
            {
                if (value == 0)
                    m_lIgnoreEventIfLateBy = long.MaxValue;
                else
                    m_lIgnoreEventIfLateBy = value;
            }
        }

        public bool Enabled
        {
            set
            {
                if (value)
                    Start();
                else
                    Stop();
            }
            get
            {
                return (m_threadTimer != null && m_threadTimer.IsAlive);
            }
        }

        public void Start()
        {
            if ((m_threadTimer == null || !m_threadTimer.IsAlive) && Interval > 0 )
            {
                m_bStopTimer = false;
                ThreadStart threadStart = delegate(){ NotificationTimer(Interval, IgnoreEventIfLateBy, ref m_bStopTimer); };
                m_threadTimer = new Thread(threadStart);
                m_threadTimer.Priority = ThreadPriority.Highest;
                m_threadTimer.Start();
            }
        }

        public void Stop()
        {
            m_bStopTimer = true;

            while (Enabled)
            {
            }
        }

        void NotificationTimer(long lTimerInterval, long lIgnoreEventIfLateBy, ref bool bStopTimer)
        {
            int nTimerCount = 0;
            long lNextNotification = 0;
            long lCallbackFunctionExecutionTime = 0;

            MicroStopwatch microStopwatch = new MicroStopwatch();
            microStopwatch.Start();

            while (!bStopTimer)
            {
                lCallbackFunctionExecutionTime = microStopwatch.ElapsedMicroseconds - lNextNotification;
                lNextNotification += lTimerInterval;
                nTimerCount++;
                long lElapsedMicroseconds = 0;

                while ((lElapsedMicroseconds = microStopwatch.ElapsedMicroseconds) < lNextNotification)
                    Thread.Sleep(1);
                
                long lTimerLateBy = lElapsedMicroseconds - (nTimerCount * lTimerInterval);

                if (lTimerLateBy < lIgnoreEventIfLateBy)
                {
                    MicroTimerEventArgs microTimerEventArgs = new MicroTimerEventArgs(nTimerCount, lElapsedMicroseconds,
                                                                                       lTimerLateBy, lCallbackFunctionExecutionTime);
                    MicroTimerElapsed(this, microTimerEventArgs);
                }
            }

            microStopwatch.Stop();
        }
    }

    public class MicroTimerEventArgs : EventArgs
    {
        public int TimerCount { get; private set; }                     // Simple counter, number times timed event (callback function) executed
        public long ElapsedMicroseconds { get; private set; }           // Time when timed event was called since timer started
        public long TimerLateBy { get; private set; }                   // How late the timer was compared to when it should have been called
        public long CallbackFunctionExecutionTime { get; private set; } // The time it took to execute the previous call to the callback function (OnTimedEvent)

        public MicroTimerEventArgs(int nTimerCount, long lElapsedMicroseconds, long lTimerLateBy, long lCallbackFunctionExecutionTime)
        {
            TimerCount = nTimerCount;
            ElapsedMicroseconds = lElapsedMicroseconds;
            TimerLateBy = lTimerLateBy;
            CallbackFunctionExecutionTime = lCallbackFunctionExecutionTime;
        }
    }
}
