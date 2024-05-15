using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Com.Elrecoal.Stray_Bullet
{
    public class Data : MonoBehaviour
    {
        public static void SaveProfile(ProfileData t_user)
        {
            try
            {
                string path = Application.persistentDataPath + "/profile.dt";

                if (File.Exists(path)) File.Delete(path);

                FileStream file = File.Create(path);

                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(file, t_user);
                file.Close();
                Debug.Log("Datos guardados");
            }
            finally
            {
                Debug.Log("Algo ha ido mal");
            }
        }

        public static ProfileData LoadProfile()
        {

            ProfileData ret = new ProfileData();

            try
            {
                string path = Application.persistentDataPath + "/profile.dt";

                if (File.Exists(path))
                {
                    FileStream file = File.Open(path, FileMode.Open);
                    BinaryFormatter bf = new BinaryFormatter();
                    ret = (ProfileData)bf.Deserialize(file);
                }
            }
            finally
            {
                Debug.Log("Archivo no encontrado");
            }

            
            return ret;
        }

    }
}