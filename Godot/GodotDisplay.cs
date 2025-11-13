using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Brick_Breaker;

public partial class GodotDisplay : Node
{
	private BrickBreaker brickBreaker;
	private int w_pixels, h_pixels;
	private Window window;
	private ColorRect paddleRect;
	private readonly Vector2 spriteSize = new Vector2(600, 600);
	private Sprite2D ballSprite;
	[Export] public double ballSpeed = 0.5;
	[Export] public double PaddleSpeed = 1;
	[Export] public double ballRadius = 0.02;
	private List<ColorRect> listBrickRect = new List<ColorRect>();

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
		window.Position = new Vector2I(0, 0);
		W_pixels = window.Size.X;
		H_pixels = window.Size.Y;	

		brickBreaker.init(W_pixels, H_pixels, true);
		BrickBreaker.SetBallSpeed(ballSpeed);
		BrickBreaker.SetPaddleSpeed(PaddleSpeed);
		BrickBreaker.SetBallRadius(ballRadius);
		
		//Draw brickWall
		for (int i = 0; i < brickBreaker.getBrickWallAttribute(BrickWallAttribute.BrickCount); i++)
		{
			float tempX = GetPositionX(BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionX));
			float tempY = GetPositionY(BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionY));
			float tempEndX = GetPositionX(BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionX) +
										  brickBreaker.getBrickAttribute(i, BrickAttribute.Width));
			float tempEndY = GetPositionY(BrickBreaker.getBrickAttribute(i, BrickAttribute.PositionY) +
										  brickBreaker.getBrickAttribute(i, BrickAttribute.Height));
			listBrickRect.Add(new ColorRect());
			listBrickRect[i].Size = new Vector2(tempEndX - tempX, tempEndY - tempY);

			listBrickRect[i].Position = new Vector2(tempX, tempY);
			
			switch (BrickBreaker.getBrickAttribute(i, BrickAttribute.Health))
			{
				case 0:
					GD.Print("la brique est dead");
					break;
				case 1:
					listBrickRect[i].Color = new Color(0, 1, 0);
					break;
				case 2:
					listBrickRect[i].Color = new Color(0, 0, 1);
					break;
				case 3:
					listBrickRect[i].Color = new Color(1, 0, 0);
					break;
				default:
					GD.Print("defaut case");
					break;
				
			}

			window.AddChild(listBrickRect[i]);
		}

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
		ballSprite = new Sprite2D();

		Texture2D texture = (Texture2D)GD.Load("res://ball.png");
		ballSprite.Texture = texture;

		ballSprite.Position = new Vector2(
			GetPositionX(BrickBreaker.getBallAttribute(0, BallAttribute.PositionX)),
			GetPositionY(BrickBreaker.getBallAttribute(0, BallAttribute.PositionY)));

		Vector2 scale = new Vector2();
		scale.X = (float)(BrickBreaker.getBallAttribute(0, BallAttribute.Radius) * h_pixels) / spriteSize.X;
		scale.Y = scale.X;
		ballSprite.SetScale(scale);

		window.AddChild(ballSprite);

		SubscribeToEvents();
		//GetNode<SoundManager>("AudioStreamPlayer").PlaySound("game-over");
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		
		//input pour quitter le jeu
		if (Input.IsActionPressed("QuitGame"))
		{
			GetTree().Quit();
		}

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
		BrickBreaker.update(delta, movement, true);

		//Update Paddle
		paddleRect.Position = new Vector2(
			GetPositionX(BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionX)),
				GetPositionY(BrickBreaker.getPaddleAttribute(PaddleAttribute.PositionY)));

		//Update Ball
		ballSprite.Position = new Vector2(
		GetPositionX(BrickBreaker.getBallAttribute(0, BallAttribute.PositionX)),
		GetPositionY(BrickBreaker.getBallAttribute(0, BallAttribute.PositionY)));
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
			GetNode<SoundManager>("AudioStreamPlayer").PlaySound("game-over");
            var label = GetChild(2) as Label;
            window.Visible = false;
            label.Visible = true;
        }
		if (e.eventType == EvenType.GameWon)
		{
			GD.Print("You Win!");
            GetNode<SoundManager>("AudioStreamPlayer").PlaySound("game-won");
			var label = GetChild(1) as Label;
			window.Visible = false;
            label.Visible = true;
		}
	}

	private void OnBrickWallLoosesHealthEvent(object sender, Brick_Breaker.EventArgs e)
	{
		if (e.eventType != EvenType.BrickHealthDecreased) return;
		var index = (int)e.p[0];
		if (BrickBreaker.getBrickAttribute(index, BrickAttribute.Health) <= 0)
		{
			listBrickRect[index].QueueFree();
		}

		switch (BrickBreaker.getBrickAttribute(index, BrickAttribute.Health))
		{
			case 0:
				listBrickRect[index].QueueFree();
				listBrickRect.RemoveAt(index);
				break;
			case 1:
				listBrickRect[index].Color = new Color(0, 1, 0);
				break;
			case 2:
				listBrickRect[index].Color = new Color(0, 0, 1);
				break;
			case 3:
				listBrickRect[index].Color = new Color(1, 0, 0);
				break;
			default:
				GD.Print("defaut case");
				break;
				
		}
	}

	private void OnBallEvent(object sender, Brick_Breaker.EventArgs e)
	{
		if (e.eventType == EvenType.BallBounceOnPaddle)
		{
			GD.Print("Ball hit paddle!");
			GetNode<SoundManager>("AudioStreamPlayer").PlaySound("hit-paddle");
		}
		if (e.eventType == EvenType.BallBounceOnWall)
		{
			GD.Print("Ball hit wall!");
			GetNode<SoundManager>("AudioStreamPlayer").PlaySound("brick-die");

		}
		if (e.eventType == EvenType.BallHitBrick)
		{
			GD.Print("Ball hit brick!");
			GetNode<SoundManager>("AudioStreamPlayer").PlaySound("brick-die");
		}
	}
}
