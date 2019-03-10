using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Gamecube_Iso_Renamer_for_Nintendont
{
    public class CoroutineOnEnumerable : IDisposable
    {
        private DispatcherTimer m_timer;
        private IEnumerator<double> m_routine;
        private double m_progress;

        public event EventHandler Progressed, Completed;
        public double MinimumTimeSlice { get; set; }

        public CoroutineOnEnumerable()
        {
            MinimumTimeSlice = 100;
        }

        public bool Busy
        {
            get { return m_timer != null; }
        }

        public double Progress
        {
            get { return m_progress; }

            set
            {
                if (m_progress != value)
                {
                    m_progress = value;

                    if (Progressed != null)
                        Progressed(this, EventArgs.Empty);
                }
            }
        }

        public void Start(IEnumerable<double> routine)
        {
            Stop();

            m_routine = routine.GetEnumerator();

            m_timer = new DispatcherTimer(DispatcherPriority.Normal,
                                          Dispatcher.CurrentDispatcher);
            m_timer.Interval = TimeSpan.FromMilliseconds(1);
            m_timer.Tick += OnTick;
            m_timer.Start();
        }

        public void Stop()
        {
            if (m_timer != null)
            {
                m_timer.Stop();
                m_timer = null;
            }

            if (m_routine != null)
            {
                IDisposable disp = m_routine as IDisposable;
                if (disp != null)
                    disp.Dispose();
            }
        }

        void OnTick(object sender, EventArgs e)
        {
            if (m_routine != null)
            {
                System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
                stopWatch.Start();

                while (stopWatch.ElapsedMilliseconds < MinimumTimeSlice)
                {
                    if (!m_routine.MoveNext())
                    {
                        Stop();
                        Progress = 1;
                        if (Completed != null)
                            Completed(this, EventArgs.Empty);

                        return;
                    }
                }

                Progress = m_routine.Current;
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
