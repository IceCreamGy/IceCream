public class MD5_FileInfo
{
	public string Name { get; private set; }
	public string MD5 { get; private set; }
	public long Size { get; private set; }

	public MD5_FileInfo(string name,string md5, long size)
	{
		this.Name = name;
		this.MD5 = md5;
		this.Size = size;
	}
}