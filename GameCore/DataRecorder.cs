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
        private FileStream _stream;
        private StreamWriter _writer;

        public DataRecorder(int maxNbBricks)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"game_data{DateTime.Now:'dd'-'MM'-'yy'T'HH':'mm'}.csv");
            _stream = new FileStream(path, FileMode.Create, FileAccess.Write);
            _writer = new StreamWriter(_stream);

            var bricksHeader = "";
            for (int i = 0; i < maxNbBricks; i++)
            {
                bricksHeader += $",Brick{i}";
            }

            _writer.WriteLine($"BallPosX,BallPosY,BallVelX,BallVelY,PaddlePosX,PaddlePosY{bricksHeader}");
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
            double paddlePosX, double paddlePosY,
            int[] bricks)
        {
            var sb = new StringBuilder();

            sb.Append($"{ballPosX},{ballPosY},{ballVelX},{ballVelY},{paddlePosX},{paddlePosY}");
            for (int i = 0; i < bricks.Length; i++)
            {
                sb.Append(',');
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
