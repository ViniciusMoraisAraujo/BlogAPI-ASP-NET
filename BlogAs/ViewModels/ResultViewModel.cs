namespace BlogAs.ViewModels;

public class ResultViewModel<T>
{
    public T Data { get; private set; }
    public List<string> Errors { get; private set; } = new();
    public string ErrorCode { get; set; }
    
    public ResultViewModel(T data, List<string> errors)
    {
        Data = data;
        Errors = errors;
    }
    
    public ResultViewModel(T data)
    {
        Data = data;
    }

    public ResultViewModel(T data, string erros)
    {
        Data = data;
        Errors.Add(erros);   
    }
    
    public ResultViewModel(List<string> errors)
    {
        Errors = errors;   
    }
    
    public ResultViewModel(List<string> errors, string errorCode = null)
    {
        Errors = errors;   
        ErrorCode = errorCode;  
    }
    
    public ResultViewModel(string error)
        => Errors.Add(error);
    
}