using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class AchievementManager : MonoBehaviour
{

    

    public int[] AchievemntIds = new int[10000];
    public bool[] AchievementStore = new bool[10000];


    public static AchievementManager current;
    static Loader loader = new Loader();

    void Awake()
    {
        current = this;
    }

    void Start()
    {
        loader.Load("_achievements.save", "_ids.save");
    }

    public void RegisterAchievement(bool isUnlocked, int id)
    {
        
        //     ~ Set Achievement
        int y = 0;
        foreach(int item in current.AchievemntIds)
        {      
            if(item == 0)
            {
                current.AchievemntIds[y] = id;
                current.AchievementStore[y] = true;
                Debug.Log("ID's Registered");
                break;
            }
            y++; 
        }
    }


    //    ~ After Registering New Achievement
    public void SaveLoadedAchievements(string storepath, string idpath)
    {
        FileStream fs = new FileStream(storepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        FileStream fs2 = new FileStream(idpath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
        BinaryFormatter bf = new BinaryFormatter();

        bf.Serialize(fs, AchievementStore);
        bf.Serialize(fs2, AchievemntIds);
        
        fs.Close();
        fs2.Close();
    }



    //     ~ Startup
    public bool[] LoadAllAchievements(string path)
    {
        if (File.Exists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            BinaryFormatter bf = new BinaryFormatter();
            fs.Position = 0;
            return (bool[])bf.Deserialize(fs);
        } 
        return null;
    }
    public int[] LoadAchievementIDs(string path)
    {
        if (File.Exists(path))
        {
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            BinaryFormatter bf = new BinaryFormatter();
            fs.Position = 0;
            return (int[])bf.Deserialize(fs);
        }
        return null;
    }


    //     ~ Loader

    internal class Loader
    {
        public void Load(string storepath, string idspath)
        {
            if (File.Exists("_ids.save"))
            {
                current.AchievementStore = current.LoadAllAchievements(storepath);
                current.AchievemntIds = current.LoadAchievementIDs(idspath);
            }

        }
    }
 
}
