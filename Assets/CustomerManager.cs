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
        // Start with filling the cafe initially
        for (int i = 0; i < queuePositions.Count; i++)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        if (customers.Count < queuePositions.Count)
        {
            // Instantiate customer at the door position
            GameObject customerObj = Instantiate(customerPrefab, doorPosition.position, Quaternion.identity);
            Customer customer = customerObj.GetComponent<Customer>();
            customer.targetPosition = queuePositions[customers.Count]; // Set the initial target position
            customers.Add(customer);
            Debug.Log("New customer spawned at the door and moving to queue.");
        }
    }

    public void CustomerServed(Customer customer)
    {
        int index = customers.IndexOf(customer);
        if (index != -1)
        {
            customers.RemoveAt(index);
            Destroy(customer.gameObject);
            // Update queue positions for remaining customers
            for (int i = index; i < customers.Count; i++)
            {
                customers[i].targetPosition = queuePositions[i];
            }
        }

        // After serving one customer, spawn another if the queue is not full
        Spawn();
    }
}
