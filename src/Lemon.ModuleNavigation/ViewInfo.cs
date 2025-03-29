namespace Lemon.ModuleNavigation;

public struct ViewDiscription
{
    public string ViewClassName 
    { 
        get; 
        set; 
    }
    public string ViewKey 
    { 
        get; 
        set; 
    }
    public Type ViewType 
    { 
        get; 
        set; 
    }
    public Type ViewModelType 
    { 
        get; 
        set; 
    }

    public override string ToString()
    {
        return $"{ViewKey}:{ViewClassName}-{ViewModelType.Name}";
    }
}
