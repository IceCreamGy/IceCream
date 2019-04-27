using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameClient : IClient
{
	private string data_path = Application.persistentDataPath + "/cjj_master/data";                 //资源路径
	private string lua_path = Application.persistentDataPath + "/cjj_master/luacode";             //lua路径
	private string persisdent_path = Application.persistentDataPath + "/cjj_master";                //持久化目录
	private string md5_path = Application.persistentDataPath + "/cjj_master/md5.txt";               //持久化目录下的md5
	private string server_temp_md5_path = Application.persistentDataPath + "/cjj_master/server_md5.txt";               //服务器最新md5
	private string depend_text_name = Application.persistentDataPath + "/cjj_master/data/depend_text.txt";            //依赖记录文件                                                                                                               
	private string log_path = Application.persistentDataPath + "/cjj_master/log";                                 //日志目录

	private string streaming_data_path = Application.streamingAssetsPath;                          //随包资源目录
	private string streaming_md5_file = Application.streamingAssetsPath + "/md5.txt";                //随包md5文件


	public string GetDataPath()
	{
		return data_path;
	}

	public abstract string GetHttpServerBundleDir();

	public abstract string GetHttpServerMD5();


	public string GetLogPath()
	{
		return log_path;
	}

	public string GetLuaPath()
	{
		return lua_path;
	}

	public string GetPersisdentMD5File()
	{
		return md5_path;
	}

	public string GetPersisdentPath()
	{
		return persisdent_path;
	}

	public string GetPersisdentServerMD5File()
	{
		return server_temp_md5_path;
	}

	public string GetResDependFile()
	{
		return depend_text_name;
	}

	public string GetStreamingDataPath()
	{
		return streaming_data_path;
	}

	public string GetStreamingMD5File()
	{
		return streaming_md5_file;
	}
}

