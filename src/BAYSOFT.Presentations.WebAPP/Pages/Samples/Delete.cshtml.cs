using BAYSOFT.Presentations.WebAPP.Abstractions.Pages;
using BAYSOFT.Core.Application.Default.Samples.Commands.DeleteSample;
using BAYSOFT.Core.Application.Default.Samples.Queries.GetSampleByID;
using BAYSOFT.Core.Domain.Entities.Default;
using Microsoft.AspNetCore.Mvc;
using ModelWrapper.Extensions.GetModel;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;

namespace BAYSOFT.Presentations.WebAPP.Pages.Samples
{
    public class DeleteModel : PageModelBase
    {
        [BindProperty]
        public SampleModel Input { get; set; }
        public DeleteModel()
        {

        }
        public async Task OnGetAsync(int id, CancellationToken cancellationToken = default(CancellationToken))
        {
            var query = new GetSampleByIDQuery();

            query.Project(x => x.SampleID = id);

            var response = await Mediator.Send(query, cancellationToken);

            Input = new SampleModel(response.GetModel());
        }

        public async Task<ActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var command = new DeleteSampleCommand();

            command.Project(x => x.SampleID = Input.SampleID);

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
            public int SampleID { get; set; }
            [Display(Name = "Description")]
            public string Description { get; set; }
            public SampleModel()
            {

            }
            public SampleModel(Sample sample)
            {
                SampleID = sample.SampleID;
                Description = sample.Description;
            }
        }
    }
}