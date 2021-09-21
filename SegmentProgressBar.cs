/**
 * Author: www.github.com/nichnet/
 */

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;


namespace Nichnet
{
    public partial class SegmentProgressBar : UserControl
    {
        public enum SegmentState { NOT_STARTED, IN_PROGRESS, COMPLETED, FAILED };

        private const int MINIMUM_SEGMENTS = 1;

        private int maximumSegments;
        private bool showBorder = true;
        private bool showText = true;

        private Color borderColor = Color.Black;
        private Color textColor = Color.White;
        private Color notStartedColor = Color.LightGray;
        private Color failedColor = Color.Red;
        private Color inProgressColor = Color.Yellow;
        private Color completedColor = Color.Green;
        private string text;

        private SegmentState[] segments;


        public SegmentProgressBar() {
            InitializeComponent();
            Reset();
        }

        private void Reset() {
            //ensure maximum segments is at least 1...
            this.maximumSegments = Math.Max(1, this.maximumSegments);

            this.segments = new SegmentState[maximumSegments];
            this.Invalidate();
        }

        public void UpdateSegment(int index, SegmentState state) {
            if (index >= 0 && index < maximumSegments) {
                this.segments[index] = state;
                this.Invalidate();
                return;
            }

            //error out of bounds
            throw new IndexOutOfRangeException("Tried to update segment (index) that is outside the bounds of the segment array.");
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            DrawSegments(e.Graphics);

            if (ShowText) {
                DrawText(e.Graphics);
            }

            if (ShowBorder) {
                DrawBorder(e.Graphics);
            }
        }

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            this.Invalidate();
        }

        private void DrawBorder(Graphics g) {
            g.DrawRectangle(new Pen(Color.Black), 0, 0, this.Width - 1, this.Height - 1);
        }

        private void DrawText(Graphics g) {
            SizeF textSize = g.MeasureString(this.Text, this.Font);
            g.DrawString(this.text, this.Font, new SolidBrush(TextColor), new Point((int)(this.Width / 2 - textSize.Width / 2), (int)(this.Height / 2 - textSize.Height / 2)));
        }

        private void DrawSegments(Graphics g) {
            //these must be cast as float to have the sum a float, otherwise it rounds down to int
            float segmentWidth = (float)this.Width / (float)this.segments.Length;

            for (int j = 0; j < this.segments.Length; j++) {
                float x = j * segmentWidth;
                g.FillRectangle(new SolidBrush(ColorFromSegmentState(this.segments[j])), x, 0, segmentWidth, this.Height);
            }
        }

        private Color ColorFromSegmentState(SegmentState state) {
            switch (state) {
                default:
                case SegmentState.NOT_STARTED:
                    return NotStartedColor;
                case SegmentState.IN_PROGRESS:
                    return InProgressColor;
                case SegmentState.COMPLETED:
                    return CompletedColor;
                case SegmentState.FAILED:
                    return FailedColor;
            }
        }

        [Description("Maximum segments of the progress bar."), DefaultValue(100), Browsable(true)]
        public int MaximumSegments {
            get { return this.maximumSegments; }
            set {
                if (value < MINIMUM_SEGMENTS) {
                    throw new Exception("Minimum segment value must be at least 1");
                }

                this.maximumSegments = value; this.Reset();
            }
        }

        [Description("Is the border shown?"), DefaultValue(true), Browsable(true)]
        public bool ShowBorder {
            get { return this.showBorder; }
            set { this.showBorder = value; this.Invalidate(); }
        }

        [Description("Is the text shown?"), DefaultValue(true), Browsable(true)]
        public bool ShowText {
            get { return this.showText; }
            set { this.showText = value; this.Invalidate(); }
        }

        [Description("Color of the border."), Category("Progress"), Browsable(true)]
        public Color BorderColor {
            get { return this.borderColor; }
            set { this.borderColor = value; this.Invalidate(); }
        }

        [Description("Color of the text."), Category("Progress"), Browsable(true)]
        public Color TextColor {
            get { return this.textColor; }
            set { this.textColor = value; this.Invalidate(); }
        }

        [Description("Color of non-started segments."), Category("Progress"), Browsable(true)]
        public Color NotStartedColor {
            get { return this.notStartedColor; }
            set { this.notStartedColor = value; this.Invalidate(); }
        }

        [Description("Color of working segments."), Category("Progress"), Browsable(true)]
        public Color InProgressColor {
            get { return this.inProgressColor; }
            set { this.inProgressColor = value; this.Invalidate(); }
        }

        [Description("Color of completed segments."), Category("Progress"), Browsable(true)]
        public Color CompletedColor {
            get { return this.completedColor; }
            set { this.completedColor = value; this.Invalidate(); }
        }

        [Description("Color of failed segments."), Category("Progress"), Browsable(true)]
        public Color FailedColor {
            get { return this.failedColor; }
            set { this.failedColor = value; this.Invalidate(); }
        }

        [Description("Text value of the progress bar."), Category("Progress"), Browsable(true)]
        public override string Text {
            get { return this.text; }
            set { this.text = value; this.Invalidate(); }
        }

        public int SegmentsCompleted {
            get {
                return GetSegmentsOfType(SegmentState.COMPLETED);
            }
        }

        public int SegmentsFailed {
            get {
                return GetSegmentsOfType(SegmentState.FAILED);
            }
        }

        public int SegmentsInProgress {
            get {
                return GetSegmentsOfType(SegmentState.IN_PROGRESS);
            }
        }

        public int SegmentsNotStarted {
            get {
                return GetSegmentsOfType(SegmentState.NOT_STARTED);
            }
        }

        private int GetSegmentsOfType(SegmentState state) {
            int total = 0;

            foreach (SegmentState setState in this.segments) {
                if (setState == state) {
                    total++;
                }
            }

            return total;
        }
    }
}
