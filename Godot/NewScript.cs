using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Brick_Breaker;

public partial class NewScript : Node
{
    private BrickBreaker brickBreaker;
    private int w_pixels, h_pixels;
    private Window window;
    private ColorRect paddleRect;
    private readonly Vector2 spriteSize = new Vector2(600, 600);
    private Sprite2D ballSprite;
    [Export] public double ballSpeed = 1;
    [Export] public double PaddleSpeed = 1;

    public BrickBreaker BrickBreaker
    {
        get => brickBreaker;
    }

    public int W_pixels
    {
        get => w_pixels;
        set => w_pixels = value;
    }

    public int H_pixels
    {
        get => h_pixels;
        set => h_pixels = value;
    }

    public float GetPositionX(double PositionX)
    {
        return (float)((PositionX * (W_pixels - 1)) / Utils.screenSizeWidth);
    }
    public float GetPositionY(double PositionY)
    {
        return (float)((PositionY * (H_pixels - 1)) / Utils.screenSizeHeight);
    }

    public override void _Ready()
    {
        base._Ready();
        brickBreaker = new BrickBreaker();
        window = GetChild(0) as Window;
        W_pixels = window.Size.X;
        H_pixels = window.Size.Y;

        brickBreaker.init(W_pixels, H_pixels);
        BrickBreaker.SetBallSpeed(ballSpeed);
        BrickBreaker.SetPaddleSpeed(PaddleSpeed);

        //Draw paddle
        float posX = GetPositionX(BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionX));
        float posY = GetPositionY(BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionY));
        float endX = GetPositionX(BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionX) +
                                  brickBreaker.getPaddleAttribute(PaddleAttribute.Width));
        float endY = GetPositionY(BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionY) +
                                  brickBreaker.getPaddleAttribute(PaddleAttribute.Height));
        paddleRect = new ColorRect();
        paddleRect.Size = new Vector2(endX - posX, endY - posY);

        paddleRect.Position = new Vector2(posX, posY);

        paddleRect.Color = new Color(0, 1, 0);

        window.AddChild(paddleRect);

        //Draw balls
        GD.Print(0);
        ballSprite = new Sprite2D();

        Texture2D texture = (Texture2D)GD.Load("res://ball.png");
        ballSprite.Texture = texture;

        GD.Print(1);
        ballSprite.Position = new Vector2(
            GetPositionX(BrickBreaker.getBallAttribute(0, BallAttribute.PositionX)),
            GetPositionY(BrickBreaker.getBallAttribute(0, BallAttribute.PositionY)));

        Vector2 scale = new Vector2();
        scale.X = (float)(BrickBreaker.getBallAttribute(0, BallAttribute.Radius) * h_pixels) / spriteSize.X;
        scale.Y = scale.X;
        ballSprite.SetScale(scale);

        GD.Print(GetPositionX(BrickBreaker.getBallAttribute(0, BallAttribute.PositionX)));
        window.AddChild(ballSprite);

        SubscribeToEvents();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        //Update BrickBreaker et input
        PlayerMovement movement = PlayerMovement.Nothing;
        if (Input.IsActionPressed("MoveLeft"))
        {
            movement = PlayerMovement.Left;
        }
        else if (Input.IsActionPressed("MoveRight"))
        {
            movement = PlayerMovement.Right;
        }
        BrickBreaker.update(delta, movement);

        //Draw brickWall
        for (int i = 0; i < brickBreaker.getBrickWallAttribute(BrickWallAttribute.BrickCount); i++)
        {
            int tempX = (int)((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionX) * (W_pixels - 1)) / Utils.screenSizeWidth);
            int tempY = (int)((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionY) * (H_pixels - 1)) / Utils.screenSizeHeight);
            int tempEndX = (int)(((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionX) + brickBreaker.getBrickAttribute(i, BrickAttribute.Width)) * (W_pixels - 1)) / Utils.screenSizeWidth);
            int tempEndY = (int)(((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionY) + brickBreaker.getBrickAttribute(i, BrickAttribute.Height)) * (H_pixels - 1)) / Utils.screenSizeHeight);

            ColorRect brickRect = new ColorRect();
            brickRect.Size = new Vector2(tempEndX - tempX, tempEndY - tempY);

            brickRect.Position = new Vector2(tempX, tempY);

            brickRect.Color = new Color(1, 0, 0);

            window.AddChild(brickRect);
        }

        //Update Paddle
        paddleRect.Position = new Vector2(
            GetPositionX(BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionX)),
                GetPositionY(BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionY)));

        ballSprite.Position = new Vector2(
        GetPositionX(BrickBreaker.getBallAttribute(0, BallAttribute.PositionX)),
        GetPositionY(BrickBreaker.getBallAttribute(0, BallAttribute.PositionY)));
        GD.Print(GetPositionX(BrickBreaker.getBallAttribute(0, BallAttribute.PositionX)));
    }

    private void SubscribeToEvents()
    {
        brickBreaker.BrickWall.Event += OnBrickWallLoosesHealthEvent;
        brickBreaker.BallManager.getBall(0).Event += OnBallEvent;
        brickBreaker.Event += OnMainGameEvent;
    }

    private void UnsibscribeToEvents()
    {
        brickBreaker.BrickWall.Event -= OnBrickWallLoosesHealthEvent;
        brickBreaker.BallManager.getBall(0).Event -= OnBallEvent;
        brickBreaker.Event -= OnMainGameEvent;
    }

    private void OnMainGameEvent(object sender, Brick_Breaker.EventArgs e)
    {
        if (e.eventType == EvenType.GameOver)
        {
            GD.Print("Game Over!");
            //play sound game over
        }
        if (e.eventType == EvenType.GameWon)
        {
            GD.Print("You Win!");
            //play sound game won
        }
    }

    private void OnBrickWallLoosesHealthEvent(object sender, Brick_Breaker.EventArgs e)
    {
        if (e.eventType != EvenType.BrickHealthDecreased) return;
        var hp = (int)e.p[0];
        GD.Print($"Brick has been hit! health remaining : {hp}");
    }

    private void OnBallEvent(object sender, Brick_Breaker.EventArgs e)
    {
        if (e.eventType == EvenType.BallBounceOnPaddle)
        {
            GD.Print("Ball hit paddle!");
            //play sound ball hit paddle
        }
        if (e.eventType == EvenType.BallBounceOnWall)
        {
            GD.Print("Ball hit wall!");
        }
        if (e.eventType == EvenType.BallHitBrick)
        {
            GD.Print("Ball hit brick!");
            //play sound ball hit brick
        }
    }
}
