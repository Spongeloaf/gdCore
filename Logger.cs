using System.Text;
using Godot;


// Logs to visual studio debug output and godot console
// There's a copy of this in both the base project and the utility project. If you overhaul this, please duplicate it there!
namespace GdCore;

[Flags]
public enum DebugCategory : ulong
{
	Animations = 1,
	PawnController = 2,
	GameWorld = 4,
	Models = 8,
	Turrets = 16,
}

public static class Log
{
	public static DebugCategory Categories { get; private set; } = 0u;
	private const string categoryHeader = "Active debug logging categories: Uncategorized";
	public static void EnableCategory(DebugCategory cat)
	{
		Categories |= cat;
		PrintCurrentCategories();
	}

	public static void DisableCategory(DebugCategory cat)
	{
		Categories &= ~cat;
		PrintCurrentCategories();
	}

	public static bool IsDebugCategoryActive(DebugCategory cat)
	{
		return (cat & Categories) == cat;
	}

	private static void PrintCurrentCategories()
	{
		DebugCategory[] categories = Enum.GetValues<DebugCategory>();
		StringBuilder sb = new StringBuilder((categories.Length * 32) + categoryHeader.Length);
		sb.Append(categoryHeader);
		foreach (DebugCategory cat in categories)
		{
			if ((cat & Categories) != cat)
				continue;

			sb.Append(", ");
			sb.Append(cat.ToString());
		}

		DebugNoFormat(sb.ToString());
	}

	/// <summary>
	/// Uncategorized debug logging. Will always be printed, regardless of category settings.
	/// </summary>
	/// <param name="message"></param>
	public static void DebugNoFormat(string message)
	{
		GD.Print(message);
	}

	public static void DebugNoFormat(DebugCategory cat, string message)
	{
		if (IsDebugCategoryActive(cat))
			GD.Print(message);
	}

	public static void Debug(string message, params object[] formatValues)
	{
		// TODO: skip formatting and return if logging level is disabled
		GD.Print(string.Format(message, formatValues));
	}

	public static void Debug(DebugCategory cat, string message, params object[] formatValues)
	{
		if (IsDebugCategoryActive(cat))
			GD.Print(string.Format(message, formatValues));
	}

	public static void GameErrorNoFormat(string message)
	{
		GD.PrintErr(message);
	}

	public static void GameError(string message, params object[] formatValues)
	{
		// TODO: skip formatting and return if logging level is disabled
		GD.PrintErr(string.Format(message, formatValues));
	}

	public static void GameInfo(string message, params object[] formatValues)
	{
		// TODO: skip formatting and return if logging level is disabled
		GD.Print(string.Format(message, formatValues));
	}

	public static void UserErrorNoFormat(string message)
	{
		// TODO: skip formatting and return if logging level is disabled
		GD.Print(message);
	}

	public static void UserError(string message, params object[] formatValues)
	{
		// TODO: skip formatting and return if logging level is disabled
		GD.Print(message);
	}

	public static void PrintHumanReadableTransform(in Transform3D tfm, string tag)
	{
		var scale = tfm.Basis.Scale;
		var euler = tfm.Basis.GetEuler();
		euler = new(Mathf.RadToDeg(euler.X), Mathf.RadToDeg(euler.Y), Mathf.RadToDeg(euler.Z));
		string? tagActual = tag is null ? "Transform3d: " : tag + ": ";
		GD.Print($"{tagActual} Offset: {tfm.Origin}, Euler: {euler}, Scale: {scale}");
	}

}