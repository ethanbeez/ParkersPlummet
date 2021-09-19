using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    // Types of upgrades
    public enum Upgrades {
        Horsepower,
        Flexibility,
        Torque,
        Time
    }

    // Fields
    [SerializeField] int currency;
    [SerializeField] float launchForce;
    [SerializeField] Vector2 spawn;
    [SerializeField] Animator anim;

    // Event Handler Hiddens
    float runTimer;
    int runDistance;
    int timeExtension;
    int maxLevel;
    PlayerController player;
    public Dictionary<Upgrades, int> upgradeLevels = new Dictionary<Upgrades, int>();

    // Upgrades
    Stack<int> horsepowers = new Stack<int>(new int[] {380, 330, 280});
    Stack<int> flexibilities = new Stack<int>(new int[] {30, 25, 20});
    Stack<int> torques = new Stack<int>(new int[] {25, 20, 15});
    Stack<int> timeExtensions = new Stack<int>(new int[] {25, 20, 15});

    // Sets up all initial values
    private void Start()
    {
        // Dictionary Setup
        upgradeLevels.Add(Upgrades.Horsepower, 0);
        upgradeLevels.Add(Upgrades.Flexibility, 0);
        upgradeLevels.Add(Upgrades.Torque, 0);
        upgradeLevels.Add(Upgrades.Time, 0);

        // Other Hiddens
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        runTimer = 0;
        runDistance = 0;
        //currency = 0;
        timeExtension = 10;
        maxLevel = 3;
    }

    // Checks for status updates once per frame
    private void Update()
    {
        // Increment Timers
        runTimer += Time.deltaTime;
        runDistance = -(int)player.transform.position.y;
    }

    // Tries to add currency
    public bool AddCurrency(int amount) 
    {
        if (amount > 0)
        {
            currency += amount;
            return true;
        }
        return false;
    }

    // Tries to remove currency
    public bool RemoveCurrency(int amount)
    {
        int difference = currency - amount;
        if (difference > 0)
        {
            currency = difference;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Tries to buy an upgrade for the corresponding characteristic
    /// </summary>
    /// <param name="upgrade">The upgrade being bought</param>
    /// <returns>
    /// True if the purchase was successful
    /// false if it was not
    /// </returns>
    public bool Buy(Upgrades upgrade)
    { 
        upgradeLevels.TryGetValue(upgrade, out int upgradeLevel);
        if (upgradeLevel < maxLevel && RemoveCurrency(GetPrice(upgradeLevel)))
        {
            anim.SetTrigger("SuccesfulPurchase");
            Upgrade(upgrade);
            return true;
        }
        return false;
        //TODO: Shopkeep animation
    }

    // Gets the price for the corresponding level
    int GetPrice(int upgradeLevel)
    {
        return (upgradeLevel + 1) * 100;
    }

    // Upgrades the corresponding characteristic
    void Upgrade(Upgrades upgrade)
    {
        if (upgradeLevels.TryGetValue(upgrade, out int level)){
            upgradeLevels[upgrade] = level + 1;
            switch (upgrade)
            {
                case Upgrades.Horsepower:
                    if (horsepowers.Count > 0)
                        Debug.Log(horsepowers.Peek());
                        player.horsepower = horsepowers.Pop();
                    break;
                case Upgrades.Flexibility:
                    if (flexibilities.Count > 0)
                        player.flexibility = flexibilities.Pop();
                    break;
                case Upgrades.Torque:
                    if (torques.Count > 0)
                        player.torque = torques.Pop();
                    break;
                case Upgrades.Time:
                    if (timeExtensions.Count > 0)
                        timeExtension = timeExtensions.Pop();
                    break;
            }
        }
        
    }

    void ResetRun() 
    {
        // reset player transform
        player.transform.position = spawn;
        // Enable UI
    }

    void StartRun()
    {
        // Disable UI
        player.Launch(Vector2.right * launchForce);
        // Might need to wait to avoid shop collisions
        player.EnableMovement();
    }

    void EngageFailState()
    {
        player.DisableMovement();
        AddCurrency(runDistance);
        // TODO:: Play wizard hand animation
        // TODO:: Play reset screen animation
        ResetRun();
    }
}
