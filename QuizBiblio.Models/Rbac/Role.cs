using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizBiblio.Models.Rbac;

public class Role(int id, string name, string uid)
{
    public int Id { get; init; } = id;

    public string Name { get; init; } = name;

    public string Uid { get; init; } = uid;

}