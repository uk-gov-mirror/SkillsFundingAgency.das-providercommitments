using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SFA.DAS.ProviderCommitments.Web.Extensions
{
    public class SilentModelBinder2 : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var valueResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);

            var converter = TypeDescriptor.GetConverter(bindingContext.ModelType);

            try
            {
                var result = converter.ConvertFrom(valueResult.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(result);
                //bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult);
            }
            catch (Exception)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                //bindingContext.ModelState.SetModelValue(bindingContext.ModelName, valueResult.FirstValue);
            }

            return Task.CompletedTask;
        }
    }
}
