using System;

namespace BuilderPatternDemo
{
    // ============================================================
    // 1. ПРОДУКТ (Product) — складний об'єкт, який ми будуємо
    // ============================================================
    class Computer
    {
        public string Cpu { get; set; } = "не встановлено";
        public string Ram { get; set; } = "не встановлено";
        public string Storage { get; set; } = "не встановлено";
        public string Gpu { get; set; } = "не встановлено";
        public string Cooling { get; set; } = "не встановлено";

        public void Show(string title)
        {
            Console.WriteLine("=== " + title + " ===");
            Console.WriteLine("Процесор:       " + Cpu);
            Console.WriteLine("Оперативна пам'ять: " + Ram);
            Console.WriteLine("Накопичувач:    " + Storage);
            Console.WriteLine("Відеокарта:     " + Gpu);
            Console.WriteLine("Охолодження:    " + Cooling);
            Console.WriteLine();
        }
    }

    // ============================================================
    // 2. ІНТЕРФЕЙС БУДІВЕЛЬНИКА (Builder)
    //    Описує кроки створення продукту
    // ============================================================
    interface IComputerBuilder
    {
        void BuildCpu();
        void BuildRam();
        void BuildStorage();
        void BuildGpu();
        void BuildCooling();
        Computer GetResult();
    }

    // ============================================================
    // 3. КОНКРЕТНІ БУДІВЕЛЬНИКИ (Concrete Builders)
    //    Кожен реалізує кроки по-своєму
    // ============================================================
    class GamingComputerBuilder : IComputerBuilder
    {
        private Computer _computer = new Computer();

        public void BuildCpu()     { _computer.Cpu = "Intel Core i9"; }
        public void BuildRam()     { _computer.Ram = "32 ГБ DDR5"; }
        public void BuildStorage() { _computer.Storage = "SSD NVMe 2 ТБ"; }
        public void BuildGpu()     { _computer.Gpu = "NVIDIA RTX 4080"; }
        public void BuildCooling() { _computer.Cooling = "Рідинне охолодження"; }

        public Computer GetResult()
        {
            Computer result = _computer;
            _computer = new Computer(); // готуємось до нової збірки
            return result;
        }
    }

    class OfficeComputerBuilder : IComputerBuilder
    {
        private Computer _computer = new Computer();

        public void BuildCpu()     { _computer.Cpu = "Intel Core i3"; }
        public void BuildRam()     { _computer.Ram = "8 ГБ DDR4"; }
        public void BuildStorage() { _computer.Storage = "SSD 256 ГБ"; }
        public void BuildGpu()     { _computer.Gpu = "Вбудована графіка"; }
        public void BuildCooling() { _computer.Cooling = "Стандартний кулер"; }

        public Computer GetResult()
        {
            Computer result = _computer;
            _computer = new Computer();
            return result;
        }
    }

    // ============================================================
    // 4. ДИРЕКТОР (Director)
    //    Знає ПОРЯДОК кроків, але не знає, які саме деталі ставити
    // ============================================================
    class Director
    {
        private IComputerBuilder _builder;

        public Director(IComputerBuilder builder)
        {
            _builder = builder;
        }

        public void SetBuilder(IComputerBuilder builder)
        {
            _builder = builder;
        }

        // Повна збірка — усі кроки
        public void BuildFullComputer()
        {
            _builder.BuildCpu();
            _builder.BuildRam();
            _builder.BuildStorage();
            _builder.BuildGpu();
            _builder.BuildCooling();
        }

        // Спрощена збірка — лише базові компоненти
        public void BuildBasicComputer()
        {
            _builder.BuildCpu();
            _builder.BuildRam();
            _builder.BuildStorage();
        }
    }

    // ============================================================
    // 5. КЛІЄНТ (Client) — точка входу
    // ============================================================
    class Program
    {
        static void Main(string[] args)
        {
            // Обираємо конкретного будівельника
            GamingComputerBuilder gamingBuilder = new GamingComputerBuilder();
            Director director = new Director(gamingBuilder);

            // Ігровий ПК — повна збірка
            director.BuildFullComputer();
            Computer gamingPc = gamingBuilder.GetResult();
            gamingPc.Show("Ігровий комп'ютер (повна збірка)");

            // Той самий директор, інший будівельник
            OfficeComputerBuilder officeBuilder = new OfficeComputerBuilder();
            director.SetBuilder(officeBuilder);

            // Офісний ПК — повна збірка
            director.BuildFullComputer();
            Computer officePc = officeBuilder.GetResult();
            officePc.Show("Офісний комп'ютер (повна збірка)");

            // Офісний ПК — спрощена збірка (інший порядок/набір кроків)
            director.BuildBasicComputer();
            Computer basicPc = officeBuilder.GetResult();
            basicPc.Show("Офісний комп'ютер (базова збірка)");

            // Клієнт може обійтись і без директора, керуючи кроками вручну
            GamingComputerBuilder customBuilder = new GamingComputerBuilder();
            customBuilder.BuildCpu();
            customBuilder.BuildGpu();
            Computer customPc = customBuilder.GetResult();
            customPc.Show("Нестандартна збірка вручну (без директора)");

            Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}
