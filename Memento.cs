using System;
using System.Collections.Generic;

// Memento (Знімок) - зберігає стан Творця. 
// Поля доступні лише для читання.
public class EditorMemento
{
    public string State { get; }
    public DateTime DateSaved { get; }

    public EditorMemento(string state)
    {
        State = state;
        DateSaved = DateTime.Now;
    }
}

// Originator (Творець) - об'єкт, стан якого ми зберігаємо
public class TextEditor
{
    private string _text = string.Empty;

    public void Type(string words)
    {
        _text += words;
    }

    public string GetText() => _text;

    // Створює знімок поточного стану
    public EditorMemento Save()
    {
        Console.WriteLine($"[Editor] Saving state: '{_text}'");
        return new EditorMemento(_text);
    }

    // Відновлює стан зі знімка
    public void Restore(EditorMemento memento)
    {
        _text = memento.State;
        Console.WriteLine($"[Editor] Restored state to: '{_text}'");
    }
}

// Caretaker (Опікун) - зберігає історію знімків, але не торкається їхнього вмісту
public class History
{
    private Stack<EditorMemento> _mementos = new Stack<EditorMemento>();
    private TextEditor _editor;

    public History(TextEditor editor)
    {
        _editor = editor;
    }

    public void Backup()
    {
        _mementos.Push(_editor.Save());
    }

    public void Undo()
    {
        if (_mementos.Count == 0)
        {
            Console.WriteLine("[History] No states to undo.");
            return;
        }

        var memento = _mementos.Pop();
        Console.WriteLine($"[History] Undoing operation. State was saved at {memento.DateSaved}");
        _editor.Restore(memento);
    }
}

public class MementoProgram
{
    public static void Main()
    {
        var editor = new TextEditor();
        var history = new History(editor);

        // Редагуємо та зберігаємо
        editor.Type("Hello");
        history.Backup();

        editor.Type(" World!");
        history.Backup();

        editor.Type(" How are you?");
        
        Console.WriteLine($"\nCurrent Text: {editor.GetText()}\n");

        // Скасовуємо дії
        history.Undo();
        Console.WriteLine($"Text after Undo: {editor.GetText()}\n");

        history.Undo();
        Console.WriteLine($"Text after second Undo: {editor.GetText()}");
    }
}