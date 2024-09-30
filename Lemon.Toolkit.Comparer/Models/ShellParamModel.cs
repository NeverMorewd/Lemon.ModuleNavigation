namespace Lemon.Toolkit.Models
{
    public struct ShellParamModel
    {
        public bool IsProcessing 
        { 
            get; 
            set; 
        }
        public override string ToString()
        {
            return $"IsProcessing:{IsProcessing}";
        }
    }
}
