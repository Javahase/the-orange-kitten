using System.Linq;
using UnityEngine;

/// <summary>
/// This class describes objects that are used at a global scale within the scene.
///
/// Objects that derive from this class can be called from any context, but they must exist in the scene exactly once.
/// </summary>
/// <typeparam name="T">The type of the singleton that should be exposed within the scene.</typeparam>
public class SingletonObject<T> : MonoBehaviour where T : SingletonObject<T>
{

    /// <summary>
    /// The cached object instance. Null, if <see cref="GetInstance"/> was never called.
    /// </summary>
    private static T _instance;

    /// <summary>
    /// Receives the instance of that singleton. If this method has already been called,
    /// it returns a cached instance. Otherwise, it searched the scene for exactly one object
    /// of this type.
    /// </summary>
    /// <returns>The singleton instance.</returns>
    public static T GetInstance()
    {
        if (_instance)  return _instance;
        var objs = FindObjectsByType<T>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
            if (objs.Length != 1)
            {
                Debug.LogError("Singleton may only exist once!");
                return null;
            }
        _instance = objs.First();
        return _instance;
    }

}
