using System;

// Інтерфейс для клонування
public interface ICloneablePrototype<T>
{
    T Clone(); // Поверхневе копіювання
    T DeepClone(); // Глибоке копіювання (якщо є вкладені посилання)
}

// Допоміжний клас для демонстрації глибокого копіювання
public class AuthorInfo
{
    public string Name { get; set; }
    
    public AuthorInfo(string name)
    {
        Name = name;
    }
}

// Конкретний прототип
public class Report : ICloneablePrototype<Report>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public AuthorInfo Author { get; set; }

    public Report(string title, string content, AuthorInfo author)
    {
        Title = title;
        Content = content;
        Author = author;
    }

    // Поверхневе клонування: копіюються лише значення типу value та посилання
    public Report Clone()
    {
        return (Report)this.MemberwiseClone();
    }

    // Глибоке клонування: створюються нові екземпляри для вкладених об'єктів
    public Report DeepClone()
    {
        Report clone = (Report)this.MemberwiseClone();
        clone.Author = new AuthorInfo(this.Author.Name);
        return clone;
    }

    public void Print()
    {
        Console.WriteLine($"Report: '{Title}', Author: {Author.Name}, Content: {Content}");
    }
}

public class PrototypeProgram
{
    public static void Main()
    {
        var originalReport = new Report("Annual Sales", "Sales were up by 15%", new AuthorInfo("John Doe"));
        
        // Клонування
        var clonedReport = originalReport.Clone();
        var deepClonedReport = originalReport.DeepClone();

        // Змінюємо внутрішній об'єкт (Author) у клонованому екземплярі
        clonedReport.Author.Name = "Jane Smith";
        
        // Демонстрація результатів
        Console.WriteLine("Original (affected by shallow clone change):");
        originalReport.Print(); // Автор зміниться на Jane Smith через поверхневе копіювання
        
        Console.WriteLine("\nShallow Clone:");
        clonedReport.Print();
        
        Console.WriteLine("\nDeep Clone (unaffected):");
        deepClonedReport.Print(); // Автор залишиться John Doe
    }
}