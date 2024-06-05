using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public List<GameObject> customerPrefabs; // List of customer prefabs (male and female)
    public Transform doorPosition; // Assign this in the Unity Editor to the position of the door
    public List<Transform> chairPositions = new List<Transform>(); // Adjusted naming for clarity
    public float spawnRate = 5f;
    private List<Customer> customers = new List<Customer>();

    void Start()
    {
        StartCoroutine(SpawnCustomers());
    }

    IEnumerator SpawnCustomers()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);
            if (customers.Count < chairPositions.Count)
            {
                Spawn();
            }
        }
    }

    void Spawn()
    {
        if (customers.Count < chairPositions.Count)
        {
            GameObject customerPrefab = GetRandomCustomerPrefab();
            GameObject customerObj = Instantiate(customerPrefab, doorPosition.position, Quaternion.identity);
            Customer customer = customerObj.GetComponent<Customer>();
            if (customer != null)
            {
                customer.enabled = true;  // Make sure the script is enabled
            }
            int chairIndex = FindAvailableChair();
            if (chairIndex != -1)
            {
                customer.targetPosition = chairPositions[chairIndex]; // Set the target position as a chair
                customers.Add(customer);
                Debug.Log("New customer spawned at the door and moving to a chair.");
            }
        }
    }

    GameObject GetRandomCustomerPrefab()
    {
        if (customerPrefabs.Count == 0) return null;
        int randomIndex = Random.Range(0, customerPrefabs.Count);
        return customerPrefabs[randomIndex];
    }

    int FindAvailableChair()
    {
        for (int i = 0; i < chairPositions.Count; i++)
        {
            if (!customers.Exists(c => c.targetPosition == chairPositions[i]))
                return i;
        }
        return -1; // No available chair
    }

    public void CustomerServed(Customer customer)
    {
        int index = customers.IndexOf(customer);
        if (index != -1)
        {
            customers.RemoveAt(index);
            Debug.Log("Customer leaving the cafe and chair becoming available.");
        }
        Destroy(customer.gameObject);
    }
}
