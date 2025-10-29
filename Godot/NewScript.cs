using Godot;
using System;
using System.Diagnostics;
using Brick_Breaker;

public partial class NewScript : Node
{
	private BrickBreaker brickBreaker;
	private int w_pixels, h_pixels;
	private char[] displaytab;

	public BrickBreaker BrickBreaker
	{
		get => brickBreaker;
	}

	public int W_pixels
	{
		get => w_pixels;
		set  => w_pixels = value;
	}

	public int H_pixels
	{
		get => h_pixels;
		set => h_pixels = value;
	}

	public override void _Ready()
	{
		base._Ready();
		brickBreaker = new BrickBreaker();
		Window window = GetChild(0) as Window;
		W_pixels = window.Size.X;
		H_pixels = window.Size.Y;
		brickBreaker.init(W_pixels, H_pixels);
		
		//Draw brickWall
		for (int i = 0; i < brickBreaker.getBrickWallAttribute(BrickWallAttribute.BrickCount); i++)
		{
			int tempX = (int)((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionX) * (W_pixels-1)) / Utils.screenSizeWidth);
			int tempY = (int)((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionY) * (H_pixels-1)) / Utils.screenSizeHeight);
			int tempEndX =  (int)(((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionX) + brickBreaker.getBrickAttribute(i, BrickAttribute.Width)) * (W_pixels-1)) / Utils.screenSizeWidth);
			int tempEndY = (int)(((BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionY) + brickBreaker.getBrickAttribute(i, BrickAttribute.Height)) * (H_pixels-1)) / Utils.screenSizeHeight);
			
			ColorRect brickRect = new ColorRect(); 
			brickRect.Size = new Vector2(tempEndX - tempX, tempEndY - tempY); 
			
			brickRect.Position = new Vector2(tempX, tempY);
			
			brickRect.Color = new Color(1, 0, 0);
			
			window.AddChild(brickRect);
		}
		
		//Draw paddle
		int posX = (int)((BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionX) * (W_pixels - 1)) /
						 Utils.screenSizeWidth);
		int posY = (int)((BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionY) * (H_pixels - 1)) /
						 Utils.screenSizeHeight);
		int endX = (int)(((BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionX) +
						   brickBreaker.getPaddleAttribute(PaddleAttribute.Width)) * (W_pixels - 1)) /
						 Utils.screenSizeWidth);
		int endY = (int)(((BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionY) +
						   brickBreaker.getPaddleAttribute(PaddleAttribute.Height)) * (H_pixels - 1)) /
						 Utils.screenSizeHeight);
		ColorRect rect = new ColorRect(); 
		rect.Size = new Vector2(endX - posX, endY - posY); 
			
		rect.Position = new Vector2(posX, posY);
			
		rect.Color = new Color(0, 1, 0);
			
		window.AddChild(rect);
		
		//Draw balls
		for (int i = 0; i < brickBreaker.nbBalls; i++)
		{
			Sprite2D sprite = new Sprite2D();

			Texture2D texture = (Texture2D)GD.Load("res://icon.svg");
			sprite.Texture = texture;

			sprite.Position = new Vector2(
				(int)(BrickBreaker.getBallAttribute(i, BallAttribute.PositionX) * (W_pixels - 1) /
					  Utils.screenSizeWidth),
				(int)(BrickBreaker.getBallAttribute(i, BallAttribute.PositionY) * (H_pixels - 1) /
					  Utils.screenSizeHeight));

			AddChild(sprite);
		}

		SubscribeToEvents();
    }

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		//Update BrickBreaker et input
		/*PlayerMovement movement = PlayerMovement.Nothing;
		if (ConsoleInput.pressingLeft)
		{
			movement = PlayerMovement.Left;
		}
		else if (ConsoleInput.pressingRight)
		{
			movement = PlayerMovement.Right;
		}
		BrickBreaker.update(deltaTime, movement);

		initCharDisplay();*/

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
        if ( e.eventType == EvenType.GameWon)
		{
			GD.Print("You Win!");
            //play sound game won
        }
    }

    private void OnBrickWallLoosesHealthEvent(object sender, Brick_Breaker.EventArgs e)
    {
		if (e.eventType != EvenType.BrickHealthDecreased) return;
		var bw = (BrickWall)e.p[0];
		GD.Print($"Brick has been hit! health remaining : {bw.h}");
    }

	private void OnBallEvent(object sender, Brick_Breaker.EventArgs e)
	{
		if (e.eventType == EvenType.BallBounceOnPaddle)
		{
            GD.Print("Ball hit paddle!");
			//play sound ball hit paddle
        }
		if(e.eventType == EvenType.BallBounceOnWall)
		{
			GD.Print("Ball hit wall!");
        }
		if(e.eventType == EvenType.BallHitBrick)
		{
			GD.Print("Ball hit brick!");
			//play sound ball hit brick
        }
    }
}
