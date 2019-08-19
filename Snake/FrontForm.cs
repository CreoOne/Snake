using EntityComponentFramework;
using EntityComponentFramework.Processes;
using Snake.Incubators;
using Snake.MovementDirection;
using Snake.Processes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class FrontForm : Form
    {
        private Processor Processor;
        private FrameCounter FrameCounter;

        public FrontForm()
        {
            InitializeComponent();
            InitializeCanvasSize();
            FrontForm_Resize(this, EventArgs.Empty);
            FrameCounter = new FrameCounter();

            Bitmap frame = new Bitmap(canvas.ClientSize.Width, canvas.ClientSize.Height);
            canvas.Image = frame;
            SoftwareRender renderProcess = new SoftwareRender(Graphics.FromImage(frame));
            renderProcess.ExecuteEnded += (o, e) =>
            {
                canvas.Refresh();
                FrameCounter.Tick();
                Text = string.Format("FPS: {0:00.00}", FrameCounter.SmoothFPS);
            };

            List<IProcess> processes = new List<IProcess>();
            processes.Add(new Movement());
            processes.Add(renderProcess);
            processes.Add(new Collision());

            HashSet<Entity> entities = new HashSet<Entity>
            {
                new SnakeIncubator().Create(),
                new AppleIncubator().Create()
            };

            Processor = new Processor(processes, entities);
        }

        private void InitializeCanvasSize()
        {
            canvas.Width = GameConfig.BoardWidth * GameConfig.TileWidth;
            canvas.Height = GameConfig.BoardHeight * GameConfig.TileHeight;
        }

        private void LogicTimer_Tick(object sender, EventArgs e)
        {
            Processor.Advance();
        }

        private void FrontForm_Resize(object sender, EventArgs e)
        {
            canvas.Left = ClientSize.Width / 2 - canvas.Width / 2;
            canvas.Top = ClientSize.Height / 2 - canvas.Height / 2;
        }

        private void FrontForm_KeyDown(object sender, KeyEventArgs e)
        {
            IMovementDirection movementDirection = MovementDirectionFactory.Create(e.KeyCode);

            if(movementDirection != null)
                Movement.MovementDirection = movementDirection;
        }
    }
}
