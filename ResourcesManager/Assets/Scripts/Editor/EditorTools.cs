using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;

public class EditorTools : MonoBehaviour
{

	[MenuItem("Tools/OpenPersistent")]
	static void OpenPersistent()
	{
		Process.Start(Application.persistentDataPath);
	}
}
