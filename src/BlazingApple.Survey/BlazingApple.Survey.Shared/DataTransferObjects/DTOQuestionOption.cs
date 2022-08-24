using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Shared.DataTransferObjects;

/// <summary>DTO for <see cref="QuestionOption" /></summary>
public class DTOQuestionOption
{
    /// <inheritdoc cref="QuestionOption.Id" />
    public Guid Id { get; set; }

    /// <inheritdoc cref="QuestionOption.OptionLabel" />
    public string? OptionLabel { get; set; }
}
