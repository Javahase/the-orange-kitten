using System.Linq;
using UnityEngine;

public class SingletonObject<T> : MonoBehaviour where T : SingletonObject<T>
{

    private static T _instance;

    public static T GetInstance()
    {
        if (_instance == null)
        {
            var objs = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
            if (objs.Length != 1)
            {
                Debug.LogError("Singleton may only exist once!");
                return null;
            }
            _instance = objs.First();
        }
        return _instance;
    }

}
