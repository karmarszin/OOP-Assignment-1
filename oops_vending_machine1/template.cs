using System;
using VendingApp;

namespace VendingApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VendingMachine vm = new VendingMachine();
            bool isRunning = true;

            while (isRunning)
            {
                Console.Clear();
                Console.WriteLine("=== ВЕНДИНГОВЫЙ АВТОМАТ ===");
                Console.WriteLine("1. Показать список товаров");
                Console.WriteLine("2. Внести монету");
                Console.WriteLine("3. Купить товар");
                Console.WriteLine("4. Получить сдачу");
                Console.WriteLine("5. Режим администратора");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");
                
                string choice = Console.ReadLine();
                Console.Clear();

                switch (choice)
                {
                    case "1":
                        vm.ShowProducts();
                        break;

                    case "2":
                        Console.Write("Введите номинал монеты (1, 2, 5, 10, 50, 100, 200): ");
                        if (int.TryParse(Console.ReadLine(), out int coin))
                            vm.InsertCoin(coin);
                        else
                            Console.WriteLine("Неверный ввод!");
                        break;

                    case "3":
                        Console.Write("Введите название товара: ");
                        string name = Console.ReadLine();
                        vm.GetProduct(name);
                        break;

                    case "4":
                        vm.GetChange();
                        break;

                    case "5":
                        AdminMenu(vm);
                        break;

                    case "0":
                        isRunning = false;
                        break;

                    default:
                        Console.WriteLine("Неизвестная команда!");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
                Console.ReadKey();
            }
        }

        static void AdminMenu(VendingMachine vm)
        {
            Console.Clear();
            Console.Write("Введите пароль администратора: ");
            string pass = Console.ReadLine();

            if (pass != "admin123")
            {
                Console.WriteLine("Неверный пароль!");
                return;
            }

            bool adminMode = true;
            while (adminMode)
            {
                Console.Clear();
                Console.WriteLine("=== РЕЖИМ АДМИНИСТРАТОРА ===");
                Console.WriteLine("1. Добавить товар");
                Console.WriteLine("2. Просмотреть товары");
                Console.WriteLine("3. Собрать деньги");
                Console.WriteLine("0. Выход из режима");
                Console.Write("Выберите действие: ");
                string adminChoice = Console.ReadLine();
                Console.Clear();

                switch (adminChoice)
                {
                    case "1":
                        Console.Write("Название товара: ");
                        string name = Console.ReadLine();
                        Console.Write("Цена: ");
                        int price = int.Parse(Console.ReadLine());
                        Console.Write("Количество: ");
                        int quantity = int.Parse(Console.ReadLine());
                        vm.AdminAddProducts(name, price, quantity);
                        Console.WriteLine("Товар успешно добавлен!");
                        break;

                    case "2":
                        vm.ShowProducts();
                        break;

                    case "3":
                        Console.Write("Введите сумму для изъятия: ");
                        int amount = int.Parse(Console.ReadLine());
                        vm.AdminCollectMoney(amount);
                        break;

                    case "0":
                        adminMode = false;
                        break;

                    default:
                        Console.WriteLine("Неверный ввод!");
                        break;
                }

                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
            }
        }
    }
}



