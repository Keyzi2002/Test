using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class SimplePool
{
    static int DEFAULT_AMOUNT = 10;
    //pool tong
    static Dictionary<GameUnit, Pool> poolObjects = new Dictionary<GameUnit, Pool>();

    //tim pool cha cua thang object
    static Dictionary<GameUnit, Pool> poolParents = new Dictionary<GameUnit, Pool>();

    public static void Preload(GameUnit prefab, int amount, Transform parent)
    {
        if (!poolObjects.ContainsKey(prefab))
        {
            poolObjects.Add(prefab, new Pool(prefab, amount, parent));
        }
    }

    public static GameUnit Spawn(GameUnit prefab, Vector3 position, Quaternion rotation, Transform ParentSpawn = null)
    {
        GameUnit obj = null;

        if (!poolObjects.ContainsKey(prefab) || poolObjects[prefab] == null)
        {
            poolObjects.Add(prefab, new Pool(prefab, DEFAULT_AMOUNT, ParentSpawn));
        }
        obj = poolObjects[prefab].Spawn(position, rotation);

        return obj;
    }

    public static T Spawn<T>(GameUnit prefab, Vector3 position, Quaternion rotation) where T : GameUnit
    {
        GameUnit obj = null;

        if (!poolObjects.ContainsKey(prefab) || poolObjects[prefab] == null)
        {
            poolObjects.Add(prefab, new Pool(prefab, DEFAULT_AMOUNT, null));
        }

        obj = poolObjects[prefab].Spawn(position, rotation);

        return obj as T;
    }

    public static void Despawn(GameUnit obj)
    {
        if (poolParents.ContainsKey(obj))
        {
            poolParents[obj].Despawn(obj);
        }
        else
        {
            GameObject.Destroy(obj);
        }
    }
    public static void ReleaseAll(GameUnit obj)
    {
        poolObjects[obj].Release();
    }
    public static void CollectAll()
    {
        foreach (var item in poolObjects)
        {
            item.Value.Collect();
        }
    }

    public static void ReleaseAll()
    {
        foreach (var item in poolObjects)
        {
            item.Value.Release();
        }
    }

    public class Pool
    {
        Queue<GameUnit> pools = new Queue<GameUnit>();
        List<GameUnit> activeObjs = new List<GameUnit>();
        Transform parent;
        GameUnit prefab;

        /* hàm constructer khởi tạo ban đầu*/
        public Pool(GameUnit prefab, int amount, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;

            /* Tạo ra obj cần pools gán vào Queue pools quản lý*/
            for (int i = 0; i < amount; i++)
            {
                GameUnit obj = GameObject.Instantiate(prefab, parent);
                poolParents.Add(obj, this);
                pools.Enqueue(obj);
                obj.gameObject.SetActive(false);
            }
        }

        /*Hàm spawn để lấy ra các obj đã tạo trước đó trong pools*/
        public GameUnit Spawn(Vector3 position, Quaternion rotation)
        {
            GameUnit obj = null;

            if (pools.Count == 0) // nếu trong pools không có obj nào thì thực hiện tạo thêm 1 obj và thêm lại nó vào trong pools
            {
                obj = GameObject.Instantiate(prefab, parent);
                poolParents.Add(obj, this);
            }
            else
            {
                obj = pools.Dequeue(); // đã có thì lấy nó ra
            }

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.gameObject.SetActive(true);

            activeObjs.Add(obj); // thêm vào list quản lí những obj đã được gọi (hay active)

            return obj;
        }

        /*Hàm để hủy active của một obj*/
        public void Despawn(GameUnit obj)
        {
            if (obj.gameObject.activeInHierarchy)
            {
                activeObjs.Remove(obj);
                pools.Enqueue(obj);
                obj.gameObject.SetActive(false);
            }
        }
        /*Hàm để Collect tất cả các obj đã active*/
        public void Collect()
        {
            while (activeObjs.Count > 0)
            {
                Despawn(activeObjs[0]);
            }
        }

        /*Hàm để xóa all các obj đã được tạo nằm trong pools*/
        public void Release()
        {
            Collect(); // thực hiện collect tất cả các obj trước khi xóa

            while (pools.Count > 0)
            {
                GameUnit obj = pools.Dequeue();
                GameObject.DestroyImmediate(obj);
                //GameObject.Destroy(obj);
            }
        }

    }
}

public class GameUnit: MonoBehaviour
{
    private Transform tf;

    public Transform Transform //Định nghĩa lại Transform
    {
        get
        {
            if (this.tf == null)
            {
                this.tf = transform;
            }

            return tf;
        }
    }
}