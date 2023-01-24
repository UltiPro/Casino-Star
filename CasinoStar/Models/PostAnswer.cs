using System.ComponentModel.DataAnnotations;

namespace Models.PostAnswer;

public class PostAnswer
{
    [Display(Name = "statusCode")]
    public bool statusCode { get; set; }
    [Display(Name = "message")]
    public string? message { get; set; }
}