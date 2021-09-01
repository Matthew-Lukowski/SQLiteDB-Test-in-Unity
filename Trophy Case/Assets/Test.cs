using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System;
using System.Drawing;
using System.Windows.Forms;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Drawing.Imaging;

public class Test : MonoBehaviour
{

    public GameObject text;
    public GameObject nameBox;
    public GameObject sportBox;
    public GameObject displayedImage;

    private IDbConnection dbconn;
    private IDbCommand dbcmd;

    private string conn;

    public string filepath = "";

    private void Start() {
        conn = "URI=file:" + UnityEngine.Application.dataPath + "/Database.db";
    }


    public void UploadImg() {

        OpenFileDialog fileDialog = new OpenFileDialog();

        if (DialogResult.OK == fileDialog.ShowDialog()) {
            filepath = fileDialog.FileName;
        }
        text.GetComponent<Text>().text = filepath;

    }
    


    void ReadDatabase()
    {
        ConnectDB();

        string sqlQuery = "SELECT value,name, randomSequence " + "FROM PlaceSequence";

        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read())
        {
            int value = reader.GetInt32(0);
            string name = reader.GetString(1);
            int rand = reader.GetInt32(2);

            Debug.Log("value= " + value + "  name =" + name + "  random =" + rand);
        }

        reader.Close();
        CloseDB();
    }

     public void UploadItemsToDB() {


        ConnectDB();


        //Take a picture from the filepath and turn it into a byte array and into a string

        System.Drawing.Image picture = System.Drawing.Image.FromFile(filepath);
        MemoryStream tmpStream = new MemoryStream();
        picture.Save(tmpStream, ImageFormat.Png);
        tmpStream.Seek(0, SeekOrigin.Begin);
        byte[] blob = tmpStream.ToArray();
        string imageToString = Convert.ToBase64String(blob);



        //Convert string to image and display as texture on image.
        
        //byte[] testarrayimage = Convert.FromBase64String(imageToString);
        //Texture2D tex = new Texture2D(2, 2);
        //tex.LoadImage(testarrayimage);
        //displayedImage.GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), Vector2.zero); ;
        

        //Get Text from Components
        string nb = nameBox.GetComponent<InputField>().text;
        List<Dropdown.OptionData> menuOption = sportBox.GetComponent<Dropdown>().options;
        string sb = menuOption[sportBox.GetComponent<Dropdown>().value].text;


        //Make query and execute it to the db.
        string sqlQuery = "INSERT INTO testtable (Name, Sport, Image) VALUES(\""+ nb + "\",\"" + sb + "\", \"" + imageToString +"\")";
        dbcmd.CommandText = sqlQuery;
        dbcmd.ExecuteNonQuery();



        CloseDB();
    }

    public void testFetchFromDB() {

        ConnectDB();
        //SELECT max(Id) FROM testtable
        string sqlQuery = "SELECT Image FROM testtable WHERE Id=(18)";
        dbcmd.CommandText = sqlQuery;
        IDataReader reader = dbcmd.ExecuteReader();

        while (reader.Read()) {

            string imgstring = reader.GetString(0);

            byte[] testarrayimage = Convert.FromBase64String(imgstring);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(testarrayimage);
            displayedImage.GetComponent<UnityEngine.UI.Image>().sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), Vector2.zero);
            displayedImage.GetComponent<UnityEngine.UI.Image>().preserveAspect = true;
        }



        reader.Close();
        CloseDB();

    }



    void ConnectDB() {

        //Connect To DB and prepare a command.

        dbconn = new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        dbcmd = dbconn.CreateCommand();

    }

    void CloseDB() {
        //Close database and command.
        dbcmd.Dispose();
        dbconn.Close();
    }
    

}
