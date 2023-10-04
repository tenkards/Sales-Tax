namespace StoreReceipts
{
    public class Receipt
    {
        private List<Item> items;

        public Receipt()
        // called in program.cs. Constructor for the list whenever Receipt obj is created
        {
            items = new List<Item>();
        }

        public void AddItem(string name, decimal price, bool isImported)
        {
            // split the name by space and discard the first element (quantity)
            var nameParts = name.Split(' ');
            if (nameParts.Length > 1)
            {
                name = string.Join(" ", nameParts.Skip(1));
                // to get rid of the quantity e.g "-1- bottle of perfume..."
            }

            items.Add(new Item { Name = name, Price = price, IsImported = isImported });
        }


        public void GenerateReceipt()
        {
            decimal totalTax = 0;
            decimal totalAmount = 0;
            //to keep track of the total tax and total amount for the entire receipt.

            var groupedItems = items
                .Select(item => new { Item = item, Tax = CalculateTax(item) })
                // selects each item from the items list with its corresponding tax amount calculated by calling CalculateTax
                .GroupBy(item => new { item.Item.Name, item.Item.IsImported })
                // groups the items based on their Name and IsImported properties.
                .Select(group =>
                // quantity of items is obtained by counting the number of items in the group using group.Count().
                {
                    int quantity = group.Count();
                    decimal totalPrice = group.Sum(item => item.Item.Price + item.Tax);
                    decimal unitPrice = totalPrice / quantity;
                    decimal totalItemPrice = totalPrice ;

                    totalTax += group.Sum(item => item.Tax);
                    totalAmount += totalItemPrice;

                    string importedStr = group.Key.IsImported ? "Imported " : "";
                    string itemName = $"{importedStr}{group.Key.Name}";
                    string itemString = quantity > 1 ? $"{quantity} @ {unitPrice :F2}" : "";

                    return new { ItemName = itemName, ItemString = itemString, TotalPrice = totalItemPrice };
                });

            foreach (var item in groupedItems)
            {
                if (!string.IsNullOrEmpty(item.ItemString))
                {
                    Console.WriteLine($"{item.ItemName}: {item.TotalPrice:F2} ({item.ItemString})");
                    // If the ItemString is not empty it means multiple items with the same name exist, it includes the quantity and unit price in the output.
                }
                else
                {
                    Console.WriteLine($"{item.ItemName}: {item.TotalPrice:F2}");
                }
            }

            Console.WriteLine($"Sales Taxes: {totalTax:F2}");
            Console.WriteLine($"Total: {totalAmount:F2}");
        }
        private decimal CalculateTax(Item item)
        {
            decimal taxRate = 0.1m;
            decimal importTaxRate = 0.05m;

            if (IsTaxExempt(item.Name))
            {
                taxRate = 0;
            }

            if (item.IsImported)
            {
                taxRate += importTaxRate;
            }

            decimal tax = Math.Ceiling(item.Price * taxRate / 0.05m) * 0.05m;
            return tax;
        }

        private bool IsTaxExempt(string itemName)
        {
            string[] exemptItems = { "book", "food", "headache", "chocolate bar", "chocolates" };
            return exemptItems.Any(itemName.ToLower().Contains);
        }
    }
}
