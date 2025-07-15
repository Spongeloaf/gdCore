using Godot;

namespace GdCore;

/// <summary>
/// This class formalizes the concept of a resource path into an object that can be easily
/// converted into a real file path. Also prevents accidentally passing a ResPath string to
/// system directory functions.
/// </summary>
public class ResPath
{
	private enum PathType
	{
		res,
		user,
	}

	private const string projectUserFolderSetting = "application/config/custom_user_dir_name";
	private const string resPrefix = "res://";
	private const string userPrefix = "user://";
	private string _path;
	private string _realPath;
	private PathType _type;

	/// <summary>
	/// Constructs a Resource path with the given string. Cannot be null or empty.
	/// </summary>
	/// <param name="address"></param>
	/// <exception cref="ArgumentNullException">When passed a null or empty string</exception>
	/// <exception cref="ArgumentException">If the given path does not start with 'res://' or 'user://'</exception>
	public ResPath(string? address)
	{
		if (String.IsNullOrEmpty(address))
			throw new ArgumentNullException(nameof(address), "ResPaths cannot be null or empty!");

		if (address.StartsWith(resPrefix))
			_type = PathType.res;
		else if (address.StartsWith(userPrefix))
			_type = PathType.user;
		else
			throw new ArgumentException(nameof(address), $"Not a valid respath: {address}");

		_path = address;
		_realPath = MakeRealRealPath();
	}

	public static implicit operator ResPath(string address)
	{
		// While not technically a requirement; see below why this is done.
		return new ResPath(address);
	}

	public override string ToString()
	{
		return _path;
	}

	public string ToRealPath()
	{
		return _realPath;
	}

	private string MakeRealRealPath()
	{
		// TODO: See if you can if-def this for exported builds
		// Running from an editor binary.
		if (OS.HasFeature("editor"))
			return ProjectSettings.GlobalizePath(_path);

		// Running from an exported project.
		string justThePath = _path.Replace(_type == PathType.res ? resPrefix : userPrefix, "");
		return Path.Join(GetBasePath(), justThePath);
	}

	private string GetBasePath()
	{
		switch (_type)
		{
			default:
			case PathType.res:
				return OS.GetExecutablePath().GetBaseDir();
			case PathType.user:
				return OS.GetUserDataDir();
		}
	}
}