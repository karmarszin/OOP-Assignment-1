namespace VendingApp
{
    public class VendingMachine
    {
        private Dictionary<string, Product> products; //группируем объекты типа Product{'Cникерс':<'Сникерс', 50руб, 20шт>, 'Kола':<'Кола', 80руб, 12шт}
        private Dictionary<int, int> balance;
        private int insertedAmount;
        public VendingMachine()
        {

            products = new Dictionary<string, Product>();
            balance = new Dictionary<int, int>()
            {
                {1, 1}, {2, 1}, {5, 5}, {10, 5}, {50, 5}, {100, 5}, {200, 0}
            };

            insertedAmount = 0;
        }


        public void ShowProducts()
        {
            if (products.Count == 0)
            {
                Console.WriteLine("Нечего выводить");
                return;
            }
            
            foreach (var item in products)
            {
                Console.WriteLine($"Товар: {item.Value.Name} --- Цена: {item.Value.Price} руб. --- Количество: {item.Value.Quantity} шт.");
            }
            
        }

        public void InsertCoin(int amount)
        {
            if (!balance.ContainsKey(amount))
            {
                Console.WriteLine("Неправильный номинал. Принимаем только 1, 2, 5, 10, 100, 200");
                return;
            }

            insertedAmount += amount;
            balance[amount]++;
            Console.WriteLine($"Внесена монета {amount} руб. Всего внесено: {insertedAmount-amount} -> {insertedAmount} руб.");
        }

        public void GetProduct(string product_name)
        {
            if (!products.ContainsKey(product_name))
            {
                Console.WriteLine("Ошибка ввода. Такого товара нет");
                return;
            }

            if (products[product_name].Quantity == 0)
            {
                Console.WriteLine($"Товар {product_name} закончился. Внесено {insertedAmount} руб.");
                return;
            }

            int price = products[product_name].Price;

            if (price > insertedAmount)
            {
                Console.WriteLine($"Недостаточно средств. Внесено {insertedAmount} руб., а {product_name} стоит {price} руб");
                return;
            }

            insertedAmount -= price;
            products[product_name].Quantity--;
            Console.WriteLine($"Куплен товар {product_name} за {price} руб. Осталось {products[product_name].Quantity + 1} -> {products[product_name].Quantity} шт.");
            Console.WriteLine($"Осталось после покупки {insertedAmount + price} -> {insertedAmount} руб.");
        }


        private int CalculateSumOfCoins(Dictionary<int, int> coins)
        {
            int sum = 0;
            foreach (int nominal in coins.Keys)
            {
                sum += nominal * coins[nominal];
            }
            return sum;
        }

        private Dictionary<int, int> CollectValueInCoins(int total_change)
        {   
            int[] nominals = balance.Keys.ToArray();
            nominals = nominals.Reverse().ToArray();
            Dictionary<int, int> change_coin_by_coin = new Dictionary<int, int> {
                {1, 0}, {2, 0}, {5, 0}, {10, 0}, {50, 0}, {100, 0}, {200, 0}
            };

            foreach (int nominal in nominals)
            {

                int counter = 0;
                while (nominal <= total_change && counter < balance[nominal]) //еще проверяется количество монет данного номинала
                {
                    total_change -= nominal;
                    counter++;
                }
                change_coin_by_coin[nominal] = counter;
                balance[nominal] -= counter;
            }
            return change_coin_by_coin;
        }
        
        public void GetChange()
        {
            int total_change = insertedAmount; //то, что осталось от внесенных денег
            int total_change_saved = total_change;
            int totalBalance = CalculateSumOfCoins(balance);
            if (total_change > totalBalance)
            {
                Console.WriteLine($"Невозможно выдать сдачу. Сумма сдачи: {total_change} руб.");
                return;
            }

            Dictionary<int, int> change_coin_by_coin = CollectValueInCoins(total_change);
            
            if (total_change > CalculateSumOfCoins(change_coin_by_coin))
            {
                Console.WriteLine($"Полностью сдачу выдать не получится. Размер сдачи: {total_change_saved}");
                Console.WriteLine($"Автомат может выдать {CalculateSumOfCoins(change_coin_by_coin)} руб.");
                Console.WriteLine(ToBozheskiyVid(change_coin_by_coin));
                return;
            }
            Console.WriteLine($"Общая сумма сдачи составляет {total_change_saved} руб. Выдано монетами:");
            Console.WriteLine(ToBozheskiyVid(change_coin_by_coin));
        }
        
        private string ToBozheskiyVid(Dictionary<int, int> coins) // приводим в божеский вид вывод сдачи по монетам
        {
            List<string> parts = new List<string>();
            foreach (var c in coins)
                if (c.Value != 0)
                parts.Add($"{c.Key}руб x {c.Value}");
            return string.Join(", ", parts);
        }

        public void AdminAddProducts(string name, int price, int quantity)
        {
            var product = new Product(name, price, quantity);
            products.Add(name, product);
        }

        public void AdminCollectMoney(int amount)
        {
            int totalBalance = CalculateSumOfCoins(balance);
            Console.WriteLine($"В автомате: {totalBalance} руб.");
            Console.WriteLine(ToBozheskiyVid(balance));
            if (amount > totalBalance)
            {
                Console.WriteLine($"Столько вывести не выйдет. В автомате: {totalBalance} руб., запрошено: {amount} руб.");
                return;
            }

            Dictionary<int, int> coins_to_withdraw = CollectValueInCoins(amount);

            if (ToBozheskiyVid(coins_to_withdraw).Length == 0)
            {
                Console.WriteLine($"Нет монет достаточного номинала, чтобы вывести эту суму. Запрошено: {amount} руб.");
                return;
            }
            Console.WriteLine($"Сумма {amount} руб. выведена. Админ получил монеты:");
            Console.WriteLine(ToBozheskiyVid(coins_to_withdraw));
            Console.WriteLine($"Остаток в автомате: {CalculateSumOfCoins(balance)}");

        }
    }
}

