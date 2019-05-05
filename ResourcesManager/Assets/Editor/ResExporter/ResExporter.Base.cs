using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Text;

public partial class ResExporter
{
	/// <summary>
	/// 在指定目录下，搜索对应pattern的资源，生成(资源路径——包路径的映射)
	/// </summary>
	/// <param name="res2bundle_dic">    需要修改的字典</param>
	/// <param name="inputPath">    查询的路径</param>
	/// <param name="outPath">   资源的导出路径</param>
	/// <param name="searchPattern">   过滤参数</param>
	/// <param name="prefix">   前缀</param>
	public static void GetResMap(Dictionary<string, string> res2bundle_dic, string inputPath, string outPath, string searchPattern, string prefix)
	{
		if (!Directory.Exists(StandardlizePath(inputPath)))
		{
			Debug.LogError(string.Format("input path not exist :{0}", inputPath));
		}
		//搜索目录下所有符合条件的资源文件
		string[] files = Directory.GetFiles(inputPath, searchPattern, SearchOption.AllDirectories);
		foreach (string file in files)
		{
			string resPath = StandardlizePath(file);
			string resName = Path.GetFileNameWithoutExtension(resPath).ToLower();
			string bundleName = string.Format("{0}{1}{2}", outPath, prefix, resName);
			res2bundle_dic.Add(resPath, bundleName);
		}
	}
	/// <summary>
	/// 生成依赖资源的数据
	/// </summary>
	/// <param name="res2bundle_dic"></param>
	/// <param name="depend_dic"></param>
	/// <param name="output_data"></param>
	/// <param name="formats"></param>
	/// <param name="depPath"></param>
	/// <param name="前缀"></param>
	public static void GetResDepencies(Dictionary<string, string> res2bundle_dic, ref Dictionary<string, string> depend_dic, string[] formats, string depPath, string prefix)
	{
		foreach (KeyValuePair<string, string> pair in res2bundle_dic)
		{
			string[] dependencies = AssetDatabase.GetDependencies(pair.Key);
			foreach (string d in dependencies)
			{
				foreach (string format in formats)
				{
					if (d.EndsWith(format))
					{
						string bundleName = string.Format("{0}{1}{2}", depPath, prefix, Path.GetFileNameWithoutExtension(d).ToLower());
						depend_dic[d] = bundleName;
					}
				}
			}
		}
	}
	/// <summary>
	/// 清除bundle名字
	/// </summary>
	public static void ClearBundleNames()
	{
		string[] bundleNames = AssetDatabase.GetAllAssetBundleNames();
		foreach (string bundleName in bundleNames)
		{
			AssetDatabase.RemoveAssetBundleName(bundleName, true);
		}
	}
	/// <summary>
	/// 地址标准化
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	public static string StandardlizePath(string path)
	{
		string pathReplace = path.Replace(@"\", @"/");
		string pathLower = pathReplace.ToLower();
		return pathLower;
	}
	/// <summary>
	/// 导出资源
	/// </summary>
	/// <param name="outputPath"></param>
	/// <param name="target"></param>
	public static void Export(string outputPath, BuildTarget target)
	{
		if (!Directory.Exists(outputPath))
		{
			Directory.CreateDirectory(outputPath);
		}
		BuildPipeline.BuildAssetBundles(outputPath, BuildAssetBundleOptions.ChunkBasedCompression, target);
	}
	/// <summary>
	/// 设置资源的bundle名,导出csv，用作资源管理
	/// </summary>
	/// <param name="asset2Bundle"></param>
	public static void SetAssetImporter(Dictionary<string, string> asset2Bundle)
	{
		Dictionary<string, string> asset2bundle_dic = new Dictionary<string, string>();
		string res2bundlePath = EditorConst.res2bundle_file;
		if (File.Exists(res2bundlePath))
		{
			string[] lines = File.ReadAllLines(res2bundlePath);
			foreach (string line in lines)
			{
				string[] parts = line.Split(',');
				asset2bundle_dic[parts[0]] = parts[1];
			}
		}

		AssetImporter assetImporter = null;
		foreach (var pair in asset2Bundle)
		{
			string assetName = Path.GetFileNameWithoutExtension(pair.Key);
			asset2bundle_dic[assetName] = pair.Value;
			assetImporter = AssetImporter.GetAtPath(pair.Key);
			assetImporter.assetBundleName = pair.Value;
		}
		StringBuilder sb = new StringBuilder();
		foreach (var pair in asset2bundle_dic)
		{
			sb.AppendLine(string.Format("{0},{1}", pair.Key, pair.Value));
		}
		File.WriteAllText(res2bundlePath, sb.ToString());


	}
	/// <summary>
	/// 资源数据合并
	/// </summary>
	/// <param name="dic1"></param>
	/// <param name="dic2"></param>
	public static void CombineDictionary(Dictionary<string, string> dic1, Dictionary<string, string> dic2)
	{
		foreach (var pair in dic2)
		{
			dic1.Add(pair.Key, pair.Value);
		}
	}

}
