using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace RRHH.WebApi.ModelState
{
    public class ModelStateError
    {

        public IList<string> Get(ModelStateDictionary modelState)
        {
            IList<string> output = new List<string>();

            var errors = modelState
                          .Where(x => x.Value.Errors.Count > 0)
                          .Select(x => new
                          {
                              Messages = x.Value.Errors.Select(e => e.ErrorMessage).ToList()
                          });

            foreach (var item in errors)
            {
                string _result = item.Messages.FirstOrDefault();
                output.Add(_result);
            }

            return output;
        }

    }
}
