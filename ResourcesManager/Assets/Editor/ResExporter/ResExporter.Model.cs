using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public partial class ResExporter
{

	public static Dictionary<string, string> model_res_bundle_dic;
	public static Dictionary<string, string> model_dep_res_bundle_dic;


	[MenuItem("Export/Windows/Export Model For Windows")]
	public static void ExportModelForWindows()
	{
		ExportModel(EditorConst.windows_out_path, BuildTarget.StandaloneWindows);
	}

	[MenuItem("Export/Android/Export Model For Android")]
	public static void ExportModelForAndroid()
	{
		ExportModel(EditorConst.andorid_out_path, BuildTarget.Android);
	}

	[MenuItem("Export/IOS/Export Model For IOS")]
	public static void ExportModelForIOS()
	{
		ExportModel(EditorConst.ios_out_path, BuildTarget.iOS);
	}


	public static void ExportModel(string outpath, BuildTarget target)
	{
		model_res_bundle_dic = new Dictionary<string, string>();
		model_dep_res_bundle_dic = new Dictionary<string, string>();
		GetResMap(model_res_bundle_dic, "Assets/BundleRes/Model", "model/", "*.prefab", "model_");
		GetResDepencies(model_res_bundle_dic, ref model_dep_res_bundle_dic, new string[] { "png", "jpg" }, "texture/", "t_");
		//GetResDepencies(model_res_bundle_dic, ref model_dep_res_bundle_dic, new string[] { "mat" }, "mat/", "m_");
		GetResDepencies(model_res_bundle_dic, ref model_dep_res_bundle_dic, new string[] { "shader" }, "shader/", "s_");
		CombineDictionary(model_res_bundle_dic, model_dep_res_bundle_dic);
		ClearBundleNames();
		SetAssetImporter(model_res_bundle_dic);
		Export(outpath, target);
		ExportBundleDependInfo(outpath);
		ClearBundleNames();
	}
}

