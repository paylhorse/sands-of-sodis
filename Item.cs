[System.Serializable]
public class Item
{
    public string itemName;
    public string itemType;
    public string itemDescription;
    public int quantity;

    public Item(string name, string type, string desc, int quantity = 1)
    {
        this.itemName = name;
        this.itemType = type;
        this.itemDescription = desc;
        this.quantity = quantity;
    }
}
