namespace StoreReceipts
{
    public class Item
    {
        public string Name { get; set; } = ""; // Assigning a default empty string value

        public decimal Price { get; set; }
        public bool IsImported { get; set; } // this is to differentiate between the local and imported goods
    }
}
