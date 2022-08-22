﻿using BlazingApple.Survey.Shared;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazingApple.Survey.Components.Internal;

/// <summary>Allows editing a <see cref="SurveyItem" /></summary>
public partial class EditSurveyItem : OwningComponentBase<SurveyService>
{
    private readonly IEnumerable<string> FormTypes = new string[]
        { "Text Box", "Text Area", "Date", "Date Time", "Dropdown", "Multi-Select Dropdown" };
    private string _newOption = string.Empty;
    private bool ShowPopup = false;

    private string strError = "";

    /// <summary>Triggered when the edit option is closed.</summary>
    [Parameter]
    public EventHandler? OnClose { get; set; }

    /// <summary>Whether to open the edit option inline or in a modal.</summary>
    [Parameter]
    public bool PromptInline { get; set; }

    /// <summary>The question being edited.</summary>
    [Parameter, EditorRequired]
    public SurveyItem? SelectedSurveyItem { get; set; }

    /// <summary>Adds a response option to the survey question being edited.</summary>
	private void AddOption()
    {
        if (!string.IsNullOrWhiteSpace(_newOption) && SelectedSurveyItem?.SurveyItemOptions != null)
        {
            SelectedSurveyItem.SurveyItemOptions
                .Add(new SurveyItemOption
                {
                    OptionLabel = _newOption
                });

            _newOption = string.Empty;
        }
    }

    /// <summary>Closes the popup.</summary>
	private void ClosePopup() => ShowPopup = false;

    private void CloseSurveyItem(bool closeDialog = true)
    {
        if (OnClose != null)
            OnClose.Invoke(this, EventArgs.Empty);

        if (closeDialog)
            dialogService.Close();
    }

    private async Task DeleteSurveyItem()
    {
        if (SelectedSurveyItem == null)
            throw new InvalidOperationException("Disallowed null reference to edited question.");

        bool result = await @Service.DeleteSurveyItemAsync(SelectedSurveyItem);

        // Set the Id to -1 so we know it is deleted
        SelectedSurveyItem.Id = "-1";

        dialogService.Close(SelectedSurveyItem);
        dialogService.Close(SelectedSurveyItem);
        CloseSurveyItem(false);
    }

    /// <summary>Opens the popup.</summary>
    private void OpenPopup() => ShowPopup = true;

    /// <summary>Remove the option from the list of items.</summary>
    /// <param name="option"></param>
    private void RemoveOption(SurveyItemOption option)
    {
        if (SelectedSurveyItem?.SurveyItemOptions != null)
            SelectedSurveyItem.SurveyItemOptions.Remove(option);
    }

    private async Task UpdateSurveyItem()
    {
        if (SelectedSurveyItem == null)
            return;

        try
        {
            if (SelectedSurveyItem.Id == default)
                SelectedSurveyItem = await Service.CreateSurveyItemAsync(SelectedSurveyItem);
            else
                SelectedSurveyItem = await Service.UpdateSurveyItemAsync(SelectedSurveyItem);

            dialogService.Close(SelectedSurveyItem);
            CloseSurveyItem(false);
        }
        catch (Exception ex)
        {
            strError = ex.GetBaseException().Message;
        }
    }
}