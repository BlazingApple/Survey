using BlazingApple.Survey.Shared;
using BlazingApple.Survey.Shared.DataTransferObjects;
using BlazingAppleConsumer.Server.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Threading.Tasks;
using Shared = BlazingApple.Survey.Shared;

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

		// DELETE: api/Surveys/5
		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteSurvey(Guid id)
		{
			Shared.Survey? survey = await _context.Surveys.FindAsync(id);
			if (survey == null)
			{
				return NotFound();
			}

			_context.Surveys.Remove(survey);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// DELETE: api/surveys/questions/5
		[HttpDelete("questions/{id}")]
		public async Task<IActionResult> DeleteQuestion(Guid id)
		{
			Question? question = await _context.Questions.FindAsync(id);
			if (question == null)
			{
				return NotFound();
			}

			_context.Questions.Remove(question);
			await _context.SaveChangesAsync();

			return NoContent();
		}

		// GET: api/surveys/questions/5
		[HttpGet("questions/{id}")]
		public async Task<ActionResult<Question>> GetQuestion(Guid questionId)
		{
			Question? question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == questionId);

			return question == null ? (ActionResult<Question>)NotFound() : (ActionResult<Question>)Ok(question);
		}

		// GET: api/surveys/5
		[HttpGet("{id}")]
		public async Task<ActionResult<Shared.Survey>> GetSurvey(Guid id)
		{
			Shared.Survey? survey = await _context.Surveys
				.Include(surv => surv.Questions)
					.ThenInclude(q => q.Options)
				.Include(surv => surv.Questions)
					.ThenInclude(question => question.Answers)
				.FirstOrDefaultAsync(surv => surv.Id == id);

			return survey == null ? (ActionResult<Shared.Survey>)NotFound() : (ActionResult<Shared.Survey>)Ok(survey);
		}

		// GET: api/surveys/5
		[HttpGet("answers/{id}")]
		public async Task<ActionResult<Answer>> GetSurveyAnswer(Guid id)
		{
			Answer? surveyAnswer = await _context.Answers.FindAsync(id);

			return surveyAnswer == null ? (ActionResult<Answer>)NotFound() : (ActionResult<Answer>)Ok(surveyAnswer);
		}

		// GET: api/surveys/answers
		[HttpGet("answers")]
		public async Task<ActionResult<List<Answer>>> GetSurveyAnswers()
		{
			return await _context.Answers.ToListAsync();
		}

		// GET: api/surveys/5/questions
		[HttpGet("{id}/questions")]
		public async Task<ActionResult<List<Question>>> GetQuestions(Guid surveyId)
		{
			return await _context.Questions.Where(question => question.SurveyId == surveyId).ToListAsync();
		}

		[HttpPost("results/{surveyId}")]
		public async Task<ActionResult<List<DTOQuestion>>> GetSurveyResults(Guid surveyId, LoadArgs loadArgs)
		{
			IQueryable<Question> query = _context.Questions
				.Where(x => x.SurveyId == surveyId && x.Type != QuestionType.TextArea)
				.Include(x => x.Options)
				.OrderBy(x => x.Position);

			List<Question> results = await query
				.Skip(loadArgs.Skip)
				.Take(loadArgs.Top)
				.ToListAsync();

			List<DTOQuestion> SurveyResultsCollection = new();

			foreach (Question question in results)
			{
				DTOQuestion newDtoQuestion = new()
				{
					Id = question.Id,
					Prompt = question.Prompt,
					Type = question.Type,
					Position = question.Position,
					Required = question.Required
				};

				List<AnswerResponse> answers = new();

				if (question.Type is QuestionType.Date or QuestionType.DateTime)
				{
					List<AnswerResponse> tempAnswerResponses = _context.Answers
						.Where(x => x.QuestionId == question.Id)
						.GroupBy(n => n.AnswerValueDateTime)
						.Select(n => new AnswerResponse
						{
							OptionLabel = (n.Key ?? DateTime.MinValue).ToString(),
							Responses = n.Count()
						}
						).OrderBy(n => n.OptionLabel).ToList();

					foreach (AnswerResponse? answer in tempAnswerResponses)
					{
						string ShortDate = answer.OptionLabel;

						try
						{
							DateTime dtTempDate = Convert.ToDateTime(answer.OptionLabel);
							ShortDate = dtTempDate.ToShortDateString();
						}
						catch
						{
							// use original string
						}

						answers.Add(
							new AnswerResponse
							{
								OptionLabel = ShortDate,
								Responses = answer.Responses
							});
					}
				}
				else
				{
					answers = _context.Answers
						.Where(x => x.QuestionId == question.Id)
						.GroupBy(n => n.AnswerValue)
						.Select(n => new AnswerResponse
						{
							OptionLabel = n.Key ?? string.Empty,
							Responses = n.Count()
						})
						.OrderBy(n => n.OptionLabel).ToList();
				}

				if (answers.Count > 10)
				{
					// Only take top 10
					newDtoQuestion.AnswerResponses = answers
						.OrderByDescending(x => x.Responses)
						.Take(10).ToList();

					// Put remaining questions in "Other"
					List<AnswerResponse> otherAnswers = answers.OrderByDescending(x => x.Responses).Skip(10).ToList();
					double sumOfOther = otherAnswers.Sum(x => x.Responses);
					newDtoQuestion.AnswerResponses.Add(new AnswerResponse() { OptionLabel = "Other", Responses = sumOfOther });
				}
				else
				{
					newDtoQuestion.AnswerResponses = answers;
				}

				SurveyResultsCollection.Add(newDtoQuestion);
			}

			return Ok(SurveyResultsCollection);
		}

		[HttpGet("results/{surveyId}/count")]
		public async Task<ActionResult<int>> GetSurveyResultsCount([FromRoute] Guid surveyId)
		{
			int resultsCount = await _context.Answers
				.AsNoTracking()
				.Where(a => a.Question!.SurveyId == surveyId)
				.Select(a => a.UserId)
				.Distinct()
				.CountAsync();

			return resultsCount;
		}

		// GET: api/surveys
		[HttpGet()]
		public async Task<ActionResult<List<Shared.Survey>>> GetSurveys()
		{
			List<Shared.Survey> surveys = await _context.Surveys
				.Include(surv => surv.Questions)
					.ThenInclude(question => question.Options)
				.Include(surv => surv.Questions)
					.ThenInclude(question => question.Answers).ToListAsync();
			return surveys;
		}

		// POST: api/surveys/questions 
		[HttpPost("questions")]
		public async Task<ActionResult<Shared.Survey>> PostQuestion(Question question)
		{
			if (QuestionExists(question.Id))
			{
				await PutQuestion(question.Id, question);
				return Ok(question);
			}
			else
			{
				_context.Questions.Add(question);
				try
				{
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateException)
				{
					if (QuestionExists(question.Id))
					{
						return Conflict();
					}
					else
					{
						throw;
					}
				}
				return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question);
			}
		}

		// POST: api/Users 
		[HttpPost]
		public async Task<ActionResult<Shared.Survey>> PostSurvey(Shared.Survey survey)
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
						return Conflict();
					}
					else
					{
						throw;
					}
				}
				return CreatedAtAction(nameof(GetSurvey), new { id = survey.Id }, survey);
			}
		}

		// POST: api/surveys/answers 
		[HttpPost("answers")]
		public async Task<ActionResult<bool>> PostSurveyAnswer(DTOSurvey dtoSurvey)
		{
			dtoSurvey.UserId = GetUserId();
			if (dtoSurvey.Questions == null)
			{
				return false;
			}

			foreach (DTOQuestion question in dtoSurvey.Questions)
			{
				// Delete possible existing answer
				List<Answer> ExistingAnswers = await _context.Answers
					.Where(x => x.QuestionId == question.Id && x.UserId == dtoSurvey.UserId)
					.ToListAsync();

				if (ExistingAnswers != null)
				{
					_context.Answers.RemoveRange(ExistingAnswers);
					await _context.SaveChangesAsync();
				}

				// Save Answer

				if (question.Type != QuestionType.DropdownMultiSelect)
				{
					Answer NewSurveyAnswer = new()
					{
						AnswerValue = question.AnswerValueString,
						AnswerValueDateTime = question.AnswerValueDateTime,
						QuestionId = question.Id,
						UserId = dtoSurvey.UserId
					};

					_context.Answers.Add(NewSurveyAnswer);
					await _context.SaveChangesAsync();
				}

				if (question.AnswerValueList != null)
				{
					foreach (string answer in question.AnswerValueList)
					{
						Answer NewSurveyAnswerValueList = new()
						{
							AnswerValue = answer,
							QuestionId = question.Id,
							UserId = dtoSurvey.UserId
						};

						_context.Answers.Add(NewSurveyAnswerValueList);
						await _context.SaveChangesAsync();
					}
				}
			}

			return true;
		}

		// PUT: api/surveys/questions/5 
		[HttpPut("questions/{id}")]
		public async Task<IActionResult> PutQuestion(Guid id, Question question)
		{
			if (id != question.Id)
			{
				return BadRequest();
			}

			if (question.Options is not null)
			{
				IEnumerable<Guid> optionIds = question.Options.Select(o => o.Id);
				IQueryable<QuestionOption> existingOptions = _context.QuestionOptions
					.Where(lqo => lqo.QuestionId == id && !optionIds.Contains(lqo.Id));

				_context.Entry(question).State = EntityState.Modified;

				_context.QuestionOptions.RemoveRange(existingOptions);
				IEnumerable<QuestionOption> newOptions = question.Options.Where(o => o.Id == default);

				foreach (QuestionOption option in newOptions)
				{
					_context.Entry(option).State = EntityState.Added;
				}
			}
			else
			{
				_context.Entry(question).State = EntityState.Modified;
			}

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!QuestionExists(id))
				{
					return NotFound();
				}
				else
				{
					throw;
				}
			}

			return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question);
		}

		// PUT: api/Surveys/5
		[HttpPut("{id}")]
		public async Task<ActionResult<Shared.Survey>> PutSurvey(Guid id, Shared.Survey survey)
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

			return CreatedAtAction(nameof(GetSurvey), new { id = survey.Id }, survey);
		}

		// PUT: api/surveys/answers/5 
		[HttpPut("answers/{id}")]
		public async Task<IActionResult> PutSurveyAnswer(Guid id, Answer surveyAnswer)
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

		private string? GetUserId()
		{
			foreach (Claim claim in HttpContext.User.Claims)
			{
				Console.WriteLine(claim.Type + ":\t\t-\t" + claim.Value);
			}
			return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
		}

		private bool QuestionExists(Guid id)
		{
			return _context.Questions.Any(e => e.Id == id);
		}

		private bool SurveyAnswerExists(Guid id)
		{
			return _context.Answers.Any(e => e.Id == id);
		}

		private bool SurveyExists(Guid id)
		{
			return _context.Surveys.Any(e => e.Id == id);
		}
	}
}
