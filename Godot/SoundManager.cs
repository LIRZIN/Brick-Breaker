using Godot;
using System.Collections.Generic;

public partial class SoundManager : Node
{
    private AudioStreamPlayer player;
    private Dictionary<string, AudioStream> sounds = new Dictionary<string, AudioStream>();

    public override void _Ready()
    {
        player = new AudioStreamPlayer();
        AddChild(player);

        sounds["game-over"] = GD.Load<AudioStream>("res://Audio/game-over-417465.mp3");
        sounds["game-won"] = GD.Load<AudioStream>("res://Audio/good-6081.mp3");
        sounds["brick-die"] = GD.Load<AudioStream>("res://Audio/hurt_c_08-102842.mp3");
        sounds["hit-paddle"] = GD.Load<AudioStream>("res://Audio/retro-game-sfx_jump-bumpwav-14853.mp3");
    }

    public void PlaySound(string name)
    {
        if (sounds.ContainsKey(name))
        {
            player.Stream = sounds[name];
            player.Play();
        }
        else
        {
            GD.PrintErr($"Sound \"{name}\" not found");
        }
    }
}
