using System.Text;

namespace Brick_Breaker
{
    /// <summary>
    /// #How to use
    /// ##Turn on/off data recording
    /// - For Console App : ConsoleDisplay.cs -> Init() method -> brickBreaker.init() method -> set last parameter to true/false
    /// - For Godot : GodotDisplay.cs -> _Ready() method -> brickBreaker.init() method -> set last parameter to true/false
    /// </summary>
    internal class DataRecorder : IDisposable
    {
        private readonly FileStream _stream;
        private readonly StreamWriter _writer;
        private const int THEORICAL_MAX_NB_BRICKS = 100;

        public DataRecorder()
        {
            string fileName = $"game_data_{DateTime.Now:dd-MM-hh-mm-ss}.csv";

            //Path console version  : Brick-Breaker\Console\bin\Debug\net9.0-windows\
            //Path godot version    : Brick-Breaker\Godot\.godot\mono\temp\bin\Debug\
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            //uncomment the line down below to set the path in a better directory. WARNING: works for console, not for godot
            //path should look like this : Brick-Breaker\Dataset\[FileName].csv
            //path = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName+ "\\Dataset", fileName);

            _stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            _writer = new StreamWriter(_stream);

            var bricksHeader = "";
            for (int i = 0; i < THEORICAL_MAX_NB_BRICKS; i++)
            {
                bricksHeader += $";Brick{i}";
            }

            //First line
            _writer.WriteLine($"InputL;InputR;BallPosX;BallPosY;BallVelX;BallVelY;ballSpeed;ballRadius;PaddlePosX;PaddlePosY;PaddleW;PaddleH;PaddleV{bricksHeader}");
            _writer.Flush();
        }

        /// <summary>
        /// Records the data of the game at a specific time step, and save it to a CSV file.
        /// </summary>
        /// <param name="inputL">left input bool01</param>
        /// <param name="inputR">right input bool01</param>
        /// <param name="ballPosX"></param>
        /// <param name="ballPosY"></param>
        /// <param name="ballVelX"></param>
        /// <param name="ballVelY"></param>
        /// <param name="ballSpeed"></param>
        /// <param name="ballRadius"></param>
        /// <param name="paddlePosX"></param>
        /// <param name="paddlePosY"></param>
        /// <param name="paddleW"></param>
        /// <param name="paddleH"></param>
        /// <param name="paddleV"></param>
        /// <param name="bricks">list of brick's health (0 means dead or non-existant brick)</param>
        public void RecordData(
            int inputL, int inputR,
            double ballPosX, double ballPosY,
            double ballVelX, double ballVelY,
            double ballSpeed, double ballRadius,
            double paddlePosX,double paddlePosY,
            double paddleW, double paddleH, double paddleV,
            int[] bricks)
        {
            var sb = new StringBuilder();

            sb.Append($"{inputL};{inputR};{ballPosX};{ballPosY};{ballVelX};{ballVelY};{ballSpeed};{ballRadius};{paddlePosX};{paddlePosY};{paddleW};{paddleH};{paddleV}");

            //set brick healths (0 = dead brick)
            var maxBrickList = new int[THEORICAL_MAX_NB_BRICKS];

            for (int i = 0; i < maxBrickList.Length; i++)
            {
                if (i < bricks.Length)
                    maxBrickList[i] = bricks[i];

                sb.Append(';');
                sb.Append(maxBrickList[i]);
            }

            _writer.WriteLine(sb.ToString());
            _writer.Flush();
        }

        public void Dispose()
        {
            _writer?.Dispose();
        }
    }
}
