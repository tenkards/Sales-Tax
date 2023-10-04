namespace StoreReceipts
{
    public class Program
    {
public static void Main(string[] args)
{
    Receipt receipt = new Receipt(); //to store the items and generate the receipt

    // Read input
    string? input; //if one operation in a chain of conditional member or element access operations returns null, the rest of the chain doesn't execute.
    while (!string.IsNullOrEmpty(input = Console.ReadLine()!))
    {
        string[] parts = input.Split(" at "); //using "at" as the differentiator

        // Ensure the input has the correct format
        if (parts.Length != 2)
        {
            Console.WriteLine("Invalid input format. Expected format is: 'Item at Price'");
            continue;
        } // with 0 being the left (item name) and 1 being the right (price)

        string[] nameParts = parts[0].Split(' ');
        bool isImported = nameParts.Any(p => p.Equals("imported", StringComparison.OrdinalIgnoreCase));
        string name = string.Join(" ", nameParts.Where(p => !p.Equals("imported", StringComparison.OrdinalIgnoreCase))); //checks for the word "Imported", while ignoring its case

        // Validate and parse the price
        if (!decimal.TryParse(parts[1], out decimal price)) //suppress all nullable warnings for the preceding expression
        {
            Console.WriteLine($"Invalid price format: {parts[1]}");
            continue;
        }

        receipt.AddItem(name, price, isImported);
    }

    // Generate receipt
    receipt.GenerateReceipt();
}
    }
}