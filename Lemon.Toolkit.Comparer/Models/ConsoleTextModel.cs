using Avalonia.Media;
using System;

namespace Lemon.Toolkit.Models
{
    public class ConsoleTextModel
    {
        public ConsoleTextModel(Guid guid,
            string text,
            FontWeight weight = FontWeight.Normal,
            SolidColorBrush? brush = null)
        {
            Id = guid;
            Text = text;
            TextWeight = weight;
            brush ??= new SolidColorBrush(Colors.DodgerBlue);
            TextBrush = brush;
        }
        public Guid Id { get; }
        public string Text { get; }
        public FontWeight TextWeight { get; }
        public SolidColorBrush TextBrush { get; }

        // ReSharper disable once MemberCanBeMadeStatic.Global
#pragma warning disable CA1822
        public string Time => $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}";
#pragma warning restore CA1822
    }
}
