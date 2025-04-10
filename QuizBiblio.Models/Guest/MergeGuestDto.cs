using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBiblio.Models.Guest;

public class MergeGuestDto
{
    public required string GuestId { get; set; }

    public required string UserId { get; set; }
}
