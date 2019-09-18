using UnityEngine;
using UnityEngine.Playables;
using UnityEditor;
using System.Collections;
using OfficeOpenXml;
using System;
using System.IO;
using System.Collections.Generic;

public class PoolTableReader : EditorWindow
{
	public UnityEngine.Object ProtoExcelFile = null;
	private static string lastMsg = string.Empty;
	private string selectedExcelPath = "Assets/JoyconFrameWork/3_PoolManager/Table/PoolTable.xlsx";
	private string selectedExportPath = "Assets/JoyconFrameWork/3_PoolManager/DataTable";
	[MenuItem ("DataReader/PoolTable Reader", false, 1)]

	public static void Init(){
		PoolTableReader window = (PoolTableReader)EditorWindow.GetWindow (typeof (PoolTableReader));
	}

	void OnGUI () {
		string filePath = string.Empty;
		string tempPath = string.Empty;

		if (selectedExcelPath != string.Empty)
		{
			filePath = Path.GetFullPath(selectedExcelPath);
		} else {
			ProtoExcelFile = EditorGUILayout.ObjectField(ProtoExcelFile, typeof(UnityEngine.Object), false);
			if( ProtoExcelFile != null){
				filePath = Path.GetFullPath(AssetDatabase.GetAssetPath(ProtoExcelFile));
			}
		}

		if (filePath != string.Empty)
		{
			GUILayout.BeginHorizontal();
			GUILayout.Label("ExcelFile loc : "  + filePath);
			tempPath = "Assets";
			foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
			{
				tempPath = AssetDatabase.GetAssetPath(obj);
				if (File.Exists(tempPath))
				{
					tempPath = Path.GetDirectoryName(tempPath);
				}
				break;
			}
			if(GUILayout.Button("Open Excel File", GUILayout.Height (60)))
			{
				System.Diagnostics.Process.Start(filePath);
			}

			GUILayout.EndHorizontal();

			if( filePath != string.Empty)
			{
				if(GUILayout.Button("Import PoolTable from Excel", GUILayout.Height (100)))
				{
					ReadExcelFile(filePath, selectedExportPath);
				}
			}
		}
		if (lastMsg != string.Empty)
		{
			GUILayout.Label(lastMsg);
		}
	}

	static bool ReadExcelFile (string excelPath, string prefabPath) {
		lastMsg = string.Empty;

		try
		{
			Excel itemData =  ExcelHelper.LoadExcel(excelPath);

			if (itemData == null)
			{
				return false;
			}

			List<ExcelTable> itemDataTable = itemData.Tables;

			string prefabFilePath = prefabPath + "/PoolTable.asset";
			ScriptableObject poolTableSO = (ScriptableObject)AssetDatabase.LoadAssetAtPath(prefabFilePath,typeof(ScriptableObject));
			if(poolTableSO == null)
			{
                poolTableSO = ScriptableObject.CreateInstance<PoolTable>();
                AssetDatabase.CreateAsset(poolTableSO, prefabFilePath);
                poolTableSO.hideFlags = HideFlags.NotEditable;
            }

            PoolTable poolTable = (PoolTable)poolTableSO;

            poolTable.poolInfoList.Clear();

            if (itemDataTable.Count > 0)
            {
                for (int i = 0; i < itemDataTable.Count; i++)
                {
                    if (itemDataTable[i].TableName == "Sheet1")
                    {
                        int indexColumn = 0;
                        int pathColumn = 0;
                        int tagColumn = 0;
                        int preCountColumn = 0;

                        for (int column = 1; column <= itemDataTable[i].NumberOfColumns; column++)
                        {
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Index")
                                indexColumn = column;
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Path")
                                pathColumn = column;
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Tag")
                                tagColumn = column;
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "PreloadCount")
                                preCountColumn = column;
                        }

                        for (int row = 2; row <= itemDataTable[i].NumberOfRows; row++)
                        {
                            if (Convert.ToString(itemDataTable[i].GetValue(row, indexColumn)).Equals(""))
                                continue;

                            int index = Convert.ToInt32(itemDataTable[i].GetValue(row, indexColumn));
                            string path = Convert.ToString(itemDataTable[i].GetValue(row, pathColumn));
                            string tag = Convert.ToString(itemDataTable[i].GetValue(row, tagColumn));
                            int preCount = Convert.ToInt32(itemDataTable[i].GetValue(row, preCountColumn));

                            PoolTable.PoolInfo poolInfo = new PoolTable.PoolInfo();

                            poolInfo.index = index;
                            poolInfo.path = path;
                            poolInfo.tag = tag;
                            poolInfo.preloadCount = preCount;

                            poolTable.poolInfoList.Add(poolInfo);
                        }
                    }
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                EditorUtility.SetDirty(poolTableSO);
                lastMsg = "Succeeded import data to prefab file : " + prefabFilePath;
            }
            else
            {
                lastMsg = "Result : Fail. Reason : Data was not found.";
            }

            return true;
		} catch (Exception e)
		{
			UnityEngine.Debug.Log(e.Message);
			lastMsg = "Result : fail\nMessage : " + e.Message;
			return false;
		} finally {
		}
	}
}
