using UnityEngine;
using UnityEngine.Playables;
using UnityEditor;
using System.Collections;
using OfficeOpenXml;
using System;
using System.IO;
using System.Collections.Generic;

public class BasicDataTableReader : EditorWindow
{
	public UnityEngine.Object ProtoExcelFile = null;
	private static string lastMsg = string.Empty;
	private string selectedExcelPath = "Assets/JoyconFrameWork/1_ExcelDataReader/Editor/Table/BasicDataTable.xlsx";
	private string selectedExportPath = "Assets/JoyconFrameWork/Resources/BasicDataTable";
	[MenuItem ("DataReader/BasicDataTable Reader", false, 1)]

    public static void Init()
    {
        BasicDataTableReader window = (BasicDataTableReader)EditorWindow.GetWindow(typeof(BasicDataTableReader));
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
				if(GUILayout.Button("Import BasicDataTable from Excel", GUILayout.Height (100)))
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
		GameObject basicTableInstance = null;
		lastMsg = string.Empty;

		try
		{
			Excel itemData =  ExcelHelper.LoadExcel(excelPath);

			if (itemData == null)
			{
				return false;
			}

			List<ExcelTable> itemDataTable = itemData.Tables;

			string prefabFilePath = prefabPath + "/BasicDataTable.prefab";
			GameObject basicTablePrefab = (GameObject)AssetDatabase.LoadAssetAtPath(prefabFilePath,typeof(GameObject));
			if(basicTablePrefab == null)
			{
				basicTableInstance = new GameObject("BasicDataTable");
				basicTableInstance.AddComponent<BasicDataTable>();
                basicTablePrefab = PrefabUtility.CreatePrefab(prefabFilePath,basicTableInstance);
			}

			basicTableInstance = (GameObject)PrefabUtility.InstantiatePrefab(basicTablePrefab);

            BasicDataTable basicDataTable = basicTableInstance.GetComponent<BasicDataTable>();

            BasicInfo[] childObj = basicDataTable.GetComponentsInChildren<BasicInfo>();
            for (int i = 0; i < childObj.Length; i++)
            {
                DestroyImmediate(childObj[i].gameObject);
            }

            basicDataTable.basicInfoList1.Clear();
            basicDataTable.basicInfoList2.Clear();

            if (itemDataTable.Count > 0)
            {
                for (int i = 0; i < itemDataTable.Count; i++)
                {
                    if (itemDataTable[i].TableName == "Sheet1")
                    {
                        int indexColumn = 0;
                        int stringValueColumn = 0;
                        int intValueColumnColumn = 0;
                        int floatValueColumn = 0;

                        for (int column = 1; column <= itemDataTable[i].NumberOfColumns; column++)
                        {
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Index")
                                indexColumn = column;
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "StringValue")
                                stringValueColumn = column;
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "IntValue")
                                intValueColumnColumn = column;
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "FloatValue")
                                floatValueColumn = column;
                        }

                        for (int row = 2; row <= itemDataTable[i].NumberOfRows; row++)
                        {
                            if (Convert.ToString(itemDataTable[i].GetValue(row, indexColumn)).Equals(""))
                                continue;

                            int index = Convert.ToInt32(itemDataTable[i].GetValue(row, indexColumn));
                            string stringValue = Convert.ToString(itemDataTable[i].GetValue(row, stringValueColumn));
                            int intValue = Convert.ToInt32(itemDataTable[i].GetValue(row, intValueColumnColumn));
                            float floatValue = Convert.ToSingle(itemDataTable[i].GetValue(row, floatValueColumn));

                            GameObject basicInfoObj = new GameObject(string.Format("BasicInfo {0}", index));
                            basicInfoObj.transform.parent = basicTableInstance.transform;
                            BasicInfo basicInfo = basicInfoObj.AddComponent<BasicInfo>();

                            basicInfo.index = index;
                            basicInfo.stringValue = stringValue;
                            basicInfo.intValue = intValue;
                            basicInfo.floatValue = floatValue;

                            basicDataTable.basicInfoList1.Add(basicInfo);
                        }
                    }
                    else if (itemDataTable[i].TableName == "Sheet2")
                    {
                        int indexColumn = 0;
                        int stringValueColumn = 0;
                        int intValueColumnColumn = 0;
                        int floatValueColumn = 0;

                        for (int column = 1; column <= itemDataTable[i].NumberOfColumns; column++)
                        {
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "Index")
                                indexColumn = column;
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "StringValue")
                                stringValueColumn = column;
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "IntValue")
                                intValueColumnColumn = column;
                            if (Convert.ToString(itemDataTable[i].GetValue(1, column)) == "FloatValue")
                                floatValueColumn = column;
                        }

                        for (int row = 2; row <= itemDataTable[i].NumberOfRows; row++)
                        {
                            if (Convert.ToString(itemDataTable[i].GetValue(row, indexColumn)).Equals(""))
                                continue;

                            int index = Convert.ToInt32(itemDataTable[i].GetValue(row, indexColumn));
                            string stringValue = Convert.ToString(itemDataTable[i].GetValue(row, stringValueColumn));
                            int intValue = Convert.ToInt32(itemDataTable[i].GetValue(row, intValueColumnColumn));
                            float floatValue = Convert.ToSingle(itemDataTable[i].GetValue(row, floatValueColumn));

                            GameObject basicInfoObj = new GameObject(string.Format("BasicInfo {0}", index));
                            basicInfoObj.transform.parent = basicTableInstance.transform;
                            BasicInfo basicInfo = basicInfoObj.AddComponent<BasicInfo>();

                            basicInfo.index = index;
                            basicInfo.stringValue = stringValue;
                            basicInfo.intValue = intValue;
                            basicInfo.floatValue = floatValue;

                            basicDataTable.basicInfoList2.Add(basicInfo);
                        }
                    }
                }

                //GameObject newPrefab;

                //newPrefab = PrefabUtility.ReplacePrefab(basicTableInstance, basicTablePrefab, ReplacePrefabOptions.ConnectToPrefab);
                PrefabUtility.ReplacePrefab(basicTableInstance, basicTablePrefab, ReplacePrefabOptions.ConnectToPrefab);

                DestroyImmediate(basicTableInstance);
                basicTableInstance = null;

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
			if (basicTableInstance != null)
				DestroyImmediate(basicTableInstance);
		}
	}
}
