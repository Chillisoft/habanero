using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Chillisoft.UI.Misc.v2
{
    /// <summary>
    ///	Represents a splash screen that is displayed to the user while the
    /// program is loading at the start.  It usually contains an image which
    /// shows at least the logo and version number.
    /// <br/><br/>
    /// Use MinimumDuration to specify how long the screen will stay up for
    /// and BackgroundImage to choose the image to display.
    /// <br/><br/>
    ///	You can draw version and copyright information onto the 
    /// splash screen by attaching to the Control.Paint event.
    /// </summary>
    public class SplashScreen : DropShadowForm
    {
        private TimeSpan _MinimumDuration;
        private bool _MinimumDurationCompleted;
        private bool _WaitingForTimer;
        private Timer _Timer;

        /// <summary>
        /// Constructor to initialise the splash screen
        /// </summary>
        public SplashScreen()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handles the event of the background image being changed
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        protected override void OnBackgroundImageChanged(EventArgs e)
        {
            base.OnBackgroundImageChanged(e);

            if (BackgroundImage != null)
            {
                ClientSize = BackgroundImage.Size;
            }
            else
            {
                ClientSize = Size.Empty;
            }
        }

        /// <summary>
        /// Gets and sets the minimum duration of the screen as a TimeSpan
        /// object. This value cannot be negative.
        /// </summary>
        public TimeSpan MinimumDuration
        {
            get { return _MinimumDuration; }
            set
            {
                if (value < TimeSpan.Zero)
                    throw new ArgumentOutOfRangeException("value", value,
                                                          "value must be greater than or equal to TimeSpan.Zero");

                _MinimumDuration = value;
            }
        }

        /// <summary>
        /// Releases resources used by the screen
        /// </summary>
        /// <param name="disposing">Set as true to release both managed and 
        /// unmanaged resources; false to release only unmanaged resources</param>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    if (_Timer != null)
                    {
                        _Timer.Dispose();
                        _Timer.Tick -= new EventHandler(OnTimerTick);
                        _Timer = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        /// <summary>
        /// Handles the event of the screen's visibility being changed
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (Visible && _MinimumDuration.TotalMilliseconds > 0)
            {
                _MinimumDurationCompleted = false;
                _WaitingForTimer = false;

                _Timer = new Timer();
                _Timer.Tick += new EventHandler(OnTimerTick);
                _Timer.Interval = (int) _MinimumDuration.TotalMilliseconds;
                _Timer.Start();
            }
        }

        /// <summary>
        /// Called on closing of the screen
        /// </summary>
        /// <param name="e">Attached arguments regarding the event</param>
        protected override void OnClosing(CancelEventArgs e)
        {
            if (_MinimumDuration > TimeSpan.Zero && !_MinimumDurationCompleted)
            {
                // Waiting for the timer to finish
                _WaitingForTimer = true;
                e.Cancel = true;
            }
            else
            {
                base.OnClosing(e);
            }
        }

        /// <summary>
        /// Called when the timer passes the minimum duration, closing the
        /// screen
        /// </summary>
        /// <param name="sender">The object that notified of the event</param>
        /// <param name="e">Attached arguments regarding the event</param>
        private void OnTimerTick(object sender, EventArgs e)
        {
            _Timer.Stop();
            _MinimumDurationCompleted = true;

            if (_WaitingForTimer)
            {
                Close();
            }
        }

        #region InitializeComponent()

        /// <summary>
        /// Initialises the components of the splash screen
        /// </summary>
        private void InitializeComponent()
        {
            this.AutoScaleMode = AutoScaleMode.None;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new Size(0, 0);
            this.ControlBox = false;
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashScreen";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        }

        #endregion
    }
}