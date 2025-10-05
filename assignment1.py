class VendingMachine:
    def __init__(self):
        self.products = {}
        self.coins_inserted = {1: 0, 2: 0, 5: 0, 10: 0, 50: 0, 100: 0, 200: 0}
        self.balance = {1: 0, 2: 0, 5: 0, 10: 0, 50: 0, 100: 0, 200: 0}        # деньги внутри автомата (собранные)

        self.total_inserted_sum = 0

    def show_products(self):
        return print(self.products)
    
    def insert_coin(self, amount):
        if amount not in self.coins_inserted.keys():
            return print("Автомат принимает только монеты 1, 2, 5, 10, 50, 100, 200")
        
        self.coins_inserted[amount] += 1
        self.total_inserted_sum = sum(value*count for value,count in self.coins_inserted.items())
        
        for coin, count in self.coins_inserted.items():
            self.balance[coin] = count

        return print(f'Внесли {amount} рублей. Всего внесено {self.total_inserted_sum} рублей')
    
    def get_product(self, name):
        if name not in self.products.keys():
            return print('Такого товара нет')
        
        price = self.products[name]['price']
        if self.total_inserted_sum < price:
            return print(f"Недостаточно средств. Цена товара: {price} рублей")

        self.total_inserted_sum -= price
        self.products[name]["quantity"] -= 1
        return print(f'Куплен продукт {name} за {price} рублей. Осталось {self.products[name]["quantity"]} штук')

    def get_change(self):
        result = {}
        change = self.total_inserted_sum
        for coin in dict(sorted(self.balance.items(), key=lambda x: x[0], reverse=True)):  # идём от крупных
            while change >= coin:
                change -= coin
                result[coin] = result.get(coin, 0) + 1
        for change_coin in result:
            self.balance[change_coin] -= result[change_coin] 
        return print(f'Выдана сдача в размере {result} рублей')


    def admin_add_product(self, new_product, price, quantity):
        self.products.update({new_product: {'price' : price, 'quantity' : quantity}})
        return print(f"Добавлен продукт {new_product} ценой {self.products[new_product]['price']} количеством {self.products[new_product]['quantity']} единиц")

    def admin_collect_money(self, money_needed):
        total_balance = sum(value*count for value,count in self.balance.items())

        if money_needed is None:
            print(f'Вывели все деньги в размере {total_balance} рублей')
            self.balance = {1: 0, 2: 0, 5: 0, 10: 0, 50: 0, 100: 0, 200: 0}
            return
        
        if money_needed <= total_balance:
            total_balance -= money_needed

            result = {}
            withdrawal_amount = total_balance
            for coin in dict(sorted(self.balance.items(), key=lambda x: x[0], reverse=True)):  # идём от крупных
                while withdrawal_amount >= coin:
                    withdrawal_amount -= coin
                    result[coin] = result.get(coin, 0) + 1

            return print(f'Выводим {money_needed} рублей. Выдали монеты номиналами {result}. Осталось {total_balance} рублей')
        return print(f'На балансе всего {self.balance} рублей. Нельзя вывести {money_needed} рублей')

vm = VendingMachine()
vm.show_products()
vm.admin_add_product('orbit', 30, 10)
vm.show_products()
vm.insert_coin(100)
vm.insert_coin(10)
vm.insert_coin(10)
vm.insert_coin(2)
vm.get_product('orbit')
vm.get_change()
vm.admin_collect_money(1)