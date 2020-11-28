using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radzen;
using shared = BlazingApple.Survey.Shared;
using BlazingApple.Survey.Shared;
using System.Net.Http;
using System.Net.Http.Json;

namespace BlazingApple
{
	public class SurveyService
	{
		static string API_PREFIX = "api/surveys";
		private readonly HttpClient _client;

		public SurveyService(HttpClient client)
		{
			_client = client;
		}

		// Survey

		#region public async Task<List<Survey>> GetAllSurveysAsync()
		public async Task<List<shared.Survey>> GetAllSurveysAsync()
		{
			List<shared.Survey> surveys = await _client.GetFromJsonAsync<List<shared.Survey>>(API_PREFIX);
			return surveys.OrderBy(x => x.SurveyName).ToList();
		}
		#endregion

		#region public Task<Survey> GetSurvey(int Id)
		public async Task<shared.Survey> GetSurvey(int Id)
		{
			return await _client.GetFromJsonAsync<shared.Survey>(API_PREFIX + "/" + Id);
		}
		#endregion

		#region public Task<Survey> CreateSurveyAsync(Survey NewSurvey)
		public async Task<shared.Survey> CreateSurveyAsync(shared.Survey NewSurvey)
		{
			NewSurvey.Id = 0;
			NewSurvey.DateCreated = DateTime.Now;
			HttpResponseMessage response = await _client.PostAsJsonAsync(API_PREFIX, NewSurvey);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadFromJsonAsync<shared.Survey>();
		}
		#endregion

		#region public Task<Survey> UpdateSurveyAsync(Survey objExistingSurvey)
		public async Task<shared.Survey> UpdateSurveyAsync(shared.Survey existingSurvey)
		{
			HttpResponseMessage response = await _client.PutAsJsonAsync(API_PREFIX + "/" + existingSurvey.Id, existingSurvey);
			response.EnsureSuccessStatusCode();

			return await response.Content.ReadFromJsonAsync<shared.Survey>(); ;

		}
		#endregion

		#region public Task<bool> DeleteSurveyAsync(Survey objExistingSurvey)
		public async Task<bool> DeleteSurveyAsync(shared.Survey existingSurvey)
		{
			try
			{
				HttpResponseMessage response = await _client.DeleteAsync(API_PREFIX + "/" + existingSurvey.Id);
				response.EnsureSuccessStatusCode();
			}
			catch (Exception e)
			{
				return false;
			}
			return true;
		}
		#endregion

		// Survey Item

		#region public async Task<List<SurveyItem>> GetAllSurveyItemsAsync(int SurveyId)
		public async Task<List<SurveyItem>> GetAllSurveyItemsAsync(int surveyId)
		{
			return await _client.GetFromJsonAsync<List<SurveyItem>>(API_PREFIX + "/" + surveyId + "/items");
		}
		#endregion

		#region public Task<SurveyItem> GetSurveyItemAsync(int SurveyItemId)
		public async Task<SurveyItem> GetSurveyItemAsync(int surveyItemId)
		{
			return await _client.GetFromJsonAsync<SurveyItem>(API_PREFIX + "/items/" + surveyItemId);
		}
		#endregion

		#region public Task<SurveyItem> CreateSurveyItemAsync(SurveyItem newSurveyItem)
		public async Task<SurveyItem> CreateSurveyItemAsync(SurveyItem newSurveyItem)
		{
			newSurveyItem.SurveyId = newSurveyItem.Survey.Id;
			newSurveyItem.Position = newSurveyItem.Survey.SurveyItems.Count;
			SurveyItem dtoSurveyItem = GetDTOItemCopy(newSurveyItem);


			HttpResponseMessage response = await _client.PostAsJsonAsync<SurveyItem>(API_PREFIX + "/items", dtoSurveyItem);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<SurveyItem>();
		}
		#endregion

		#region public Task<SurveyItem> UpdateSurveyItemAsync(SurveyItem objExistingSurveyItem)
		public async Task<SurveyItem> UpdateSurveyItemAsync(SurveyItem objExistingSurveyItem)
		{
			SurveyItem dtoSurveyItem = GetDTOItemCopy(objExistingSurveyItem);

			HttpResponseMessage response = await _client.PutAsJsonAsync(API_PREFIX + "/items/" + dtoSurveyItem.Id, dtoSurveyItem);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<SurveyItem>();
		}
		#endregion

		#region public Task<bool> DeleteSurveyItemAsync(SurveyItem objExistingSurveyItem)
		public async Task<bool> DeleteSurveyItemAsync(SurveyItem objExistingSurveyItem)
		{
			try
			{
				HttpResponseMessage response = await _client.DeleteAsync(API_PREFIX + "/items/" + objExistingSurveyItem.Id);
				response.EnsureSuccessStatusCode();
			}
			catch (Exception e)
			{
				return false;
			}

			return true;
		}
		#endregion

		// Survey Answers
		#region GetSurveyResults
		public async Task<int> GetSurveyResultsCount(int surveyId)
		{
			return await _client.GetFromJsonAsync<int>(API_PREFIX + "/results/" + surveyId + "/count");
		}
		public async Task<List<DTOSurveyItem>> GetSurveyResults(int surveyId, LoadDataArgs args)
		{
			HttpResponseMessage response = await _client.PostAsJsonAsync(API_PREFIX + "/results/" + surveyId, args);
			response.EnsureSuccessStatusCode();
			return await response.Content.ReadFromJsonAsync<List<DTOSurveyItem>>();
		}
		#endregion
		#region public Task<bool> CreateSurveyAnswersAsync(DTOSurvey paramDTOSurvey)
		public async Task<bool> CreateSurveyAnswersAsync(DTOSurvey paramDTOSurvey)
		{
			HttpResponseMessage response = await _client.PostAsJsonAsync(API_PREFIX + "/answers", paramDTOSurvey);
			response.EnsureSuccessStatusCode();
			return true;
		}
		#endregion
		private SurveyItem GetDTOItemCopy(SurveyItem itemToCopy)
		{
			SurveyItem dtoSurveyItem = new SurveyItem();
			dtoSurveyItem.Id = itemToCopy.Id;
			dtoSurveyItem.SurveyId = itemToCopy.SurveyId;
			dtoSurveyItem.Position = itemToCopy.Position;
			dtoSurveyItem.ItemLabel = itemToCopy.ItemLabel;
			dtoSurveyItem.ItemType = itemToCopy.ItemType;
			dtoSurveyItem.ItemValue = itemToCopy.ItemValue;
			dtoSurveyItem.Required = itemToCopy.Required;
			dtoSurveyItem.SurveyItemOptions = itemToCopy.SurveyItemOptions;
			return dtoSurveyItem;
		}

		public DTOSurvey ConvertSurveyToDTO(shared.Survey objSurvey)
		{
			DTOSurvey objDTOSurvey = new DTOSurvey();
			objDTOSurvey.Id = objSurvey.Id;
			objDTOSurvey.SurveyName = objSurvey.SurveyName;

			objDTOSurvey.SurveyItems = new List<DTOSurveyItem>();

			foreach (var SurveyItem in objSurvey.SurveyItems)
			{
				DTOSurveyItem objDTOSurveyItem = new DTOSurveyItem();

				objDTOSurveyItem.Id = SurveyItem.Id;
				objDTOSurveyItem.ItemLabel = SurveyItem.ItemLabel;
				objDTOSurveyItem.ItemType = SurveyItem.ItemType;
				objDTOSurveyItem.Position = SurveyItem.Position;
				objDTOSurveyItem.Required = SurveyItem.Required;

				objDTOSurveyItem.SurveyItemOptions =
					new List<DTOSurveyItemOption>();

				foreach (var SurveyItemOption in SurveyItem.SurveyItemOptions.OrderBy(x => x.Id))
				{
					DTOSurveyItemOption objDTOSurveyItemOption = new DTOSurveyItemOption();

					objDTOSurveyItemOption.Id = SurveyItemOption.Id;
					objDTOSurveyItemOption.OptionLabel = SurveyItemOption.OptionLabel;

					objDTOSurveyItem.SurveyItemOptions.Add(objDTOSurveyItemOption);
				}

				objDTOSurvey.SurveyItems.Add(objDTOSurveyItem);
			}
			return objDTOSurvey;
		}
	}
}
