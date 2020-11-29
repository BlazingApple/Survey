using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radzen;
using BlazingApple.Survey.Shared;
using apple=BlazingApple.Survey.Shared;
using BlazingAppleConsumer.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq.Dynamic.Core;

namespace BlazingAppleConsumer.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SurveysController : ControllerBase
	{
		private readonly ApplicationDbContext _context;

		public SurveysController(ApplicationDbContext context)
		{
			_context = context;
		}
		#region Surveys
		#region Get
		// GET: api/surveys
		[HttpGet()]
		public async Task<ActionResult<List<apple.Survey>>> GetSurveys()
		{
			List<apple.Survey> surveys = await _context.Surveys
				.Include(surv => surv.SurveyItems)
					.ThenInclude(item => item.SurveyItemOptions)
				.Include(surv => surv.SurveyItems)
					.ThenInclude(item => item.SurveyAnswers).ToListAsync();
			return surveys;
		}
		// GET: api/surveys/5
		[HttpGet("{id}")]
		public async Task<ActionResult<apple.Survey>> GetSurvey(int id)
		{
			apple.Survey survey = await _context.Surveys
				.Include(surv => surv.SurveyItems)
					.ThenInclude(item => item.SurveyItemOptions)
				.Include(surv => surv.SurveyItems)
					.ThenInclude(item => item.SurveyAnswers)
				.FirstOrDefaultAsync(surv => surv.Id == id);
			if (survey == null)
			{
				return NotFound();
			}

			return survey;
		}
		#endregion

		#region Put
		// PUT: api/Users/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
		public async Task<ActionResult<apple.Survey>> PutSurvey(int id, apple.Survey survey)
		{
			if (id != survey.Id)
			{
				return BadRequest();
			}
			_context.Entry(survey).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SurveyExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return CreatedAtAction("GetSurvey", new { id = survey.Id }, survey);
		}
		#endregion

		#region Post
		// POST: api/Users
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost]
		public async Task<ActionResult<apple.Survey>> PostSurvey(apple.Survey survey)
		{
			survey.UserId = GetUserId();
			if (SurveyExists(survey.Id))
			{
				await PutSurvey(survey.Id, survey);
				return Ok(survey);
			}
			else
			{
				_context.Surveys.Add(survey);
				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateException)
				{
					if (SurveyExists(survey.Id))
					{
						// This shouldn't really be hit, given the previous check.
						return Conflict();
					}
					else
					{
						throw;
					}
				}
				return CreatedAtAction("GetSurvey", new { id = survey.Id }, survey);
			}
		}
		#endregion

		#region Delete
		// DELETE: api/Surveys/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteSurvey(int id)
		{
			var survey = await _context.Surveys.FindAsync(id);
			if (survey == null)
			{
				return NotFound();
			}

			_context.Surveys.Remove(survey);
			await _context.SaveChangesAsync();

			return NoContent();
		}
		#endregion

		#region Helpers
		private bool SurveyExists(int id)
		{
			return _context.Surveys.Any(e => e.Id == id);
		}

		private bool SurveyItemExists(int id)
		{
			return _context.SurveyItems.Any(e => e.Id == id);
		}

		private bool SurveyAnswerExists(int id)
		{
			return _context.SurveyAnswers.Any(e => e.Id == id);
		}

		private string GetUserId()
		{
			foreach (Claim claim in HttpContext.User.Claims)
			{
				Console.WriteLine(claim.Type + ":\t\t-\t" + claim.Value);
			}
			return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
		}
		#endregion
		#endregion


		#region SurveyItems
		#region Get
		// GET: api/surveys/5/items
		[HttpGet("{id}/items")]
		public async Task<ActionResult<List<SurveyItem>>> GetSurveyItems(int id)
		{
			return await _context.SurveyItems.Where(item => item.Id == id).ToListAsync();
		}

		// GET: api/surveys/items/5
		[HttpGet("items/{id}")]
		public async Task<ActionResult<SurveyItem>> GetSurveyItem(int itemId)
		{
			return await _context.SurveyItems.FirstOrDefaultAsync(item => item.Id == itemId);
		}
		#endregion
		#region Put
		// PUT: api/surveys/items/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("items/{id}")]
		public async Task<IActionResult> PutSurveyItem(int id, SurveyItem surveyItem)
		{
			if (id != surveyItem.Id)
			{
				return BadRequest();
			}
			_context.Entry(surveyItem).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SurveyItemExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return CreatedAtAction("GetSurveyItem", new { id = surveyItem.Id }, surveyItem);
		}
		#endregion

		#region Post
		// POST: api/surveys/items
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost("items")]
		public async Task<ActionResult<apple.Survey>> PostSurveyItem(SurveyItem surveyItem)
		{
			if (SurveyItemExists(surveyItem.Id))
			{
				await PutSurveyItem(surveyItem.Id, surveyItem);
				return Ok(surveyItem);
			}
			else
			{
				_context.SurveyItems.Add(surveyItem);
				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateException)
				{
					if (SurveyItemExists(surveyItem.Id))
					{
						// This shouldn't really be hit, given the previous check.
						return Conflict();
					}
					else
					{
						throw;
					}
				}
				return CreatedAtAction("GetSurveyItem", new { id = surveyItem.Id }, surveyItem);
			}
		}
		#endregion

		#region Delete
		// DELETE: api/surveys/items/5
		[HttpDelete("items/{id}")]
		public async Task<IActionResult> DeleteSurveyItem(int id)
		{
			var surveyItem = await _context.SurveyItems.FindAsync(id);
			if (surveyItem == null)
			{
				return NotFound();
			}

			_context.SurveyItems.Remove(surveyItem);
			await _context.SaveChangesAsync();

			return NoContent();
		}
		#endregion
		#endregion

		#region Survey Answers
		#region Get
		// GET: api/surveys/answers
		[HttpGet("answers")]
		public async Task<ActionResult<List<SurveyAnswer>>> GetSurveyAnswers()
		{
			return await _context.SurveyAnswers.ToListAsync();
		}
		// GET: api/surveys/5
		[HttpGet("answers/{id}")]
		public async Task<ActionResult<SurveyAnswer>> GetSurveyAnswer(int id)
		{
			SurveyAnswer surveyAnswer = await _context.SurveyAnswers.FindAsync(id);
			if (surveyAnswer == null)
			{
				return NotFound();
			}

			return surveyAnswer;
		}
		#endregion

		#region Put
		// PUT: api/surveys/answers/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("answers/{id}")]
		public async Task<IActionResult> PutSurveyAnswer(int id, SurveyAnswer surveyAnswer)
		{
			if (id != surveyAnswer.Id)
			{
				return BadRequest();
			}
			_context.Entry(surveyAnswer).State = EntityState.Modified;

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!SurveyAnswerExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return NoContent();
		}
		#endregion

		#region Post
		// POST: api/surveys/answers
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPost("answers")]
		public async Task<ActionResult<bool>> PostSurveyAnswer(DTOSurvey paramDTOSurvey)
		{
			paramDTOSurvey.UserId = GetUserId();
			foreach (var SurveyItem in paramDTOSurvey.SurveyItems)
			{
				// Delete possible existing answer
				var ExistingAnswers = _context.SurveyAnswers
					.Where(x => x.SurveyItemId == SurveyItem.Id)
					.Where(x => x.UserId == paramDTOSurvey.UserId)
					.ToList();

				if (ExistingAnswers != null)
				{
					_context.SurveyAnswers.RemoveRange(ExistingAnswers);
					_context.SaveChanges();
				}

				// Save Answer

				if (SurveyItem.ItemType != "Multi-Select Dropdown")
				{
					SurveyAnswer NewSurveyAnswer = new SurveyAnswer();

					NewSurveyAnswer.AnswerValue = SurveyItem.AnswerValueString;
					NewSurveyAnswer.AnswerValueDateTime = SurveyItem.AnswerValueDateTime;
					NewSurveyAnswer.SurveyItemId = SurveyItem.Id;
					NewSurveyAnswer.UserId = paramDTOSurvey.UserId;

					_context.SurveyAnswers.Add(NewSurveyAnswer);
					_context.SaveChanges();
				}

				if (SurveyItem.AnswerValueList != null)
				{
					foreach (var item in SurveyItem.AnswerValueList)
					{
						SurveyAnswer NewSurveyAnswerValueList = new SurveyAnswer();

						NewSurveyAnswerValueList.AnswerValue = item;
						NewSurveyAnswerValueList.SurveyItemId = SurveyItem.Id;
						NewSurveyAnswerValueList.UserId = paramDTOSurvey.UserId;

						_context.SurveyAnswers.Add(NewSurveyAnswerValueList);
						_context.SaveChanges();
					}
				}
			}

			return true;
		}
		#endregion
		#endregion

		#region Results
		#region Get
		[HttpGet("results/{surveyId}/count")]
		public async Task<ActionResult<int>> GetSurveyResultsCount([FromRoute] int surveyId)
		{
			IQueryable<SurveyItem> query = _context.SurveyItems
							.Where(x => x.SurveyId == surveyId)
							.Where(x => x.ItemType != "Text Area")
							.Include(x => x.SurveyItemOptions)
							.OrderBy(x => x.Position).AsQueryable();

			List<SurveyItem> Results = query.ToList();
			return Results.Count;
		}
		[HttpPost("results/{surveyId}")]
		public async Task<ActionResult<List<DTOSurveyItem>>> GetSurveyResults([FromRoute] int surveyId, LoadDataArgs loadDataArgs)
		{
			IQueryable<SurveyItem> query = _context.SurveyItems
				.Where(x => x.SurveyId == surveyId)
				.Where(x => x.ItemType != "Text Area")
				.Include(x => x.SurveyItemOptions)
				.OrderBy(x => x.Position).AsQueryable();
			if (!string.IsNullOrEmpty(loadDataArgs.Filter))
			{
				query = query.Where(loadDataArgs.Filter);
			}
			if (!string.IsNullOrEmpty(loadDataArgs.OrderBy))
			{
				query = query.OrderBy(loadDataArgs.OrderBy);
			}
			List<SurveyItem> Results = query.Skip(loadDataArgs.Skip.Value).Take(loadDataArgs.Top.Value).ToList();

			List<DTOSurveyItem> SurveyResultsCollection = new List<DTOSurveyItem>();

			foreach (SurveyItem SurveyItem in Results)
			{
				DTOSurveyItem NewDTOSurveyItem = new DTOSurveyItem();

				NewDTOSurveyItem.Id = SurveyItem.Id;
				NewDTOSurveyItem.ItemLabel = SurveyItem.ItemLabel;
				NewDTOSurveyItem.ItemType = SurveyItem.ItemType;
				NewDTOSurveyItem.Position = SurveyItem.Position;
				NewDTOSurveyItem.Required = SurveyItem.Required;

				List<AnswerResponse> ColAnswerResponse = new List<AnswerResponse>();

				if ((SurveyItem.ItemType == "Date") || (SurveyItem.ItemType == "Date Time"))
				{
					var TempColAnswerResponse = _context.SurveyAnswers
						.Where(x => x.SurveyItemId == SurveyItem.Id)
						.GroupBy(n => n.AnswerValueDateTime)
						.Select(n => new AnswerResponse
						{
							OptionLabel = n.Key.Value.ToString(),
							Responses = n.Count()
						}
						).OrderBy(n => n.OptionLabel).ToList();

					foreach (var item in TempColAnswerResponse)
					{
						string ShortDate = item.OptionLabel;

						try
						{
							DateTime dtTempDate = Convert.ToDateTime(item.OptionLabel);
							ShortDate = dtTempDate.ToShortDateString();
						}
						catch
						{
							// use orginal string
						}

						ColAnswerResponse.Add(
							new AnswerResponse
							{
								OptionLabel = ShortDate,
								Responses = item.Responses
							}
							);
					}
				}
				else
				{
					ColAnswerResponse = _context.SurveyAnswers
						.Where(x => x.SurveyItemId == SurveyItem.Id)
						.GroupBy(n => n.AnswerValue)
						.Select(n => new AnswerResponse
						{
							OptionLabel = n.Key,
							Responses = n.Count()
						}
						).OrderBy(n => n.OptionLabel).ToList();
				}

				if (ColAnswerResponse.Count > 10)
				{
					// Only take top 10 
					NewDTOSurveyItem.AnswerResponses = ColAnswerResponse
						.OrderByDescending(x => x.Responses)
						.Take(10).ToList();

					// Put remaining items in "Other"
					var ColOtherItems = ColAnswerResponse.OrderByDescending(x => x.Responses).Skip(10).ToList();
					var SumOfOther = ColOtherItems.Sum(x => x.Responses);
					NewDTOSurveyItem.AnswerResponses.Add(new AnswerResponse() { OptionLabel = "Other", Responses = SumOfOther });
				}
				else
				{
					NewDTOSurveyItem.AnswerResponses = ColAnswerResponse;
				}

				SurveyResultsCollection.Add(NewDTOSurveyItem);
			}

			return Ok(SurveyResultsCollection);
		}
		#endregion
		#endregion
	}
}
