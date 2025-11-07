using System;
using VendingApp;

namespace VendingApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            VendingMachine vm = new VendingMachine();
            vm.AdminAddProducts("Сникерс", 10, 1);
            vm.InsertCoin(50);
            vm.GetProduct("Сникерс");
            vm.GetChange();
            vm.AdminCollectMoney(10);

        }
    }
}


