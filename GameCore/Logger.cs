namespace Brick_Breaker
{
    public static class Logger
    {
        public static Action<string>? Info;

        public static void Write(string message)
        {
            Info?.Invoke(message);
        }

        public static void WriteError(string message)
        {
            Info?.Invoke($"[ERROR] {message}");
        }
    }
}
