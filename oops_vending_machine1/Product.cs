namespace VendingApp
{
    public class Product
    {
        public string Name;
        public int Price;
        public int Quantity;

        public Product(string name, int price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }
}