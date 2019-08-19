using EntityComponentFramework;
using EntityComponentFramework.Processes;
using Snake.Components;
using Snake.Incubators;
using Snake.MovementDirection;
using Snake.Processes;
using Snake.Snake;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    public partial class FrontForm : Form
    {
        private Processor Processor;
        private HashSet<Entity> Entities;
        private FrameCounter FrameCounter;

        public FrontForm()
        {
            InitializeComponent();
            InitializeGame();
            FrontForm_Resize(this, EventArgs.Empty);
            FrameCounter = new FrameCounter();

            Entities = new HashSet<Entity>
            {
                new SnakeInkubator().Create(),
                new AppleIncubator().Create()
            };

            List<IProcess> processes = new List<IProcess>();

            processes.Add(new SnakeMovement());

            Bitmap frame = new Bitmap(canvas.ClientSize.Width, canvas.ClientSize.Height);
            canvas.Image = frame;
            SoftwareRender renderProcess = new SoftwareRender(Graphics.FromImage(frame));
            renderProcess.ExecuteEnded += (o, e) =>
            {
                canvas.Refresh();
                FrameCounter.Tick();
                Text = string.Format("FPS: {0:00.00}", FrameCounter.SmoothFPS);
            };
            processes.Add(renderProcess);

            processes.Add(new Collision());

            Processor = new Processor(processes, Entities);
        }

        private void InitializeGame()
        {
            canvas.Width = GameConfig.Width * GameConfig.TileWidth;
            canvas.Height = GameConfig.Height * GameConfig.TileHeight;
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
                SnakeMovement.MovementDirection = movementDirection;
        }
    }
}
