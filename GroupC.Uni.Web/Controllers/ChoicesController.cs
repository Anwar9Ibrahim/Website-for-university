using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GroupC.Uni.Core.Entities;
using GroupC.Uni.Infrastructure;
using GroupC.Uni.Core.Interfaces;
using GroupC.Uni.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace GroupC.Uni.Web.Controllers
{
    [Authorize(Roles = "ChoicesManagement")]
    public class ChoicesController : Controller
    {
        private readonly IQuestionService _questionService;
        private readonly IChoiceService _ChoiceService;

        public ChoicesController(IQuestionService questionService, IChoiceService ChoiceService)
        {
            _questionService = questionService;
            _ChoiceService = ChoiceService;
        }
        [AllowAnonymous]
        // GET: Choices
        public async Task<IActionResult> Index()
        {
            var ChoiceList = await _ChoiceService.ListActiveSyncWithQuestion();
            var QueistionViewModelList = new List<ChoiceViewModels>();
            foreach (var Choice in ChoiceList)
            {
                ChoiceViewModels currQVM = new ChoiceViewModels()
                {
                    Id = Choice.Id,
                    Text = Choice.Text,
                    Type = Choice.Type,
                    Question = Choice.Question,
                    QuestionId = Choice.QuestionId
                };
                QueistionViewModelList.Add(currQVM);
            }
            return View(QueistionViewModelList);
        }

        // GET: Choices/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var _choice = await _ChoiceService.getByIdWithQuestion(id);
            if (_choice == null)
            {
                return NotFound();
            }
            ChoiceViewModels _choiceViewModel = new ChoiceViewModels()
            {
                Id = _choice.Id,
                Text = _choice.Text,
                Type = _choice.Type,
                Status = _choice.Status,
                Question = _choice.Question,
                QuestionId=_choice.QuestionId
            };

            return View(_choiceViewModel);
        }

        // GET: Choices/Create
        public IActionResult Create()
        {
            List<Question> l = _questionService.GetAllAsList();
            ViewData["QuestionId"] = new SelectList(l, "Id", "Text");
            return View();
        }

        // POST: Choices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Text,Type,QuestionId,Id,")] ChoiceViewModels ChoiceViewModels)
        {
            if (ModelState.IsValid)
            {
                ChoiceViewModels.Id = Guid.NewGuid();
                Question _question = await _questionService.GetByIdAsync(ChoiceViewModels.QuestionId);
                Choice _choice = new Choice()
                {
                    Id = ChoiceViewModels.Id,
                    Text = ChoiceViewModels.Text,
                    Type = ChoiceViewModels.Type,
                    Question = _question,
                    QuestionId = ChoiceViewModels.QuestionId,

                };
                await _ChoiceService.CreateChoiceAsync(_choice);
                return RedirectToAction(nameof(Index));
            }

            ViewData["QuestionId"] = new SelectList(_questionService.GetAllAsList(), "Id", "Text", ChoiceViewModels.QuestionId);
            return View(ChoiceViewModels);
        }

        // GET: Choices/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var choice = await _ChoiceService.getByIdWithQuestion(id);
            if (choice == null)
            {
                return NotFound();
            }
            List<Choice> l = _ChoiceService.GetAllAsList();
            ViewData["QuestionId"] = new SelectList(l, "Id", "Name", choice.QuestionId);
            ChoiceViewModels _choiceViewModel = new ChoiceViewModels()
            {

                Id = choice.Id,
                Text = choice.Text,
                Type = choice.Type,
                Status = choice.Status,
                Question = choice.Question,
                QuestionId = choice.QuestionId
            };
            return View(_choiceViewModel);
        }

        // POST: Choices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Type,Text,QuestionId,Id,LastUpdateDate,CreationDate,Status")] ChoiceViewModels ChoiceViewModel)
        {
            if (id != ChoiceViewModel.Id)
            {
                return NotFound();
            }
            Choice _choice = new Choice()
            {
                Id = ChoiceViewModel.Id,
                Text = ChoiceViewModel.Text,
                Type = ChoiceViewModel.Type
                
            };
            if (ModelState.IsValid)
            {
                try
                {
                    await _ChoiceService.specialUpdateAsync(_choice);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChoiceExists(_choice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionId"] = new SelectList(_ChoiceService.GetAllAsList(), "Id", "Name", ChoiceViewModel.QuestionId);
            return View(ChoiceViewModel);
        }

        // GET: Choices/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var choice = await _ChoiceService.getByIdWithQuestion(id);

            if (choice == null)
            {
                return NotFound();
            }
            ChoiceViewModels _choiceViewModel = new ChoiceViewModels()
            {

                Id = choice.Id,
                Text = choice.Text,
                Type = choice.Type,
                Status = choice.Status,
                Question = choice.Question,
                QuestionId = choice.QuestionId
            };
            return View(_choiceViewModel);
        }

        // POST: Choices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var choice = await _ChoiceService.getByIdWithQuestion(id);
            await _ChoiceService.Deactivate(choice);

            return RedirectToAction(nameof(Index));
        }

        private bool ChoiceExists(Guid id)
        {
            var choice = _ChoiceService.GetByIdAsync(id);
            if (choice != null)
                return true;
            return false;
        }
    }
}
