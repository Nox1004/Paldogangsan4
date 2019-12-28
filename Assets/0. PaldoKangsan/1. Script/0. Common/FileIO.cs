using System.IO;
using UnityEngine;

// <summary>
// 파일입출력을 위한 정적클래스이다.
// </summary>
public static class FileIO
{
    public static void WriteStringToFile(FileMode fileMode, string contents, string folder, string fileName)
    {
        string path = PathForDocumentsFile(folder);

        if(!Directory.Exists(path))
        {

        #if UNITY_EDITOR
            Debug.Log("해당경로에 없습니다. 생성합니다.");
        #endif

            Directory.CreateDirectory(path);
        }

        path = PathForDocumentsFile(folder + "/" + fileName);

        FileStream file = new FileStream(path, fileMode, FileAccess.Write);

        StreamWriter sw = new StreamWriter(file);
        sw.WriteLine(contents);

        sw.Close();
        file.Close();
    }

    public static string ReadStringFromFile(string fileName)
    {
        string path = PathForDocumentsFile(fileName);
        string str = null;

        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            str = sr.ReadToEnd();

            sr.Close();
            file.Close();
        }

        return str;
    }
    
    public static string PathForDocumentsFile(string fileName)
    {
        string path = string.Empty;
    #if UNITY_EDITOR
        path = Application.dataPath;
    #else
        path = Application.persistentDataPath;
    #endif
        
        return Path.Combine(path, fileName);
    }
}
