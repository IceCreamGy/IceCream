using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorConst
{
	//依赖文件名字
	public static readonly string depend_text_name = "depend_text.txt";

	//资源名-bundle名文件导出位置
	public static readonly string res2bundle_file = "../cjj-config/define/res2bundle.csv";

	//编辑器下lua文件夹位置
	public static readonly string lua_path_in_editor = "../luacode";

	//windows平台打包资源路径
	public static readonly string windows_out_path = "../common/windows/data";                      //资源路径
	public static readonly string windows_md5_root = "../common/windows";                           //生成md5文件的根目录   
	public static readonly string windows_md5_file = "../common/windows/md5.txt";                   //md5文件  


	//ios平台打包资源路径
	public static readonly string ios_out_path = "../common/ios/data";
	public static readonly string ios_md5_root = "../common/ios";
	public static readonly string ios_md5_file = "../common/ios/md5.txt";


	//andorid平台打包资源路径
	public static readonly string andorid_out_path = "../common/android/data";
	public static readonly string andorid_md5_root = "../common/android";
	public static readonly string andorid_md5_file = "../common/android/md5.txt";

	//streaming目录
	public static readonly string streaming_path = Application.streamingAssetsPath;
}
