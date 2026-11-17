using System;

namespace MementoPatternDemo
{
    // ============================================================
    // 1. ІНТЕРФЕЙС ЗНІМКА (Memento)
    //    Це "вузький" інтерфейс: через нього опікун бачить лише
    //    короткий опис знімка, але не може дістатись до даних.
    // ============================================================
    interface IMemento
    {
        string GetInfo();
    }

    // ============================================================
    // 2. ВЛАСНИК (Originator) — об'єкт, стан якого ми зберігаємо
    // ============================================================
    class TextEditor
    {
        private string _text = "";
        private int _fontSize = 12;

        public void Type(string words)
        {
            _text = _text + words;
        }

        public void SetFontSize(int size)
        {
            _fontSize = size;
        }

        public void Show()
        {
            Console.WriteLine("Текст: «" + _text + "», шрифт: " + _fontSize);
        }

        // Створює знімок поточного стану
        public IMemento Save()
        {
            return new Snapshot(_text, _fontSize);
        }

        // Відновлює стан зі знімка
        public void Restore(IMemento memento)
        {
            Snapshot snapshot = memento as Snapshot;

            if (snapshot == null)
            {
                Console.WriteLine("Помилка: невідомий тип знімка.");
                return;
            }

            _text = snapshot.GetText();
            _fontSize = snapshot.GetFontSize();
        }

        // ========================================================
        // Конкретний знімок — ВКЛАДЕНИЙ ПРИВАТНИЙ клас.
        // Зовні його не видно, тому лише TextEditor може читати
        // збережені дані. Інші бачать тільки IMemento.
        // ========================================================
        private class Snapshot : IMemento
        {
            private string _text;
            private int _fontSize;

            public Snapshot(string text, int fontSize)
            {
                _text = text;
                _fontSize = fontSize;
            }

            public string GetText()
            {
                return _text;
            }

            public int GetFontSize()
            {
                return _fontSize;
            }

            public string GetInfo()
            {
                string preview;

                if (_text.Length == 0)
                {
                    preview = "(порожньо)";
                }
                else if (_text.Length > 15)
                {
                    preview = _text.Substring(0, 15) + "...";
                }
                else
                {
                    preview = _text;
                }

                return "[" + _text.Length + " симв., шрифт " + _fontSize + "] " + preview;
            }
        }
    }

    // ============================================================
    // 3. ОПІКУН (Caretaker) — зберігає знімки й керує історією.
    //    Він НЕ знає, що всередині знімків.
    // ============================================================
    class History
    {
        private TextEditor _editor;
        private IMemento[] _stack = new IMemento[10];
        private int _count = 0;

        public History(TextEditor editor)
        {
            _editor = editor;
        }

        // Зберегти поточний стан редактора
        public void Backup()
        {
            if (_count == _stack.Length)
            {
                // Історія заповнена — видаляємо найстаріший знімок
                for (int i = 0; i < _count - 1; i++)
                {
                    _stack[i] = _stack[i + 1];
                }
                _count--;
            }

            _stack[_count] = _editor.Save();
            _count++;
        }

        // Повернутись до останнього збереженого стану
        public void Undo()
        {
            if (_count == 0)
            {
                Console.WriteLine("Немає збережених станів для відновлення.");
                return;
            }

            _count--;
            IMemento memento = _stack[_count];
            _stack[_count] = null;

            Console.WriteLine("Відновлюємо: " + memento.GetInfo());
            _editor.Restore(memento);
        }

        public void ShowHistory()
        {
            Console.WriteLine("Збережені знімки (" + _count + "):");

            for (int i = 0; i < _count; i++)
            {
                Console.WriteLine("  " + (i + 1) + ". " + _stack[i].GetInfo());
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
            TextEditor editor = new TextEditor();
            History history = new History(editor);

            // Крок 1: зберігаємо порожній стан, потім змінюємо
            history.Backup();
            editor.Type("Привіт");
            editor.Show();

            // Крок 2
            history.Backup();
            editor.Type(", світе!");
            editor.Show();

            // Крок 3: змінюємо і текст, і шрифт
            history.Backup();
            editor.SetFontSize(20);
            editor.Type(" Це тест.");
            editor.Show();

            Console.WriteLine();
            history.ShowHistory();

            // Відміна дій одна за одною
            Console.WriteLine();
            Console.WriteLine("--- Відміна 1 ---");
            history.Undo();
            editor.Show();

            Console.WriteLine();
            Console.WriteLine("--- Відміна 2 ---");
            history.Undo();
            editor.Show();

            Console.WriteLine();
            Console.WriteLine("--- Відміна 3 ---");
            history.Undo();
            editor.Show();

            Console.WriteLine();
            Console.WriteLine("--- Відміна 4 (історія порожня) ---");
            history.Undo();

            Console.WriteLine();
            Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
            Console.ReadKey();
        }
    }
}
