using System;

namespace CompositePatternDemo
{
    // ============================================================
    // 1. КОМПОНЕНТ (Component)
    //    Спільний базовий клас і для файлів, і для папок.
    //    Клієнт працює з ним однаково, не знаючи, що саме перед ним.
    // ============================================================
    abstract class FileSystemItem
    {
        protected string _name;

        public FileSystemItem(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }

        // Операції, які мають і файл, і папка
        public abstract int GetSize();
        public abstract void Show(int level);

        // Операції керування дочірніми елементами.
        // За замовчуванням (для "листків") вони недоступні.
        public virtual void Add(FileSystemItem item)
        {
            Console.WriteLine("Помилка: до файлу «" + _name + "» не можна додати елемент.");
        }

        public virtual void Remove(FileSystemItem item)
        {
            Console.WriteLine("Помилка: з файлу «" + _name + "» не можна видалити елемент.");
        }
    }

    // ============================================================
    // 2. ЛИСТОК (Leaf) — простий об'єкт без вкладених елементів
    // ============================================================
    class FileItem : FileSystemItem
    {
        private int _size; // розмір у КБ

        public FileItem(string name, int size) : base(name)
        {
            _size = size;
        }

        public override int GetSize()
        {
            return _size;
        }

        public override void Show(int level)
        {
            string indent = new string(' ', level * 4);
            Console.WriteLine(indent + "- " + _name + " (" + _size + " КБ)");
        }
    }

    // ============================================================
    // 3. КОМПОЗИТ (Composite) — складений об'єкт, що містить
    //    інші компоненти (і файли, і інші папки)
    // ============================================================
    class FolderItem : FileSystemItem
    {
        private FileSystemItem[] _children = new FileSystemItem[10];
        private int _count = 0;

        public FolderItem(string name) : base(name)
        {
        }

        public override void Add(FileSystemItem item)
        {
            if (_count >= _children.Length)
            {
                Console.WriteLine("Помилка: папка «" + _name + "» заповнена.");
                return;
            }

            _children[_count] = item;
            _count++;
        }

        public override void Remove(FileSystemItem item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_children[i] == item)
                {
                    // Зсуваємо наступні елементи вліво
                    for (int j = i; j < _count - 1; j++)
                    {
                        _children[j] = _children[j + 1];
                    }

                    _count--;
                    _children[_count] = null;
                    return;
                }
            }
        }

        // Розмір папки = сума розмірів усіх вкладених елементів.
        // Для вкладеної папки викликається її власний GetSize() — це рекурсія.
        public override int GetSize()
        {
            int total = 0;

            for (int i = 0; i < _count; i++)
            {
                total += _children[i].GetSize();
            }

            return total;
        }

        public override void Show(int level)
        {
            string indent = new string(' ', level * 4);
            Console.WriteLine(indent + "[+] " + _name + " (" + GetSize() + " КБ)");

            for (int i = 0; i < _count; i++)
            {
                _children[i].Show(level + 1);
            }
        }
    }

    // ============================================================
    // 4. КЛІЄНТ (Client)
    // ============================================================
    class Program
    {
        static void Main(string[] args)
        {
            // Створюємо листки (файли)
            FileItem photo1 = new FileItem("photo1.jpg", 2500);
            FileItem photo2 = new FileItem("photo2.jpg", 3100);
            FileItem report = new FileItem("report.docx", 450);
            FileItem old1 = new FileItem("old1.txt", 20);
            FileItem old2 = new FileItem("old2.txt", 35);
            FileItem readme = new FileItem("readme.txt", 5);

            // Створюємо композити (папки)
            FolderItem root = new FolderItem("Мої документи");
            FolderItem photos = new FolderItem("Фото");
            FolderItem work = new FolderItem("Робота");
            FolderItem archive = new FolderItem("Архів");

            // Збираємо дерево
            photos.Add(photo1);
            photos.Add(photo2);

            archive.Add(old1);
            archive.Add(old2);

            work.Add(report);
            work.Add(archive);

            root.Add(photos);
            root.Add(work);
            root.Add(readme);

            // 1. Виводимо все дерево одним викликом
            Console.WriteLine("--- Структура ---");
            root.Show(0);

            // 2. Розмір усього дерева і окремої гілки — той самий метод
            Console.WriteLine();
            Console.WriteLine("Загальний розмір: " + root.GetSize() + " КБ");
            Console.WriteLine("Розмір папки «Робота»: " + work.GetSize() + " КБ");
            Console.WriteLine("Розмір файлу «readme.txt»: " + readme.GetSize() + " КБ");

            // 3. Видаляємо файл — розміри папок перераховуються автоматично
            Console.WriteLine();
            Console.WriteLine("--- Після видалення photo2.jpg ---");
            photos.Remove(photo2);
            root.Show(0);

            // 4. Спроба додати елемент до файлу
            Console.WriteLine();
            readme.Add(old1);

            Console.WriteLine();
            Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}
