using Avalonia.Media;
using System;

namespace Lemon.Toolkit.Models
{
    public struct ConsoleTextModel
    {
        public ConsoleTextModel(string text,
            FontWeight weight = FontWeight.Normal,
            Brush? brush = null)
        {
            Id = Guid.NewGuid();
            Text = text;
            TextWeight = weight;
            brush ??= new SolidColorBrush(Colors.DodgerBlue);
            TextBrush = brush;
        }
        public Guid Id { get; }
        public string Text { get; }
        public FontWeight TextWeight { get; }
        public Brush TextBrush { get; }
        //public string Time => $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}";
    }
}
