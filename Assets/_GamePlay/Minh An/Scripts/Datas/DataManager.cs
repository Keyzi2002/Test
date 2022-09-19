using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<DataManager>();
            }
            if(instance == null)
            {
                GameObject obj = new GameObject();
                instance = obj.AddComponent<DataManager>();
                obj.name = nameof(DataManager);
            }
            DontDestroyOnLoad(instance);
            return instance;
        }
    }

    public Transform myTransform;
    [SerializeField] DataShovel dataShovel;
    [SerializeField] DataStep dataStep;
    [SerializeField] DataBag dataBag;
    private void Awake()
    {
        if (myTransform == null) { myTransform = this.transform; }
    }
    public DataShovel GetDataShovel()
    {
        if(dataShovel == null)
        {
            GameObject objDataShovel = new GameObject();
            dataShovel = objDataShovel.AddComponent<DataShovel>();
            dataShovel.myTransform.SetParent(myTransform);
            dataShovel.name = nameof(DataShovel);
        }
        return dataShovel;
    }
    public DataStep GetDataStep()
    {
        if (dataStep == null)
        {
            GameObject objDataStep = new GameObject();
            dataStep = objDataStep.AddComponent<DataStep>();
            dataStep.myTransform.SetParent(myTransform);
            dataStep.name = nameof(DataStep);
        }
        return dataStep;
    }
    public DataBag GetDataBag()
    {
        if (dataBag == null)
        {
            GameObject objDataStep = new GameObject();
            dataBag = objDataStep.AddComponent<DataBag>();
            dataBag.myTransform.SetParent(myTransform);
            dataBag.name = nameof(DataBag);
        }
        return dataBag;
    }
    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
    }
}
