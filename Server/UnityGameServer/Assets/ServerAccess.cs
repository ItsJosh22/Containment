using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class ServerAccess : MonoBehaviour
{
    const string glyphs = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    int charAmount = 10;
    public string Key;

    string ServerFile = "Access_key.txt";



    // Start is called before the first frame update
    void Start()
    {
        if (!File.Exists(ServerFile))
        {
            
             for (int i = 0; i < charAmount; i++)
              {
             Key += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
             }

            SaveFile();
        }

        LoadFile();

        Debug.Log($"The Server Access Key is {Key}");
    }




   

    public void SaveFile()
    {
        var reader = new StreamWriter(File.Open(ServerFile, FileMode.OpenOrCreate));
        reader.WriteLine(Key);
        reader.Close();
    }

    public void LoadFile()
    {
        var reader = new StreamReader(File.Open(ServerFile, FileMode.Open));
        Key = reader.ReadLine();
        reader.Close();
    }

}
