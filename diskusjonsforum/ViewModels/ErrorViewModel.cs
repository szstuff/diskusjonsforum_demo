namespace diskusjonsforum.ViewModels;

public class ErrorViewModel
{
    //Generic ErrorViewModel
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}

