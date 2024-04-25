using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform doorPosition; // Assign this in the Unity Editor to the position of the door
    public List<Transform> queuePositions = new List<Transform>();
    public float spawnRate = 5f;
    private List<Customer> customers = new List<Customer>();

    void Start()
    {
        StartCoroutine(SpawnCustomer());
    }

    IEnumerator SpawnCustomer()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            if (customers.Count < queuePositions.Count)
            {
                Spawn();
            }
        }
    }

    void Spawn()
    {
        if (queuePositions.Count > customers.Count)
        {
            // Instantiate customer at the door position
            GameObject customerObj = Instantiate(customerPrefab, doorPosition.position, Quaternion.identity);
            Customer customer = customerObj.GetComponent<Customer>();
            if (queuePositions.Count > 0)
            {
                customer.targetPosition = queuePositions[customers.Count]; // Set the queue target position
            }
            customers.Add(customer);
            Debug.Log("New customer spawned at the door and moving to queue.");
        }
    }

    public void CustomerServed(Customer customer)
    {
        customers.Remove(customer);
        Destroy(customer.gameObject);
        // Update queue positions for remaining customers
        for (int i = 0; i < customers.Count; i++)
        {
            if (i < queuePositions.Count)
            {
                customers[i].targetPosition = queuePositions[i];
            }
        }
    }


}
