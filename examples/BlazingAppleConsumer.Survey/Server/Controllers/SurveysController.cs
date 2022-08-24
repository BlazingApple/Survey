using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Radzen;
using BlazingApple.Survey.Shared;

using apple = BlazingApple.Survey.Shared;

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
            var surveyItem = await _context.SurveyItems.FindAsync(id);
            if (surveyItem == null)
            {
                return NotFound();
            }

            _context.SurveyItems.Remove(surveyItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/surveys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<apple.Survey>> GetSurvey(Guid id)
        {
            apple.Survey survey = await _context.Surveys
                .Include(surv => surv.SurveyItems)
                    .ThenInclude(item => item.Options)
                .Include(surv => surv.SurveyItems)
                    .ThenInclude(item => item.Answers)
                .FirstOrDefaultAsync(surv => surv.Id == id);
            if (survey == null)
            {
                return NotFound();
            }

            return survey;
        }

        // GET: api/surveys/5
        [HttpGet("answers/{id}")]
        public async Task<ActionResult<Answer>> GetSurveyAnswer(int id)
        {
            Answer surveyAnswer = await _context.SurveyAnswers.FindAsync(id);
            if (surveyAnswer == null)
            {
                return NotFound();
            }

            return surveyAnswer;
        }

        // GET: api/surveys/answers
        [HttpGet("answers")]
        public async Task<ActionResult<List<Answer>>> GetSurveyAnswers()
        {
            return await _context.SurveyAnswers.ToListAsync();
        }

        // GET: api/surveys/items/5
        [HttpGet("items/{id}")]
        public async Task<ActionResult<Question>> GetSurveyItem(Guid itemId)
        {
            return await _context.SurveyItems.FirstOrDefaultAsync(item => item.Id == itemId);
        }

        // GET: api/surveys/5/items
        [HttpGet("{id}/items")]
        public async Task<ActionResult<List<Question>>> GetSurveyItems(Guid id)
        {
            return await _context.SurveyItems.Where(item => item.Id == id).ToListAsync();
        }

        [HttpPost("results/{surveyId}")]
        public async Task<ActionResult<List<DTOQuestion>>> GetSurveyResults(Guid surveyId, LoadDataArgs loadDataArgs)
        {
            IQueryable<Question> query = _context.SurveyItems
                .Where(x => x.SurveyId == surveyId && x.Type != QuestionType.TextArea)
                .Include(x => x.Options)
                .OrderBy(x => x.Position);

            if (!string.IsNullOrEmpty(loadDataArgs.Filter))
                query = query.Where(loadDataArgs.Filter);

            if (!string.IsNullOrEmpty(loadDataArgs.OrderBy))
                query = query.OrderBy(loadDataArgs.OrderBy);

            List<Question> Results = await query
                .Skip(loadDataArgs.Skip.Value)
                .Take(loadDataArgs.Top.Value)
                .ToListAsync();

            List<DTOQuestion> SurveyResultsCollection = new();

            foreach (Question SurveyItem in Results)
            {
                DTOQuestion NewDTOSurveyItem = new()
                {
                    Id = SurveyItem.Id,
                    Prompt = SurveyItem.Prompt,
                    Type = SurveyItem.Type,
                    Position = SurveyItem.Position,
                    Required = SurveyItem.Required
                };

                List<AnswerResponse> ColAnswerResponse = new();

                if (SurveyItem.Type is QuestionType.Date or QuestionType.DateTime)
                {
                    var TempColAnswerResponse = _context.SurveyAnswers
                        .Where(x => x.QuestionId == SurveyItem.Id)
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
                        .Where(x => x.QuestionId == SurveyItem.Id)
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

        [HttpGet("results/{surveyId}/count")]
        public async Task<ActionResult<int>> GetSurveyResultsCount([FromRoute] Guid surveyId)
        {
            IQueryable<Question> query = _context.SurveyItems
                            .Where(x => x.SurveyId == surveyId)
                            .Where(x => x.Type != QuestionType.TextArea)
                            .Include(x => x.Options)
                            .OrderBy(x => x.Position);

            List<Question> Results = await query.ToListAsync();
            return Results.Count;
        }

        // GET: api/surveys
        [HttpGet()]
        public async Task<ActionResult<List<apple.Survey>>> GetSurveys()
        {
            List<apple.Survey> surveys = await _context.Surveys
                .Include(surv => surv.SurveyItems)
                    .ThenInclude(item => item.Options)
                .Include(surv => surv.SurveyItems)
                    .ThenInclude(item => item.Answers).ToListAsync();
            return surveys;
        }

        // POST: api/Users To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/surveys/answers To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("answers")]
        public async Task<ActionResult<bool>> PostSurveyAnswer(DTOSurvey paramDTOSurvey)
        {
            paramDTOSurvey.UserId = GetUserId();
            foreach (var SurveyItem in paramDTOSurvey.Questions)
            {
                // Delete possible existing answer
                List<Answer> ExistingAnswers = await _context.SurveyAnswers
                    .Where(x => x.QuestionId == SurveyItem.Id)
                    .Where(x => x.UserId == paramDTOSurvey.UserId)
                    .ToListAsync();

                if (ExistingAnswers != null)
                {
                    _context.SurveyAnswers.RemoveRange(ExistingAnswers);
                    await _context.SaveChangesAsync();
                }

                // Save Answer

                if (SurveyItem.Type != QuestionType.DropdownMultiSelect)
                {
                    Answer NewSurveyAnswer = new()
                    {
                        AnswerValue = SurveyItem.AnswerValueString,
                        AnswerValueDateTime = SurveyItem.AnswerValueDateTime,
                        QuestionId = SurveyItem.Id,
                        UserId = paramDTOSurvey.UserId
                    };

                    _context.SurveyAnswers.Add(NewSurveyAnswer);
                    _context.SaveChanges();
                }

                if (SurveyItem.AnswerValueList != null)
                {
                    foreach (var item in SurveyItem.AnswerValueList)
                    {
                        Answer NewSurveyAnswerValueList = new()
                        {
                            AnswerValue = item,
                            QuestionId = SurveyItem.Id,
                            UserId = paramDTOSurvey.UserId
                        };

                        _context.SurveyAnswers.Add(NewSurveyAnswerValueList);
                        _context.SaveChanges();
                    }
                }
            }

            return true;
        }

        // POST: api/surveys/items To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("items")]
        public async Task<ActionResult<apple.Survey>> PostSurveyItem(Question surveyItem)
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

        // PUT: api/Users/5 To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<apple.Survey>> PutSurvey(Guid id, apple.Survey survey)
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

        // PUT: api/surveys/items/5 To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("items/{id}")]
        public async Task<IActionResult> PutSurveyItem(Guid id, Question surveyItem)
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

        private string GetUserId()
        {
            foreach (Claim claim in HttpContext.User.Claims)
            {
                Console.WriteLine(claim.Type + ":\t\t-\t" + claim.Value);
            }
            return HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        private bool SurveyAnswerExists(Guid id) => _context.SurveyAnswers.Any(e => e.Id == id);

        private bool SurveyExists(Guid id) => _context.Surveys.Any(e => e.Id == id);

        private bool SurveyItemExists(Guid id) => _context.SurveyItems.Any(e => e.Id == id);
    }
}
