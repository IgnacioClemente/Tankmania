using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pool {

    [SerializeField] string poolName;
    [SerializeField] GameObject obj;
    [SerializeField] int cant;
    [SerializeField] bool forceExpand;
    GameObject[] arrayGameObject;

    public string GetName()
    {
        return poolName;
    }

  public void InstanceArray(Vector3 pos,Transform parent)
    {
        //creo un array x lo recorre le asigno en esa posicion un parent luego una instancia de un gameobject y luego lo seteo en falso
        arrayGameObject = new GameObject[cant];
        for (int i = 0; i < arrayGameObject.Length; i++)
        {
            //arrayGameObject[arrayGameObject.Length - 1].transform.parent = parent;
            arrayGameObject[i] = GameObject.Instantiate(obj, parent);
            arrayGameObject[i].SetActive(false);
        }
    }
    // utilizito esta funcion solamente cuando quiero expadir el array
    private GameObject InstanceLastObject(GameObject[]arrayGameObject, Vector3 pos, Transform parent)
    {
        arrayGameObject[arrayGameObject.Length-1].transform.position = pos;
        arrayGameObject[arrayGameObject.Length-1].SetActive(false);
        arrayGameObject[arrayGameObject.Length - 1].transform.parent = parent;
        return arrayGameObject[arrayGameObject.Length - 1];

    }

    public GameObject GetObject( Vector3 pos, Transform parent)
    {
        //busco el primer objeto que no esta activo y lo devuelvo
        for(int i = 0;i< arrayGameObject.Length;i++)
        {
            if(arrayGameObject[i].activeSelf == false)
            {
                return arrayGameObject[i];
            }
        }
        //se fija si tengo que forsar el array en caso de que no encuentre ninguno desactivado
        if(forceExpand)
        {
            ExpandGameArray(arrayGameObject, pos, parent);
        }
        return null;
    }

    public void ReturnObject(GameObject objectToReturn, Transform pos)
    {
        for (int i = 0; i < arrayGameObject.Length; i++)
        {
            if(arrayGameObject[i] == objectToReturn)
            {
                arrayGameObject[i].transform.position = pos.position;
                arrayGameObject[i].SetActive(false);
            }
        }
    }

    private GameObject ExpandGameArray(GameObject[] arrayGameObject, Vector3 pos, Transform parent)
    {
        //creo un array x y recorro el que me pasaron por parametro y le voy a pasando al aux posicion a posicion
        cant++;
        GameObject[]aux= new GameObject[cant];
        for(int i = 0;i< arrayGameObject.Length;i++)
        {
            aux[i] = arrayGameObject[i];
        }
        arrayGameObject = new GameObject[0];
        arrayGameObject = aux;
        return arrayGameObject[arrayGameObject.Length - 1] = InstanceLastObject(arrayGameObject, pos, parent);
    }
}
