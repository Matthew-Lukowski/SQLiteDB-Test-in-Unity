using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class TrophyDisplay : MonoBehaviour
{
    public GameObject txtBox;
    public GameObject drpBox;
    public GameObject recPrefab;
    public Transform listOfRecords;

    private List<Trophy> Trophies = new List<Trophy>();

    private IDbConnection dbconn;
    private IDbCommand dbcmd;

    private string conn;

    private void Start() {
        conn = "URI=file:" + UnityEngine.Application.dataPath + "/Database.db";
    }

    public void LoadRecords() {

        ConnectDB();
        Trophies.Clear();


        string nb = txtBox.GetComponent<InputField>().text;
        List<Dropdown.OptionData> menuOption = drpBox.GetComponent<Dropdown>().options;
        string sb = menuOption[drpBox.GetComponent<Dropdown>().value].text;

        string sqlQuery;

        if (string.IsNullOrEmpty(nb)){ sqlQuery = string.Format("Select * FROM testtable WHERE Sport = \"{0}\"", sb); } 
        else { sqlQuery = string.Format("Select * FROM testtable WHERE Name LIKE \'%{0}%\' AND Sport = \"{1}\"", nb, sb); }



        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read()) {
            string _1,_2,_3;

            try { _1 = reader.GetString(1); } catch (Exception) { _1 = ""; }
            try { _2 = reader.GetString(2); } catch (Exception) { _2 = ""; }
            try { _3 = reader.GetString(3); } catch (Exception) { _3 = ""; }

            Trophies.Add(new Trophy(_1,_2,_3));         
        }

        ShowRecords();

        reader.Close();
        CloseDB();
    }

    private void ShowRecords() {

        foreach (GameObject gp in GameObject.FindGameObjectsWithTag("Record")) {
            Destroy(gp);
        }


        for (int i = 0; i < Trophies.Count ; i++) {
            GameObject tempRecord = Instantiate(recPrefab);
            Trophy tmpTrophy = Trophies[i];

            tempRecord.GetComponent<NewRecord>().setTextBoxes(tmpTrophy.Name,tmpTrophy.Sport);

            tempRecord.transform.SetParent(listOfRecords);

            tempRecord.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);

        }


    }


    void ConnectDB() {
        dbconn = new SqliteConnection(conn);
        dbconn.Open();
        dbcmd = dbconn.CreateCommand();

    }

    void CloseDB() {
        dbcmd.Dispose();
        dbconn.Close();
    }
}
