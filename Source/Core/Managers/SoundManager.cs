using System.Collections.Generic;

namespace Mouse_Avoider.Source.Core.Managers;

public static class SoundManager
{
	public static void Initialize()
	{
		// Initialize sound manager
	}

	public static void PlaySound(string soundName)
	{
		// Play sound
	}

	public static void StopSound(string soundName)
	{
		// Stop sound
	}

	public static void StopAllSounds()
	{
		// Stop all sounds
	}

	public static void PauseSound(string soundName)
	{
		// Pause sound
	}

	public static void ResumeSound(string soundName)
	{
		// Resume sound
	}

	public static void ResumeAllSounds()
	{
		// Resume all sounds
	}

	public static void Update()
	{
		// Update sound manager
	}

	public static void LoadSound(string soundName)
	{
		// Load sound
	}

	public static void UnloadSound(string soundName)
	{
		// Unload sound
	}

	public static void UnloadAllSounds()
	{
		// Unload all sounds
	}

	public static void SetVolume(string soundName, float volume)
	{
		// Set volume
	}

	public static void SetPitch(string soundName, float pitch)
	{
		// Set pitch
	}

	public static void SetPan(string soundName, float pan)
	{
		// Set pan
	}

	public static void SetLooping(string soundName, bool looping)
	{
		// Set looping
	}

	// sounds list field
	private static List<string> sounds = new List<string>();

	public static List<string> GetSounds()
	{
		// Get sounds
		return sounds;
	}
}