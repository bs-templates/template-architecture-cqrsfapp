using BAYSOFT.Presentations.WebAPP.Abstractions.Pages;
using BAYSOFT.Core.Application.Default.Samples.Commands.PutSample;
using BAYSOFT.Core.Application.Default.Samples.Queries.GetSampleById;
using BAYSOFT.Core.Domain.Entities.Default;
using Microsoft.AspNetCore.Mvc;
using ModelWrapper.Extensions.GetModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Presentations.WebAPP.Pages.Samples
{
    public class EditModel : PageModelBase
    {
        [BindProperty]
        public SampleModel Input { get; set; }
        public EditModel()
        {

        }
        public async Task OnGetAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = new GetSampleByIdQuery();

            query.Project(x => x.Id = id);

            var response = await Mediator.Send(query, cancellationToken);

            Input = new SampleModel(response.GetModel());
        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var command = new PutSampleCommand();

            command.Project(x =>
            {
                x.Id = Input.Id;
                x.Description = Input.Description;
            });

            var response = await Mediator.Send(command);

            if (response.StatusCode != 200)
            {
                ModelState.AddModelError("", response.Message);
                return Page();
            }

            return RedirectToPage("./Index");
        }

        public class SampleModel
        {
            [Required]
            public int Id { get; set; }
            [Required(AllowEmptyStrings = false, ErrorMessage = "{0} is required!")]
            [Display(Name = "Description")]
            [DataType(DataType.Text)]
            [StringLength(100, MinimumLength = 3, ErrorMessage = "{0} must be greater than {1} and smaller than {2}!")]
            public string Description { get; set; }
            public SampleModel()
            {

            }
            public SampleModel(Sample sample)
            {
                Id = sample.Id;
                Description = sample.Description;
            }
        }
    }
}