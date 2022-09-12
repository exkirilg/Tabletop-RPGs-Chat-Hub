using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class DiceRoll : IComparable<DiceRoll>
{
    [Required]
    public Message Message { get; init; }

    [Key]
    public Guid DiceRollId { get; init; } = Guid.NewGuid();

    [Required]
    public int Position { get; init; }

    [Required(AllowEmptyStrings = false)]
    public string Dice { get; init; }

    [Required]
    public int Result { get; init; }

    public DiceRoll()
    {

    }
    public DiceRoll(Message message, int position, string dice, int result)
    {
        Message = message;
        Position = position;
        Dice = dice;
        Result = result;
    }

    public DiceRollDTO ToDTO()
    {
        return new DiceRollDTO(Position, Dice, Result);
    }

    public int CompareTo(DiceRoll? other)
    {
        if (other is null) return 1;

        return Position.CompareTo(other.Position);
    }
}
