using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brick_Breaker
{
    internal class DataRecorder : IDisposable
    {
        private readonly FileStream _stream;
        private readonly StreamWriter _writer;

        public DataRecorder(int maxNbBricks)
        {
            string fileName = $"game_data_{DateTime.Now:hh-mm-ss}.csv";

            //for console : Brick-Breaker\Console\bin\Debug\net9.0-windows\
            //for godot   : Brick-Breaker\Godot\.godot\mono\temp\bin\Debug\
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            //uncomment the line down below to set the path in a better directory. WARNING: works for console, not for godot
            //path should look like this : Brick-Breaker\Dataset\[FileName].csv
            //path = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName+ "\\Dataset", fileName);

            _stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            _writer = new StreamWriter(_stream);

            var bricksHeader = "";
            for (int i = 0; i < maxNbBricks; i++)
            {
                bricksHeader += $";Brick{i}";
            }

            //First line
            _writer.WriteLine($"BallPosX;BallPosY;BallVelX;BallVelY;PaddlePosX{bricksHeader}");
            _writer.Flush();
        }

        /// <summary>
        /// Records the data of the game at a specific time step, and save it to a CSV file.
        /// </summary>
        /// <param name="ballPosX">ball position X</param>
        /// <param name="ballPosY">ball position Y</param>
        /// <param name="ballVelX">ball velocity X</param>
        /// <param name="ballVelY">ball velocity Y</param>
        /// <param name="paddlePosX">paddle position X</param>
        /// <param name="paddlePosY">paddle position Y</param>
        /// <param name="bricks">array of brick's health (0 = dead or non-existant brick)</param>
        public void RecordData(
            double ballPosX, double ballPosY,
            double ballVelX, double ballVelY,
            double paddlePosX,
            int[] bricks)
        {
            var sb = new StringBuilder();

            sb.Append($"{ballPosX};{ballPosY};{ballVelX};{ballVelY};{paddlePosX}");
            for (int i = 0; i < bricks.Length; i++)
            {
                sb.Append(';');
                sb.Append(bricks[i]);
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
