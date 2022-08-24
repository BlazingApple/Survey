using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radzen;
using BlazingApple.Survey.Shared;

using shared = BlazingApple.Survey.Shared;

using BlazingAppleConsumer.Server.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Linq.Dynamic.Core;
using BlazingApple.Survey.Shared.DataTransferObjects;

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

        // DELETE: api/surveys/items/5
        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteSurveyItem(int id)
        {
            Question? surveyItem = await _context.Questions.FindAsync(id);
            if (surveyItem == null)
                return NotFound();

            _context.Questions.Remove(surveyItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/surveys/items/5
        [HttpGet("items/{id}")]
        public async Task<ActionResult<Question>> GetQuestion(Guid questionId)
        {
            Question? question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
                return NotFound();

            return Ok(question);
        }

        // GET: api/surveys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<shared.Survey>> GetSurvey(Guid id)
        {
            shared.Survey? survey = await _context.Surveys
                .Include(surv => surv.Questions)
                    .ThenInclude(q => q.Options)
                .Include(surv => surv.Questions)
                    .ThenInclude(item => item.Answers)
                .FirstOrDefaultAsync(surv => surv.Id == id);

            if (survey == null)
                return NotFound();

            return Ok(survey);
        }

        // GET: api/surveys/5
        [HttpGet("answers/{id}")]
        public async Task<ActionResult<Answer>> GetSurveyAnswer(int id)
        {
            Answer? surveyAnswer = await _context.Answers.FindAsync(id);

            if (surveyAnswer == null)
                return NotFound();

            return Ok(surveyAnswer);
        }

        // GET: api/surveys/answers
        [HttpGet("answers")]
        public async Task<ActionResult<List<Answer>>> GetSurveyAnswers()
        {
            return await _context.Answers.ToListAsync();
        }

        // GET: api/surveys/5/items
        [HttpGet("{id}/items")]
        public async Task<ActionResult<List<Question>>> GetSurveyItems(Guid id)
        {
            return await _context.Questions.Where(item => item.Id == id).ToListAsync();
        }

        [HttpPost("results/{surveyId}")]
        public async Task<ActionResult<List<DTOQuestion>>> GetSurveyResults(Guid surveyId, LoadDataArgs loadDataArgs)
        {
            IQueryable<Question> query = _context.Questions
                .Where(x => x.SurveyId == surveyId && x.Type != QuestionType.TextArea)
                .Include(x => x.Options)
                .OrderBy(x => x.Position);

            if (!string.IsNullOrEmpty(loadDataArgs.Filter))
                query = query.Where(loadDataArgs.Filter);

            if (!string.IsNullOrEmpty(loadDataArgs.OrderBy))
                query = query.OrderBy(loadDataArgs.OrderBy);

            List<Question> results = await query
                .Skip(loadDataArgs.Skip!.Value)
                .Take(loadDataArgs.Top.Value)
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
                            OptionLabel = n.Key.Value.ToString(),
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
                            // use orginal string
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
                            OptionLabel = n.Key,
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

                    // Put remaining items in "Other"
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
            IQueryable<Question> query = _context.Questions
                            .Where(x => x.SurveyId == surveyId)
                            .Where(x => x.Type != QuestionType.TextArea)
                            .Include(x => x.Options)
                            .OrderBy(x => x.Position);

            List<Question> Results = await query.ToListAsync();
            return Results.Count;
        }

        // GET: api/surveys
        [HttpGet()]
        public async Task<ActionResult<List<shared.Survey>>> GetSurveys()
        {
            List<shared.Survey> surveys = await _context.Surveys
                .Include(surv => surv.Questions)
                    .ThenInclude(item => item.Options)
                .Include(surv => surv.Questions)
                    .ThenInclude(item => item.Answers).ToListAsync();
            return surveys;
        }

        // POST: api/surveys/items To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("items")]
        public async Task<ActionResult<shared.Survey>> PostQuestion(Question question)
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
                        return Conflict();
                    else
                        throw;
                }
                return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question);
            }
        }

        // POST: api/Users To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<shared.Survey>> PostSurvey(shared.Survey survey)
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
                        return Conflict();
                    else
                        throw;
                }
                return CreatedAtAction(nameof(GetSurvey), new { id = survey.Id }, survey);
            }
        }

        // POST: api/surveys/answers To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("answers")]
        public async Task<ActionResult<bool>> PostSurveyAnswer(DTOSurvey dtoSurvey)
        {
            dtoSurvey.UserId = GetUserId();
            if (dtoSurvey.Questions == null)
                return false;

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
                    foreach (string item in question.AnswerValueList)
                    {
                        Answer NewSurveyAnswerValueList = new()
                        {
                            AnswerValue = item,
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

        // PUT: api/surveys/items/5 To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("items/{id}")]
        public async Task<IActionResult> PutQuestion(Guid id, Question question)
        {
            if (id != question.Id)
                return BadRequest();

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
                    return NotFound();
                else
                    throw;
            }

            return CreatedAtAction(nameof(GetQuestion), new { id = question.Id }, question);
        }

        // PUT: api/Users/5 To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<shared.Survey>> PutSurvey(Guid id, shared.Survey survey)
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

        // PUT: api/surveys/answers/5 To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        private string GetUserId()
        {
            foreach (Claim claim in HttpContext.User.Claims)
            {
                Console.WriteLine(claim.Type + ":\t\t-\t" + claim.Value);
            }
            return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private bool QuestionExists(Guid id) => _context.Questions.Any(e => e.Id == id);

        private bool SurveyAnswerExists(Guid id) => _context.Answers.Any(e => e.Id == id);

        private bool SurveyExists(Guid id) => _context.Surveys.Any(e => e.Id == id);
    }
}
