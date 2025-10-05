using VendingApp;

VendingMachine vm = new VendingMachine();

            vm.AdminAddProduct("Вода", 50, 5);
            vm.AdminAddProduct("Сок", 80, 3);
            vm.ShowProducts();

            vm.InsertCoin(100);
            vm.InsertCoin(10);
            vm.InsertCoin(10);
            vm.InsertCoin(10);
            vm.InsertCoin(50);
            
            vm.GetProduct("Сок");
            vm.GetChange();

            vm.AdminCollectMoney();



namespace VendingApp
{
    public class Product //в питоне для этого можно использовать словарь, тут отдельный класс
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        public Product(string name, int price, int quantity)
        {
            Name = name;
            Price = price;
            Quantity = quantity;
        }
    }
    public class VendingMachine
    {
        private Dictionary<string, Product> products;
        private Dictionary<int, int> balance;
        private int totalInsertedSum;

        public VendingMachine()
        {
            products = new Dictionary<string, Product>();
            balance = new Dictionary<int, int>()
            {
                {1, 0}, {2, 0}, {5, 0}, {10, 0}, {50, 0}, {100, 0}, {200, 0}
            };
            totalInsertedSum = 0;
        }

        public void ShowProducts()
        {
            if (products.Count == 0)
            {
                Console.WriteLine("Автомат пуст, продуктов нет");
                return;
            }

            foreach (var item in products)
            {
                Console.WriteLine($"{item.Value.Name} --- {item.Value.Price} руб --- {item.Value.Quantity} шт.");
            }
        }

        public void InsertCoin(int amount)
        {
            if (!balance.ContainsKey(amount))
            {
                Console.WriteLine("Автомат принимает только монеты 1, 2, 5, 10, 50, 100, 200");
                return;
            }

            balance[amount]++;
            totalInsertedSum += amount;
            Console.WriteLine($"Внесли {amount} руб. Всего внесено {totalInsertedSum} руб");

        }

        public void GetProduct(string name)
        {
            if (!products.ContainsKey(name))
            {
                Console.WriteLine("Такого товара нет");
                return;
            }

            Product product = products[name]; // ссылочно указываю на объект product в памяти
            if (product.Quantity <= 0)
            {
                Console.WriteLine($"Товар {product.Name} закончился.");
                return;
            }

            if (totalInsertedSum < product.Price)
            {
                Console.WriteLine($"Недостаточно денег. Цена товара: {product.Price} руб");
                return;
            }

            //проводим продажу

            totalInsertedSum -= product.Price;
            product.Quantity--;
            Console.WriteLine($"Куплен продукт {product.Name} за {product.Price}руб. Осталось {product.Quantity} шт.");
        }

        
        private string ToBozheskiyVid(Dictionary<int, int> coins) // приводим в божеский вид вывод сдачи по монетам
        {
            List<string> parts = new List<string>();
            foreach (var c in coins)
                if (c.Value != 0)
                parts.Add($"{c.Key}руб x {c.Value}");
            return string.Join(", ", parts);
        }

        public void GetChange()
        {
            int change = totalInsertedSum; //после проведения продажи наш остаток все еще записан в totalinsertedsum
            var result = new Dictionary<int, int>()
            {
                {1, 0}, {2, 0}, {5, 0}, {10, 0}, {50, 0}, {100, 0}, {200, 0}
            }; //тут хранятся выданные монетки

            foreach (var coin in balance.Keys.OrderByDescending(c => c)) //для монеты каждого номинала
            {
                int available = balance[coin]; //доступно монет текущего номинала

                while (change >= coin && available > 0)
                {
                    change -= coin;
                    available--;
                    result[coin]++;
                }
                balance[coin] = available;
            }

            if (change > 0) //не вышло выдать всю сдачу имеющимися монетами
            {
                Console.WriteLine($"Автомат не смог выдать всю сдачу!\nВыдано: {ToBozheskiyVid(result)}, остаток без сдачи: {change}");
            }

            else
            {
                Console.WriteLine($"Выдана сдача монетами: {ToBozheskiyVid(result)}");
            }

            totalInsertedSum = 0;

        }




        public void AdminAddProduct(string name, int price, int quantity)
        {
            if (products.ContainsKey(name))
            {
                products[name].Price = price;
                products[name].Quantity += quantity;
            }
            else
            {
                products[name] = new Product(name, price, quantity);
            }
            Console.WriteLine($"Добавлен продукт {name} ценой {price}руб и количеством {quantity} шт.");
        }

        public void AdminCollectMoney(int? moneyNeeded = null) // при пустом значении выатскиваем все
        {
            int totalBalance = 0;

            foreach (var coin in balance)
            {
                totalBalance += coin.Key * coin.Value;
            }

            if (moneyNeeded == null) //обналичиваем все
            {
                Console.WriteLine($"Собрано {totalBalance}руб. Баланс имашины обнулён.");
                foreach (var key in new List<int>(balance.Keys))
                    balance[key] = 0;
                return;
            }

            if (moneyNeeded > totalBalance)
            {
                Console.WriteLine($"Недостаточно средств. В кассе {totalBalance} руб");
                return;
            }

            int remaining = (int)moneyNeeded; //однозначно определим тип
            Dictionary<int, int> result = new Dictionary<int, int>();

            foreach (var coin in new List<int>(balance.Keys))

            {
                int available = balance[coin];
                while (remaining >= coin && available > 0)
                {
                    remaining -= coin;
                    available--;
                    if (!result.ContainsKey(coin))
                        result[coin] = 0;
                    result[coin]++;
                }
                balance[coin] = available;
            }
            
            if (remaining > 0)
            {
                Console.WriteLine("Не удалось собрать точную сумму");
                Console.WriteLine($"Выдано монетами: {ToBozheskiyVid(result)}, остаток: {remaining}₽");
            }
            else
            {
                Console.WriteLine($"Выведено {moneyNeeded}₽ монетами: {result}");
            }

        }



    }



}




