using BAYSOFT.Presentations.WebAPP.Abstractions.Pages;
using BAYSOFT.Core.Application.Default.Samples.Commands.PostSample;
using BAYSOFT.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace BAYSOFT.Presentations.WebAPP.Pages.Samples
{
    public class CreateModel : PageModelBase
    {
        [BindProperty]
        public InputModel Input { get; set; }
        public CreateModel()
        {

        }
        public void OnGet()
        {

        }

        public async Task<ActionResult> OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                var command = new PostSampleCommand();

                command.Project(x => x.Description = Input.Description);

                var response = await Mediator.Send(command);

                if (response.StatusCode != 200)
                {
                    throw new BusinessException(response.Message);
                }

                return RedirectToPage("./Index");
            }
            catch(BusinessException bex)
            {
                ModelState.AddModelError("", bex.Message);
                if (bex.InnerExceptions != null && bex.InnerExceptions.Count > 0)
                {
                    foreach(var innerException in bex.InnerExceptions)
                    {
                        ModelState.AddModelError("", innerException.Message);
                    }
                }
                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return Page();
            }
        }

        public class InputModel
        {
            [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required!")]
            [Display(Name = "Description")]
            [DataType(DataType.Text)]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} must be greater than {1} and smaller than {2}!")]
            public string Description { get; set; }
            public InputModel()
            {

            }
        }
    }
}