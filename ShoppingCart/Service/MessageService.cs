public class MessageService
{
    private string _message;
    private string _messageType;

    public event Action OnMessageChanged;

    public void ShowMessage(string message, string messageType = "info")
    {
        _message = message;
        _messageType = messageType;
        OnMessageChanged?.Invoke();
    }

    public (string Message, string Type) GetMessage()
    {
        return (_message, _messageType);
    }

    public void Clear()
    {
        _message = null;
        _messageType = null;
        OnMessageChanged?.Invoke();
    }

}
