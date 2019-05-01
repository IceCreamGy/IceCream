using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Security.Cryptography;

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

	/// <summary>
	/// 路径合并
	/// </summary>
	/// <param name="path1"></param>
	/// <param name="path2"></param>
	/// <returns></returns>
	public static string PathCombine(string path1, string path2)
	{
		string finalPath = path1 + "/" + path2;
		return finalPath;
	}

	/// <summary>
	/// 路径合并
	/// </summary>
	/// <param name="path1"></param>
	/// <param name="path2"></param>
	/// <returns></returns>
	public static string PathCombine(string path1, string path2, string path3)
	{
		string finalPath = path1 + "/" + path2 + "/" + path3;
		return finalPath;
	}

	public static string GetBytesMD5(byte[] data)
	{
		MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
		byte[] returnBytes = md5.ComputeHash(data);
		string strMD5 = BitConverter.ToString(returnBytes);
		strMD5 = strMD5.Replace("-", "");
		return strMD5;
	}
}
