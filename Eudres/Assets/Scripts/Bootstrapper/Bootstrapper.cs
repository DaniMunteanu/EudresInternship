using UnityEngine;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Execute()
    {
        var prefab = Resources.Load<GameObject>("Prefabs/Bootstrapper");

        Object.DontDestroyOnLoad(Object.Instantiate(prefab));
    }
}
