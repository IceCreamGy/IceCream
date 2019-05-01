using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
	/// <summary>
	/// 标准化路径
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string StandardlizePath(string path)
	{
		string pathReplaca = path.Replace(@"\", @"/");
		//string pathLower = pathReplaca.ToLower();
		//return pathLower;
		return pathReplaca;
	}
}
