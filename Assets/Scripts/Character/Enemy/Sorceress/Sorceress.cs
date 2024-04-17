using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sorceress : MonoBehaviour
{
    [Header("Meteor")]
    [SerializeField]
    private Meteor meteor;
    [SerializeField]
    private GameObject meteorObject;
    [SerializeField]
    private Transform MeteorTransform;

    public void SpawnMeteor()
    {
        meteor = Instantiate(meteorObject, MeteorTransform.position, Quaternion.identity).GetComponent<Meteor>();
    }

    public void FallMeteor()
    {
        meteor.Falling(Vector3.down);
    }
}