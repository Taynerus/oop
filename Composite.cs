using System;
using System.Collections.Generic;

// Загальний компонент (Інтерфейс)
public interface IFileSystemComponent
{
    string Name { get; }
    void Display(int depth = 0);
    int GetSize();
}

// "Листок" (Leaf) - не має вкладених елементів
public class FileComponent : IFileSystemComponent
{
    public string Name { get; }
    private int _size;

    public FileComponent(string name, int size)
    {
        Name = name;
        _size = size;
    }

    public void Display(int depth = 0)
    {
        Console.WriteLine(new string('-', depth) + " File: " + Name + $" ({_size} KB)");
    }

    public int GetSize() => _size;
}

// "Компонувальник" (Composite) - гілка, що може містити інші компоненти
public class DirectoryComponent : IFileSystemComponent
{
    public string Name { get; }
    private List<IFileSystemComponent> _children = new List<IFileSystemComponent>();

    public DirectoryComponent(string name)
    {
        Name = name;
    }

    public void Add(IFileSystemComponent component)
    {
        _children.Add(component);
    }

    public void Remove(IFileSystemComponent component)
    {
        _children.Remove(component);
    }

    // Делегує виклик усім нащадкам
    public void Display(int depth = 0)
    {
        Console.WriteLine(new string('-', depth) + "+ Directory: " + Name);
        foreach (var component in _children)
        {
            component.Display(depth + 2);
        }
    }

    // Підсумовує розмір усіх нащадків
    public int GetSize()
    {
        int totalSize = 0;
        foreach (var component in _children)
        {
            totalSize += component.GetSize();
        }
        return totalSize;
    }
}

public class CompositeProgram
{
    public static void Main()
    {
        // Створення файлів
        var file1 = new FileComponent("document.txt", 10);
        var file2 = new FileComponent("image.jpg", 1500);
        var file3 = new FileComponent("script.sh", 2);

        // Створення директорій
        var docsDir = new DirectoryComponent("Documents");
        var rootDir = new DirectoryComponent("Root");

        // Побудова дерева
        docsDir.Add(file1);
        docsDir.Add(file2);
        
        rootDir.Add(docsDir);
        rootDir.Add(file3);

        // Клієнтський код працює з деревом через спільний інтерфейс
        Console.WriteLine("File System Structure:");
        rootDir.Display();
        
        Console.WriteLine($"\nTotal Size: {rootDir.GetSize()} KB");
    }
}