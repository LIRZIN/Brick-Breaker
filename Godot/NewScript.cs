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

	[Export] public int W_pixels
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
		double test = GetWindow().Size.X;
		Window window = GetChild(0) as Window;
		W_pixels = window.Size.X;
		H_pixels = window.Size.Y;
		brickBreaker.init(W_pixels, H_pixels);
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
}
