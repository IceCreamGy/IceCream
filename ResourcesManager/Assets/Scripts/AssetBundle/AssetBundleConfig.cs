using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System;

[Serializable]
public class AssetBundleConfig
{
	[XmlElement("ABList")]
	public List<ABBase> ABList { get; set; }
}

[Serializable]
public class ABBase
{
	[XmlAttribute("Crc")]
	public uint Crc { get; set; }
	[XmlAttribute("ABName")]
	public string ABName { get; set; }
	[XmlAttribute("AssetName")]
	public string AssetName { get; set; }
	[XmlAttribute("Path")]
	public string Path { get; set; }
	[XmlElement("ABDependce")]
	public List<string> ABDependce { get; set; }
}
