using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBarrelComponent : MonoBehaviour
{
    public Transform originalBarrel; 
    public Transform barrelPieces;
    public float explosionForce = 2.0f; 
    public float delayBeforeDestruction = 2.5f; 

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Explode (GameObject _player)
    {
        //_player.GetComponentInChildren<Animator>().SetTrigger("Damaged");
        _player.GetComponent<PlayerComponent>().RemoveLifeFromPlayer();

        originalBarrel.gameObject.SetActive(false);
        barrelPieces.gameObject.SetActive(true);

        for (int i = 0; i < barrelPieces.childCount; i++)
        {
            barrelPieces.GetChild(i).GetComponent<Rigidbody>().AddForce(barrelPieces.GetChild(i).transform.position - this.transform.position * explosionForce, ForceMode.Impulse);
        }

        StartCoroutine(DestroyBarrel());
    }

    private IEnumerator DestroyBarrel ()
    {
        yield return new WaitForSeconds(delayBeforeDestruction);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))    
        {
            Explode(other.gameObject);
        }
    }
}
