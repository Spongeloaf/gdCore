
namespace GdCore.Libraries;

public static class RNG
{
	static Random dotNetRandom = new();
	static RandomNumberGenerator godotRandom = new();

	/// <summary>
	/// Returns a rantom integer between min & max, inclusive. Not Thread safe
	/// </summary>
	/// <param name="min"></param>
	/// <param name="max"></param>
	public static int GetRandomInt(int min, int max)
	{
		// The lower bound of Random.Next() is inclusive, but the upper bound is not.
		// We want fully inclusive bounds, without overflow.
		if (max != int.MaxValue)
			max += 1;

		return dotNetRandom.Next(min, max);
	}

	public static float GetRandomAngleRadians()
	{
		return godotRandom.RandfRange(0.0f, MathF.Tau);
	}
}