using BlazingApple.Survey.Components.Services;
using Microsoft.AspNetCore.Components;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>Allows editing a <see cref="Question" /></summary>
public partial class EditQuestion : ComponentBase
{
    private readonly IEnumerable<QuestionType> FormTypes = Enum.GetValues<QuestionType>();
    private string _newOption = string.Empty;
    private bool ShowPopup = false;

    private string strError = "";

    /// <summary>Triggered when the edit option is closed.</summary>
    [Parameter]
    public EventHandler? OnClose { get; set; }

    [Inject]
    private ISurveyClient Service { get; set; } = null!;

    /// <summary>Whether to open the edit option inline or in a modal.</summary>
    [Parameter]
    public bool PromptInline { get; set; }

    /// <summary>The question being edited.</summary>
    [Parameter, EditorRequired]
    public Question? SelectedQuestion { get; set; }

    /// <summary>Adds a response option to the survey question being edited.</summary>
    private void AddOption()
    {
        if (!string.IsNullOrWhiteSpace(_newOption) && SelectedQuestion?.Options != null)
        {
            SelectedQuestion.Options
                .Add(new QuestionOption
                {
                    OptionLabel = _newOption
                });

            _newOption = string.Empty;
        }
    }

    private async Task AddOrUpdate()
    {
        if (SelectedQuestion == null)
        {
            return;
        }

        try
        {
            ItemRequest request;
            if (SelectedQuestion.Id == default)
            {
                SelectedQuestion = await Service.CreateQuestionAsync(SelectedQuestion);
                request = new ItemRequest(UserAction.Create, SelectedQuestion);
            }
            else
            {
                SelectedQuestion = await Service.UpdateQuestion(SelectedQuestion);
                request = new ItemRequest(UserAction.Update, SelectedQuestion);
            }

            dialogService.Close(request);
            CloseQuestion(false);
        }
        catch (Exception ex)
        {
            strError = ex.GetBaseException().Message;
        }
    }

    /// <summary>Closes the popup.</summary>
    private void ClosePopup()
    {
        ShowPopup = false;
    }

    private void CloseQuestion(bool closeDialog = true)
    {
        OnClose?.Invoke(this, EventArgs.Empty);

        if (closeDialog)
        {
            dialogService.Close();
        }
    }

    private async Task Delete()
    {
        if (SelectedQuestion == null)
        {
            throw new InvalidOperationException("Disallowed null reference to edited question.");
        }

        bool result = await @Service.DeleteQuestionAsync(SelectedQuestion);

        ItemRequest? request = null;

        if (result)
        {
            request = new(UserAction.Delete, SelectedQuestion);
        }

        dialogService.Close(request);
        CloseQuestion(false);
    }

    /// <summary>Opens the popup.</summary>
    private void OpenPopup()
    {
        ShowPopup = true;
    }

    /// <summary>Remove the option from the list of items.</summary>
    /// <param name="option"></param>
    private void RemoveOption(QuestionOption option)
    {
        if (SelectedQuestion?.Options != null)
        {
            SelectedQuestion.Options.Remove(option);
        }
    }
}
