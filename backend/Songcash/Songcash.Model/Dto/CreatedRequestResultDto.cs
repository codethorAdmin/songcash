namespace Songcash.Model.Dto;

public class CreatedRequestResultDto
{
    public int InsertedId { get; set; }
    public required Request Request { get; set; }
}
