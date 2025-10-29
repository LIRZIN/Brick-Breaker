namespace Brick_Breaker;
public class EventArgs(EvenType eventType, params object[] p) : System.EventArgs
{
    public readonly EvenType eventType = eventType;
    public readonly object[] p = p;
}